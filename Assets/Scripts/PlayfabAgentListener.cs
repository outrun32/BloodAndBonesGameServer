using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;


public class PlayfabAgentListener : MonoBehaviour
{

    private List<ConnectedPlayer> _connectedPlayers;

    public bool Debugging = true;

    public float timeToShutDown;

    private bool _canBeShutDown = false;


    void Start()
    {
        _connectedPlayers = new List<ConnectedPlayer>();
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.IsDebugging = Debugging;
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
        PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutDown;
        PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;

        Server.OnPlayerAdded.AddListener(OnPlayerAdded);
        Server.OnPlayerRemoved.AddListener(OnPlayerRemoved);

        StartCoroutine(ReadyForPlayers());

        if (shutdownServerInTime)
            StartCoroutine(StopServerInXSeconds(timeToShutDown));
    }

    private void OnServerActive()
    {
        if (!NetworkManager.instance.startServerInNManager)
            Server.Start(10, NetworkManager.instance.PortTCP, NetworkManager.instance.PortUDP);
    }

    private void OnShutDown()
    {
        Server.Stop();
    }

    IEnumerator StopServerInXSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Server.Stop();
    }

    private void OnAgentError(string error)
    {
        Debug.Log(error);
    }

    private void OnPlayerRemoved(string playfabId)
    {
        ConnectedPlayer player = _connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, System.StringComparison.OrdinalIgnoreCase));
        _connectedPlayers.Remove(player);
        if(_connectedPlayers.Count == 0 && _canBeShutDown)
        {
            Debug.Log("Last player left, shutting down server");
            StartCoroutine(StopServerInXSeconds(timeToShutDown));
        }
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
    }

    private void OnPlayerAdded(string playfabId)
    {
        _connectedPlayers.Add(new ConnectedPlayer(playfabId));
        _canBeShutDown = true;
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
    }

    private IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }
}
