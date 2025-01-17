using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
using Models;

public class Client
{
    public static int dataBufferSize = 4096;

    public bool IsAutorized
    {
        get;
        private set;
    }
    public int id;
    public string Username;
    public Player Player;
    public TCP tcp;
    public UDP udp;

    private string playfabID;

    public Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }

    public void SetPlayFabID(string _id)
    {
        playfabID = _id;
    }

    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.clients[id].Disconnect();
                    Debug.Log("Dispoze Disconnect");
                    return;
                }   
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);
                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.clients[id].Disconnect();
            }
        }
        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }

        public void SendData(Packet _packet)
        {
            Server.SendUDPData(endPoint, _packet);
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    Server.packetHandlers[_packetId](id, _packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SetUsername(string username)
    {
        IsAutorized = true;
        Username = username;
    }
    
    /// <summary>
    /// Метод для создания игрока и отправки его в игру
    /// </summary>
    /// <param name="_playerName">Имя игрока</param>
    public void SendIntoGame(Player player)
    {
        Player = player;
        Player.Initialize(id, Username);
        //отправляем информацию о всех игроках клиенту, чтобы они появились в его мире
        foreach (Client client in Server.clients.Values)
        {
            Player player1 = client.Player;
            if (player1 != null)
            {
                //Except ourselves
                if (client.id != id)
                {
                    ServerSend.SpawnPlayer(id,
                        new PlayerSpawnModel(player1.ID, player1.Username, player1.transform,
                            player1.MAXHealth, player1.MAXMana, player1.StartMana,
                            player1.StartHealth));
                }
            }
        }
        //аналогичное, но для клиента
        foreach (Client _client in Server.clients.Values)
        {
            Player player1 = Player;
            if (_client.Player != null)
            {
                ServerSend.SpawnPlayer(_client.id,
                    new PlayerSpawnModel(player1.ID, player1.Username, player1.transform,
                        player1.MAXHealth, player1.MAXMana, player1.StartMana,
                        player1.StartHealth));
            }
        }

    }

    public void SendIntoGame(Player player, Dictionary<string, bool> listTeam)
    {
        Player = player;
        Player.Initialize(id, Username);
        //отправляем информацию о всех игроках клиенту, чтобы они появились в его мире
        foreach (Client client in Server.clients.Values)
        {
            Player player1 = client.Player;
            if (player1 != null)
            {
                //Except ourselves
                if (client.id != id)
                {
                    ServerSend.SpawnPlayer(id,
                        new PlayerSpawnModel(player1.ID, player1.Username, player1.transform,
                            player1.MAXHealth, player1.MAXMana, player1.StartMana,
                            player1.StartHealth));
                    ServerSend.PlayerTeam(client.id,id,listTeam[player1.Username]);
                }
            }
        }
        //аналогичное, но для клиента
        foreach (Client _client in Server.clients.Values)
        {
            Player player1 = Player;
            if (_client.Player != null)
            {
                ServerSend.SpawnPlayer(_client.id,
                    new PlayerSpawnModel(player1.ID, player1.Username, player1.transform,
                        player1.MAXHealth, player1.MAXMana, player1.StartMana,
                        player1.StartHealth));
                ServerSend.PlayerTeam(id,_client.id,listTeam[player1.Username]);

            }
        }

    }
    /// <summary>
    /// Метод для создания игрока и отправки его в игру
    /// </summary>
    /// <param name="_playerName">Имя игрока</param>
    public void Respawn(Player player)
    {
        Player = player;
        Player.Initialize(id, Username);
        //аналогичное, но для клиента
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.Player != null)
            {
                ServerSend.SpawnPlayer(_client.id,
                    new PlayerSpawnModel(Player.ID, Player.Username, Player.transform,
                        Player.MAXHealth, Player.MAXMana, Player.StartMana,
                        Player.StartHealth));
            }
        }
    }
    /// <summary>
    /// Метод для создания игрока и отправки его в игру
    /// </summary>
    /// <param name="_playerName">Имя игрока</param>
    public void Respawn(Player player, bool isRed)
    {
        Player = player;
        Player.Initialize(id, Username);
        //аналогичное, но для клиента
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.Player != null)
            {
                ServerSend.SpawnPlayer(_client.id,
                    new PlayerSpawnModel(Player.ID, Player.Username, Player.transform,
                        Player.MAXHealth, Player.MAXMana, Player.StartMana,
                        Player.StartHealth));
                ServerSend.PlayerTeam(id,_client.id, isRed);
            }
        }
    }

    private void Disconnect()
    {
        IsAutorized = false;
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            if (Player)UnityEngine.Object.Destroy(Player.gameObject);
            Player = null;
        });


        tcp.Disconnect();
        udp.Disconnect();

        Server.OnPlayerRemoved.Invoke(playfabID);
        Server.OnClientRemoved.Invoke(this);
        ServerSend.PlayerDisconnected(id);
    }
}