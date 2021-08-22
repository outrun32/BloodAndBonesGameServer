using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
    [SerializeField]private int _port = 26950;

    public Player playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {

        PlayFabMultiplayerAgentAPI.Start();
        StartCoroutine(ReadyForPlayers());



        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Server.Start(10, _port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void Respawn(Player player)
    {
        Debug.Log("RESPAWN");
        player.DeathPlayerEvent -= Respawn;
        Server.clients[player.ID].Respawn(player.Username);
        Destroy(player.gameObject);
        
    }

    private IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    public Player InstantiatePlayer()
    {
        Player instantiate = Instantiate(playerPrefab, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),
            Quaternion.identity);
        instantiate.DeathPlayerEvent += Respawn;
        return instantiate;
    }
}
