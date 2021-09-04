using System.Collections;
using Controllers;
using Controllers.Character;
using Delegates;
using Interfaces.Attack;
using UnityEngine;

public class Player : Character
{
    private event ReturnVoid DeathEvent;
    public event ReturnPlayer DeathPlayerEvent;
    [Header("Player Settings")]
    [Header("Controllers")]
    protected MovementController _movementController;
    [Header("Movement")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] [Range(0,1f)]private float _moveAccelerate;
    [SerializeField] private float _gravity = Physics.gravity.y;
    [SerializeField] private float _jumpHeight;
    public override bool IsBlocking 
    {
        get => _isBlocking;
        set
        {
            if (value != _isBlocking)
            {
                _movementController.SetCanMove(!value);
            }
            _damageController.SetDamageState(value);
            _isBlocking = value;
        }
    }

    public void StartSession()
    {
        _isStarted = true;
    }
    public override void Initialize(int id, string username)
    {
        base.Initialize(id, username);
        Debug.Log(username);
        _movementController = new MovementController(_characterController, _moveSpeed, _jumpHeight, this.transform);
    }

    public virtual void Start()
    {   
        base.StartN();
        _movementController.Start();
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
        if (_isStarted)
        {
            _inputDirection = Vector2.Lerp(_inputDirection, inputModel.JoystickAxis, _moveAccelerate);
            inputModel.JoystickAxis = _inputDirection;
            _movementController.SetInput(inputModel);
            _animationController.Update(inputModel);
            if (inputModel.IsAttacking)
            {
                _movementController.SetCanMove(false);
                if (GetAnimationModel().AttackInd == 4)
                {
                    _movementController.MoveUntil();
                    StartCoroutine(_movementController.StopMoveUntilByTime(1));
                }
                Debug.Log(GetAnimationModel().AttackInd);
                _attackController.SetAttack(GetAnimationModel().AttackInd);
                _attackController.Attack(10);
            }

            IsBlocking = inputModel.IsBlocking;
        }
    }

    public AnimationModel GetAnimationModel()
    {
        return _animationController.GetAnimationModel();
    }

    private void EndAttack()
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
    public void Damage(float value, DamageType damageType)
    {
        Damage();
    }
    public void Damage(float value, DamageType damageType, Character character)
    {
        Damage();
    }
    public override void OnEnableN()
    {
        base.OnEnableN();
        _damageController.DamageCharacterEvent += Damage;
        _damageController.DamageEvent += Damage;
        _animationController.StopAttackEvent += EndAttack;
    }

    public override void OnDisableN()
    {
        base.OnDisableN();
        _damageController.DamageEvent -= Damage;
        _damageController.DamageCharacterEvent -= Damage;
        _animationController.StopAttackEvent -= EndAttack;

    }
    public IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(3);
        DeathEvent?.Invoke();
        DeathPlayerEvent?.Invoke(this);
    }
}