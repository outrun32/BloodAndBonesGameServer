using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
<<<<<<< Updated upstream

    [SerializeField]private int _port = 26950;

    public bool startServerInNManager = false;

    public int Port { get => _port;}

    public Player playerPrefab;

=======
    [SerializeField] private int _portTCP = 26950;
    [SerializeField] private int _portUDP = 26950;

    public bool startServerInNManager = false;

    public int PortTCP { get => _portTCP;}
    public int PortUDP { get => _portUDP; }
>>>>>>> Stashed changes
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
        if (startServerInNManager)
            Server.Start(10, _portTCP, _portUDP);
    }

<<<<<<< Updated upstream
=======
    private void FixedUpdate()
    {
        //Debug.Log(Server.clients.Count); 
    }

>>>>>>> Stashed changes
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

    public Player InstantiatePlayer()
    {
        Player instantiate = Instantiate(playerPrefab, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),
            Quaternion.identity);
        instantiate.DeathPlayerEvent += Respawn;
        return instantiate;
    }
}
