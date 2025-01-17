using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        string playfabID = _packet.ReadString();
        Debug.Log(playfabID);
        Server.OnPlayerAdded.Invoke(playfabID);
        
        Server.clients[_fromClient].SetUsername(_username);
        Server.clients[_fromClient].SetPlayFabID(playfabID);
        Server.OnClientAdded.Invoke(Server.clients[_fromClient]);
        Debug.Log($"{ Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {_fromClient} with nickname {_username}.");

        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\"(ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
    }

    public static void PlayerInput(int fromClient, Packet packet)
    {
        Server.clients[fromClient].Player.SetInput(packet.ReadInputModel());
    }
    
}
