using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;


public class PlayfabAgentListener : MonoBehaviour
{

    private List<ConnectedPlayer> _connectedPlayers;
    public bool Debugging = true;

    void Start()
    {
        _connectedPlayers = new List<ConnectedPlayer>();
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.IsDebugging = Debugging;
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
        PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutDown;
        PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;

        StartCoroutine(ReadyForPlayers());
    }

    private void OnServerActive()
    {
        Server.Start(10, 26950);
    }

    private void OnShutDown()
    {
        Server.Stop();
    }

    private void OnAgentError(string error)
    {
        Debug.Log(error);
    }

    private void OnPlayerRemoved()
    {

    }

    private void OnPlayerAdded()
    {

    }

    private IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }
}
