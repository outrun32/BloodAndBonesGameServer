using UnityEngine;

namespace Controllers
{
    public class HealthController : MonoBehaviour
    {
        private float _maxHealth;
        private float _health;

        public float MaxHealth => _maxHealth;

        public float Health
        {
            get
            {
                return _health;
            }
            private set
            {
                if (value > MaxHealth) _health = _maxHealth;
                else if (value < 0) _health = 0;
                else _health = value;
            }
        }

        public void Initialize(float startHealth, float maxHealth)
        {
            SetHealth(startHealth);
            SetMaxHealth(maxHealth);
        }
        protected void SetMaxHealth(float value)
        {
            _maxHealth = value;
        }
        public void SetHealth(float value)
        {
            Health = value;
        }

        public void AddHealth(float value)
        {
            Health += value;
        }

        public void SubHealth(float value)
        {
            Health -= value;
        }
    }
}
