using UnityEngine;

namespace Controllers
{
    public class SpawnController : MonoBehaviour
    {
        public Player playerPrefab;
        public void Respawn(Player player)
        {
            Debug.Log("RESPAWN");
            Server.clients[player.ID].Respawn();
            Destroy(player.gameObject);
        
        }
        public Player InstantiatePlayer(Transform spawnTransform)
        {
            Player instantiate = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            return instantiate;
        }
    }
}
