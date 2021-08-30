using Controllers;
using UnityEngine;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    [SerializeField] private int _port = 26950;

    public bool startServerInNManager = false;

    public int Port { get => _port;}
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        if (startServerInNManager)
            Server.Start(10, _port);
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    public Player InstantiatePlayer()
    {
        return new Player(); //_spawnController.InstantiatePlayer();
    }
}
