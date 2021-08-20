using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(10, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),
            Quaternion.identity);
    }
}
