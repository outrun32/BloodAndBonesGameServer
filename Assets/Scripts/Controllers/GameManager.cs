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
        [Header("Controllers")]
        
        [SerializeField] private SpawnController _spawnController;

        [Header("Time Settings")] 
        [SerializeField] private float _preloadTime = 30;
        [SerializeField] private bool _isStartTime = true;
        [SerializeField] private float _endTime = 120;
        [SerializeField] private bool _isEndTime = true;
        private float _startPreloadTime = 0;
        private float _startTime = 0;
        private bool _isStartedTimer = false;
        private bool _isStartedSession = false;
        [Header("Players Settings")]
        [SerializeField] private List<Transform> _spawnsRed;
        [SerializeField] private List<Transform> _spawnsBLue;
        private Dictionary<string,(PlayerDataModel, Character.Character)> _redTeam;
        private Dictionary<string,(PlayerDataModel, Character.Character)> _blueTeam;
        private Dictionary<string,bool> _teams = new Dictionary<string, bool>();
        private Dictionary<Character.Character, Transform> _spawnTransforms;
        
        // Start is called before the first frame update
        void Start()
        {
            Server.OnClientAdded += ClientAdded;
            Server.OnClientRemoved += ClientRemove;
            _spawnTransforms = new Dictionary<Character.Character, Transform>();
            _redTeam = new Dictionary<string,(PlayerDataModel, Character.Character)>();
            _blueTeam = new Dictionary<string,(PlayerDataModel, Character.Character)>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - _startPreloadTime < _preloadTime && _isStartedTimer)
            {
                ServerSend.SetCountTimer(0, Mathf.CeilToInt(_preloadTime - Time.time + _startPreloadTime));
            }
            if (Time.time - _startTime < _endTime && _isStartedSession)
            {
                ServerSend.SetCountTimer(1, Mathf.CeilToInt(_endTime - Time.time + _startTime));
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
            _spawnController.DestroyCharacter(character);
            ServerSend.PlayerTeam(player.ID, _teams[player.Username]);
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
                if (isRed) indexSpawn++;
                SpawnPlayer(isRed, indexSpawn, client);
            }
        }

        private void SpawnPlayer(bool isRed, int indexSpawn, Client client)
        {
            Player player;
            if (isRed)
            {
                //indexSpawn++;
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
            if (_clients.Count == 0)
            {
                if (_isStartTime && !_isStartedTimer) StartCoroutine(WaitStartTime());
                _isStartedTimer = true;
            }
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
            _startPreloadTime = Time.time;
            yield return new WaitForSeconds(_preloadTime);
            _startTime = Time.time;
            _isStartedSession = true;
            if (_isEndTime) StartCoroutine(WaitEndTime());
            SpawnPlayers();
            ServerSend.StartSession();
            foreach (Client client in _clients)
            {
                if (client.Player) client.Player.StartSession();
            }
        }
        IEnumerator WaitEndTime()
        {
            yield return new WaitForSeconds(_endTime);
            foreach (Client client in _clients)
            {
                if (client.Player) client.Player.EndSession();
            }
            ServerSend.EndSession(new EndSessionModel(_redTeam, _blueTeam));

        }
    }
}
