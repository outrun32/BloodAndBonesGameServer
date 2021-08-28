using UnityEngine;

namespace Controllers
{
    public class SpawnController : MonoBehaviour
    {
        public Player playerPrefab;
        public void Respawn(Player player)
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
}
