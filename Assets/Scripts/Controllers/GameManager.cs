using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        private List<Client> _clients = new List<Client>();
            
            
        [SerializeField] private SpawnController _spawnController;
        [SerializeField] private float _preloadTime = 30;
        [SerializeField] private Dictionary<string,(PlayerDataModel, Character.Character)> _redTeam;
        [SerializeField] private Dictionary<string,(PlayerDataModel, Character.Character)> _blueTeam;
        [SerializeField] private Dictionary<string,bool> _teams = new Dictionary<string, bool>();
        [SerializeField] private Dictionary<Character.Character, Transform> _spawnTransforms;
        [SerializeField] private List<Transform> _spawnsRed;
        [SerializeField] private List<Transform> _spawnsBLue;
        [SerializeField] private bool _isStartTime = true;
        // Start is called before the first frame update
        void Start()
        {
            Server.OnClientAdded += ClientAdded;
            Server.OnClientRemoved += ClientRemove;
            if (_isStartTime) StartCoroutine(WaitStartTime());
            _spawnTransforms = new Dictionary<Character.Character, Transform>();
            _redTeam = new Dictionary<string,(PlayerDataModel, Character.Character)>();
            _blueTeam = new Dictionary<string,(PlayerDataModel, Character.Character)>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_preloadTime - Time.time > 0)
            {
                ServerSend.SetCountTimer(Mathf.CeilToInt(_preloadTime - Time.time));
            }
        }

        (PlayerDataModel, Character.Character) GetPlayerData(string username)
        {
            return _teams[username] ? _redTeam[username] : _blueTeam[username];
        }
        private void Death(Character.Character characterStriker, Character.Character characterDeath)
        {
            GetPlayerData(characterStriker.Username).Item1.KillCount++;
            GetPlayerData(characterDeath.Username).Item1.DeathCount++;
            Debug.Log($"User  {characterStriker.Username} kill {characterDeath.Username}");
        }
        private void Respawn(Player character)  
        {
            character.DeathCharacter -= Death;
            character.DeathPlayerEvent -= Respawn;
            
            Player player = _spawnController.InstantiatePlayer(_spawnTransforms[character]);
            _spawnTransforms.Add(player, _spawnTransforms[character]);
            _spawnTransforms.Remove(character);
            Server.clients[character.ID].Respawn(player);
            _spawnController.Respawn(character);
            
            player.DeathCharacter += Death;
            player.DeathPlayerEvent += Respawn;
            player.StartSession();
        }

        private void SpawnPlayers()
        {
            bool isRed = false;
            int indexSpawn = -1;
            foreach (Client client in _clients)
            {
                isRed = !isRed;
                SpawnPlayer(isRed, indexSpawn, client);
            }
        }

        private void SpawnPlayer(bool isRed, int indexSpawn, Client client)
        {
            Player player;
            if (isRed)
            {
                indexSpawn++;
                player = _spawnController.InstantiatePlayer(_spawnsRed[indexSpawn]);
                _teams.Add(client.Username,true);
                _spawnTransforms.Add(player,_spawnsRed[indexSpawn]);
                _redTeam.Add(client.Username, (new PlayerDataModel(client.Username, client.id,0,0,0), player));
            }
            else
            {
                player = _spawnController.InstantiatePlayer(_spawnsBLue[indexSpawn]); 
                _teams.Add(client.Username,false);
                _spawnTransforms.Add(player,_spawnsBLue[indexSpawn]);
                _blueTeam.Add(client.Username,(new PlayerDataModel(client.Username, client.id,0,0,0), player));
            }

            player.DeathCharacter += Death;
            player.DeathPlayerEvent += Respawn;
            client.SendIntoGame(player);
        }
        public void ClientAdded(Client client)
        {
            _clients.Add(client);
            if (!_isStartTime)
            {
                SpawnPlayer(true, Random.Range(0, 4), client);
                client.Player.StartSession();
            }
        }

        public void ClientRemove(Client client)
        {
            _clients.Remove(client);
            if (_teams[client.Username]) _redTeam.Remove(client.Username);
            else _blueTeam.Remove(client.Username);
            _teams.Remove(client.Username);
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
