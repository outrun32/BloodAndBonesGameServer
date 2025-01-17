using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Delegates;
using UnityEngine.Events;

class Server
{
    public static int MaxPlayers { get; private set; }
    public static int PortTCP { get; private set; }
    public static int PortUDP { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;
    public static ReturnClient OnClientAdded;
    public static ReturnClient OnClientRemoved;
    public static PlayerEvent OnPlayerAdded = new PlayerEvent();
    public static PlayerEvent OnPlayerRemoved = new PlayerEvent();

    public class PlayerEvent : UnityEvent<string> {}

    public static void Start(int _maxPlayers)
    {
        MaxPlayers = _maxPlayers;

        Debug.Log("Starting Server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, PortTCP);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        udpListener = new UdpClient(PortUDP);
        udpListener.Client.IOControl((IOControlCode)Constants.SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server started on port TCP: {PortTCP} and port UDP: {PortUDP}.");
    }

    public static void Start(int _maxPlayers, int _portTCP, int _portUDP)
    {
        SetPort(_portTCP, _portUDP);
        Start(_maxPlayers);
    }

    public static void SetPort(int _portTCP, int _portUDP)
    {
        PortTCP = _portTCP;
        PortUDP = _portUDP;
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }
        }

        Debug.Log($"{ _client.Client.RemoteEndPoint} failed to connect to server: Server full");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }
            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }

                //Means that user just connected and we should handle his first connection
                if (clients[_clientId].udp.endPoint == null)
                {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                //or else you can literally stole someones id and throw packets from his client
                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error receiving UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to {_clientEndPoint} via UDP : {_ex}");
        }
    }

    public static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.playerInput, ServerHandle.PlayerInput }
        };

        Debug.Log("Initialized packets");
    }

    public struct ReceiveAuthenticateMessage
    {
        public string PlayFabId;
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
        Application.Quit();
    }
}
