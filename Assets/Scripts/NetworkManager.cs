using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;
public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
    [SerializeField] private SpawnController _spawnController;
    [SerializeField]private int _port = 26950;

    public int Port { get => _port;}
    //public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
#if  UNITY_EDITOR
        Server.Start(10, _port);
#endif
    }

    private void FixedUpdate()
    {
        //Debug.Log(Server.clients.Count); 
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return _spawnController.InstantiatePlayer();
    }
}
