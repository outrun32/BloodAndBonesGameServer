using UnityEngine;

namespace Controllers
{
    public class ManaController : MonoBehaviour
    {
        private float _mana;
        private float _maxMana;

        public float Mana
        {
            get => _mana;
            private set
            {
                if (value > _maxMana) _mana = _maxMana;
                else if (value < 0) _mana = 0;
                else _mana = value;
            }
        }

        public void Initialize(float startMana, float maxMana)
        {
            _mana = startMana;
            _maxMana = maxMana;
        }
        public bool CheckMana(float value)
        {
            return Mana >= value;
        }
        public void SubMana(float value)
        {
            Mana -= value;
        }
        public void AddMana(float value)
        {
            Mana += value;
        }
        public void SetMana(float value)
        {
            Mana = value;
        }
    }
}
