using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _preloadTime = 30;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitStartTime());
        }

        // Update is called once per frame
        void Update()
        {
            if (_preloadTime - Time.time > 0)
            {
                ServerSend.SetCountTimer(Mathf.CeilToInt(_preloadTime - Time.time));
            }
        }

        IEnumerator WaitStartTime()
        {
            yield return new WaitForSeconds(_preloadTime);
            ServerSend.StartSession();
            foreach (KeyValuePair<int,Client> client in Server.clients)
            {
                if (client.Value.player) client.Value.player.StartSession();
            }
        }
    }
}
