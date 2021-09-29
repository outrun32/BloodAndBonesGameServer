using Controllers;
using UnityEngine;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public Player playerPrefab;

    [SerializeField] private int _portTCP = 26950;
    [SerializeField] private int _portUDP = 26950;

    public bool startServerInNManager = false;

    public int PortTCP { get => _portTCP;}
    public int PortUDP { get => _portUDP; }

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
            Server.Start(10, _portTCP, _portUDP);
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
