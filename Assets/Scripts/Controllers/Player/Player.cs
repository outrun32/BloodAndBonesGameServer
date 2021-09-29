using System.Collections;
using Controllers;
using Controllers.Character;
using Delegates;
using Interfaces.Attack;
using Models;
using UnityEngine;

public class Player : Character
{
    //private event ReturnVoid DeathEvent;
    public event ReturnPlayer DeathPlayerEvent;
    [Header("Player Settings")]
    [Header("Controllers")]
    protected IMovemennt _movementController;
    [Header("Movement")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] [Range(0,1f)]private float _moveAccelerate;
    [SerializeField] private float _gravity = Physics.gravity.y;
    [SerializeField] private float _jumpHeight;

    protected AnimationModel _animationModel;
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
    
    public override void Initialize(int id, string username)
    {
        base.Initialize(id, username);
        Debug.Log(username);
    }

    public virtual void Start()
    {
        StartN();
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
        _animationModel = GetAnimationModel();  
        ServerSend.SendPlayerData(new PlayerSendingDataModel(ID,transform,_animationModel,Health,Mana));
    }       
    public virtual void SetInput(InputModel inputModel)
    {
        if (!_isStarted)
        {
            inputModel = new InputModel(Vector2.zero, inputModel.CameraRotate, false, false, false, false, false, false, false);
        }
        _inputDirection = Vector2.Lerp(_inputDirection, inputModel.JoystickAxis, _moveAccelerate);
        inputModel.JoystickAxis = _inputDirection;
        _movementController.SetInput(inputModel);
        _animationController.Update(inputModel);
        if (inputModel.IsAttacking)
        {
            Attack();
        }

        IsBlocking = inputModel.IsBlocking;
    }

    public virtual void Attack()
    {
        _attackController.SetAttack(_animationModel.AttackInd);
        
    }

    private void EndAttack()
    {
        _movementController.SetCanMove(true);
        _attackController.EndAttack();
    }

    public virtual void Damage()
    {
        
        _isDeath = _healthController.CheckDeath();
        if (_isDeath)
        {
            _animationController.SendMassage(AnimationMessages.Death, 0);
            StartCoroutine(WaitDeath());
            ServerSend.PlayerDeath(ID);
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
        //_animationController.StopAttackEvent += EndAttack;
    }

    public override void OnDisableN()
    {
        base.OnDisableN();
        _damageController.DamageEvent -= Damage;
        _damageController.DamageCharacterEvent -= Damage;
        //_animationController.StopAttackEvent -= EndAttack;

    }
    public IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(3);
        //DeathEvent?.Invoke();
        DeathPlayerEvent?.Invoke(this);
    }
    
    public virtual void SendMessage(string message)
    {
        Debug.Log("Message");
        switch (message)
        {
            case "EndRotation":
                _animationController.SendMassage(AnimationMessages.EndRotation,"");
                break;
        }
    }public virtual void SendEvent(string message)
    {
        Debug.Log($"SendEvent: {message}");
        switch (message)
        {
            case "toStrike":
               //EndAttack();
                break;
            case "endAttack":
                EndAttack();
                break;
        }
    }
}