using System.Collections;
using Delegates;
using Interfaces.Attack;
using UnityEngine;

namespace Controllers.Character
{
    public class Character : MonoBehaviour
    {
        public event ReturnTwoCharacter DeathCharacter;
        protected int _id;
        protected string _username;
        protected bool _isBlocking;
        protected bool _isDeath;
        protected bool _isStarted;
        protected float _speed;
        protected Vector2 _inputDirection; 
        [Header("Character Settings")]
        [Header("Controllers")]
        protected AnimationController _animationController;
        protected IAttack _attackController;
        
        [SerializeField] protected DamageController _damageController;
        [SerializeField] protected ManaController _manaController;
        
        [Header("Health")] 
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _startHealth;
        [Header("Mana")] 
        [SerializeField] private float _maxMana;
        [SerializeField] private float _startMana;
        [Header("Animation")] 
        [SerializeField] private Animator _animator;
        [SerializeField] private int _maxIndAttack = 0;

        public int ID => _id;
        public string Username => _username;
        public float MAXHealth => _maxHealth;
        public float StartHealth => _startHealth;
        public float MAXMana => _maxMana;
        public float StartMana => _startMana;
        public float Health => _damageController.Health;
        public float Mana => _manaController.Mana;
        public virtual bool IsBlocking
        {
            get => _isBlocking;
            set
            {
                if (value != _isBlocking)
                {
                    
                }
                _damageController.SetDamageState(value);
                _isBlocking = value;
            }
        }

        public void StartSession()
        {
            _isStarted = true;
        }
        public virtual void Initialize(int id, string username)
        {
            _id = id;
            _username = username;
            _damageController.Initialize(_startHealth, _maxHealth);
            _manaController.Initialize(_startMana, _maxMana);
            _animationController = new AnimationController(_animator, _maxIndAttack);
        }

        public void GetDamage(float value, DamageType damageType, Character character)
        {
            if (Health <=0) DeathCharacter?.Invoke(character, this);
        }
        public void StartN()
        {
            _damageController.DamageCharacterEvent += GetDamage;
            _attackController.NStart();
            Debug.Log("NSTART Attack Controller");
        }
        public virtual void OnEnableN()
        {
           
        }

        public virtual void OnDisableN()
        {
            _damageController.DamageCharacterEvent -= GetDamage;
        }
    }
}
