using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, PlayerSpawnModel _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.ID);
            _packet.Write(_player.Username);
            _packet.Write(_player.Transform.position);
            _packet.Write(_player.Transform.rotation);
            _packet.Write(_player.MaxHealth);
            _packet.Write(_player.MaxMana);
            _packet.Write(_player.StartHealth);
            _packet.Write(_player.StartMana);
                
            SendTCPData(_toClient, _packet);
        }
    }
    public static void PlayerTeam(int id, bool isRed)
    {
        using (Packet _packet = new Packet((int)ServerPackets.setTeam))
        {
            _packet.Write(id);
            _packet.Write(isRed);

            SendUDPDataToAll(_packet);
        }
    }
    public static void PlayerPosition(PlayerSendingDataModel _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.ID);
            _packet.Write(_player.Transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(PlayerSendingDataModel _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.ID);
            _packet.Write(_player.Transform.rotation);

            SendUDPDataToAll( _packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {

        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerAnimation(PlayerSendingDataModel _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerAnimation))
        {
            _packet.Write(_player.ID);
            _packet.Write(_player.AnimationModel);
            SendUDPDataToAll(_packet);
        }
    }
    public static void PlayerInfo(PlayerSendingDataModel _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerInfo))
        {
            _packet.Write(_player.ID);
            _packet.Write(_player.Health);
            _packet.Write(_player.Mana);
            SendUDPDataToAll(_packet);
        }
    }

    public static void SendPlayerData(PlayerSendingDataModel playerSendingDataModel)
    {
        PlayerPosition(playerSendingDataModel);
        PlayerRotation(playerSendingDataModel);
        PlayerAnimation(playerSendingDataModel);
        PlayerInfo(playerSendingDataModel);
    }
    public static void PlayerDeath(int id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDeath))
        {
            _packet.Write(id);
            SendTCPDataToAll(_packet);
        }
    }

    public static void SetCountTimer(int idTimer, int value)
    {
        using (Packet _packet = new Packet((int)ServerPackets.setTimerCounter))
        {
            _packet.Write(idTimer);
            _packet.Write(value);
            SendTCPDataToAll(_packet);
        }
    }

    public static void StartSession()
    {
        using (Packet _packet = new Packet((int)ServerPackets.startSession))
        {
            SendTCPDataToAll(_packet);
        }
    }
    public static void EndSession(EndSessionModel endSessionModel)
    {
        using (Packet _packet = new Packet((int)ServerPackets.endSession))
        {
            _packet.Write(endSessionModel.BlueTeam.Count);
            foreach (KeyValuePair<string,(PlayerDataModel, Controllers.Character.Character)> player in endSessionModel.BlueTeam)
            {
                _packet.Write(player.Key);
                _packet.Write(player.Value.Item1.KillCount);
                _packet.Write(player.Value.Item1.DeathCount);
                _packet.Write(player.Value.Item1.Score);
            }
            _packet.Write(endSessionModel.RedTeam.Count);
            foreach (KeyValuePair<string,(PlayerDataModel, Controllers.Character.Character)> player in endSessionModel.RedTeam)
            {
                _packet.Write(player.Key);
                _packet.Write(player.Value.Item1.KillCount);
                _packet.Write(player.Value.Item1.DeathCount);
                _packet.Write(player.Value.Item1.Score);
            }
            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
