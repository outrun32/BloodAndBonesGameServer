using UnityEngine;

namespace Controllers
{
    public class SpawnController : MonoBehaviour
    {
        public Player playerPrefab;
        public void Respawn(Character.Character player)
        {
            Debug.Log("RESPAWN");
            Destroy(player.gameObject);
        
        }
        public Player InstantiatePlayer(Transform spawnTransform)
        {
            Player instantiate = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            return instantiate;
        }
    }
}
