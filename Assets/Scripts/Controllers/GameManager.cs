using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SpawnController _spawnController;
        [SerializeField] private float _preloadTime = 30;
        [SerializeField] private Dictionary<string, Character.Character> _redTeam;
        [SerializeField] private Dictionary<string, Character.Character> _blueTeam;
        [SerializeField] private Dictionary<Character.Character, Transform> _spawnTransforms;
        [SerializeField] private List<Transform> _spawnsRed;
        [SerializeField] private List<Transform> _spawnsBLue;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitStartTime());
            _spawnTransforms = new Dictionary<Character.Character, Transform>();
            _redTeam = new Dictionary<string, Character.Character>();
            _blueTeam = new Dictionary<string, Character.Character>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_preloadTime - Time.time > 0)
            {
                ServerSend.SetCountTimer(Mathf.CeilToInt(_preloadTime - Time.time));
            }
        }

        private void Death(Character.Character characterStriker, Character.Character characterDeath)
        {
            Respawn(characterDeath);
            Debug.Log($"User  {characterStriker.Username} kill {characterDeath.Username}");
        }
        private void Respawn(Character.Character client)
        {
            
        }
        private void SpawnPlayers()
        {
            bool isRed = false;
            int indexSpawn = -1;
            Player player;
            List<Client> clients = Server.clients.Select(c => c.Value).Where(c => c.IsAutorized).ToList();
            foreach (Client client in clients)
            {
                isRed = !isRed;
                if (isRed)
                {
                    indexSpawn++;
                    player = _spawnController.InstantiatePlayer(_spawnsRed[indexSpawn]);
                    _spawnTransforms.Add(player,_spawnsRed[indexSpawn]);
                    _redTeam.Add(client.Username, player);
                }
                else
                {
                    player = _spawnController.InstantiatePlayer(_spawnsBLue[indexSpawn]); 
                    _spawnTransforms.Add(player,_spawnsBLue[indexSpawn]);
                    _blueTeam.Add(client.Username, player);
                }

                player.DeathCharacter += Death;
                client.SendIntoGame(player);
            }
        }
        IEnumerator WaitStartTime()
        {
            yield return new WaitForSeconds(_preloadTime);
            SpawnPlayers();
            ServerSend.StartSession();
            foreach (KeyValuePair<int,Client> client in Server.clients)
            {
                if (client.Value.Player) client.Value.Player.StartSession();
            }
        }
    }
}
