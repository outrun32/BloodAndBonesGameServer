using UnityEngine;

namespace Models
{
    public readonly struct PlayerSpawnModel
    {
        public readonly int ID;
        public readonly string Username;
        public readonly Transform Transform;
        public readonly float MaxHealth;
        public readonly float MaxMana;
        public readonly float StartMana;
        public readonly float StartHealth;

        public PlayerSpawnModel(int id, string username, Transform transform, float maxHealth, float maxMana, float startMana, float startHealth)
        {
            ID = id;
            Username = username;
            Transform = transform;
            MaxHealth = maxHealth;
            MaxMana = maxMana;
            StartMana = startMana;
            StartHealth = startHealth;
        }
    }
}
