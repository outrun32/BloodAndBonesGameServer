using System.Collections;
using Controllers;
using Controllers.Character;
using Delegates;
using Interfaces.Attack;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event ReturnVoid DeathEvent;
    public event ReturnPlayer DeathPlayerEvent;
    private int _id;
    private string _username;
    private bool _isBlocking;
    private bool _isDeath;
    [Header("Controllers")]
    protected MovementController _movementController;
    private AnimationController _animationController;
    protected IAttack _attackController;
    
    [SerializeField] private DamageController _damageController;
    [SerializeField] private ManaController _manaController;
    
    [Header("Movement")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity = Physics.gravity.y;
    [SerializeField] private float _jumpHeight;
    [Header("Health")] 
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _startHealth;
    [Header("Mana")] 
    [SerializeField] private float _maxMana;
    [SerializeField] private float _startMana;
    [Header("Animation")] 
    [SerializeField] private Animator _animator;

    public int ID => _id;
    public string Username => _username;
    public float MAXHealth => _maxHealth;
    public float StartHealth => _startHealth;
    public float MAXMana => _maxMana;
    public float StartMana => _startMana;
    public float Health => _damageController.Health;
    public float Mana => _manaController.Mana;

    public bool IsBlocking
    {
        get => _isBlocking;
        set
        {
            _damageController.SetDamageState(value);
            _isBlocking = value;
        }
    }
    public void Initialize(int id, string username)
    {
        _id = id;
        _username = username;
        _movementController = new MovementController(_characterController, _moveSpeed, _jumpHeight, this.transform);
        _damageController.Initialize(_startHealth, _maxHealth);
        _damageController.Damage += Damage;
        _manaController.Initialize(_startMana, _maxMana);
        _animationController = new AnimationController(_animator);
        _animationController.StopAttackEvent += EndAttack;
    }

    public virtual void Start()
    {
        _movementController.Start();
        
        _attackController.NStart();
        _movementController.SetCanMove(true);
        OnEnableN();
    }

    public void FixedUpdate()
    {
        _movementController.FixedUpdate();
        //Отправка данных
        SendPlayerData();
    }
    
    private void SendPlayerData()
    {
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
        ServerSend.PlayerAnimation(this);
        ServerSend.PlayerInfo(this);
    }
    public void SetInput(InputModel inputModel)
    {
        _movementController.SetInput(inputModel);
        _animationController.Update(inputModel);
        if (inputModel.IsAttacking)
        {
            _movementController.SetCanMove(false);
            Debug.Log(GetAnimationModel().AttackInd);
            if (GetAnimationModel().AttackInd == 4) _movementController.MoveUntil();
            _attackController.SetAttack(GetAnimationModel().AttackInd);
            _attackController.Attack(10);
        }
        IsBlocking = inputModel.IsBlocking;
    }

    public AnimationModel GetAnimationModel()
    {
        return _animationController.GetAnimationModel();
    }

    public void EndAttack()
    {
        _movementController.SetCanMove(true);
        _attackController.EndAttack();
    }

    public void Damage()
    {
        _animationController.Damage(2);
        _isDeath = _damageController.CheckDeath();
        if (_isDeath)
        {
            _animationController.Death();
            StartCoroutine(WaitDeath());
            ServerSend.PlayerDeath(this);
        }
    }
    public void OnEnableN()
    {
        DeathEvent += OnDisableN;
    }

    public void OnDisableN()
    {
        _damageController.Damage -= Damage;
        _animationController.StopAttackEvent -= EndAttack;
        DeathEvent -= OnDisableN;

    }
    public IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(3);
        DeathEvent?.Invoke();
        DeathPlayerEvent?.Invoke(this);
    }
}