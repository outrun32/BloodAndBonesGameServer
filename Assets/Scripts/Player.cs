using Controllers;
using Controllers.Character;
using Controllers.Character.Attack;
using Interfaces.Attack;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _id;
    private string _username;
    public int ID => _id;
    public string Username => _username;
    [Header("Controllers")]
    private MovementController _movementController;
    private AnimationController _animationController;
    
    [SerializeField] private HealthController _healthController;
    [SerializeField] private ManaController _manaController;
    protected IAttack _attackController;
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
    public void Initialize(int id, string username)
    {
        _id = id;
        _username = username;
        _movementController = new MovementController(_characterController, _moveSpeed, _jumpHeight, this.transform);
        _healthController.Initialize(_startHealth, _maxHealth);
        _manaController.Initialize(_startMana, _maxMana);
        _animationController = new AnimationController(_animator);
    }

    public virtual void Start()
    {
        _movementController.Start();
        _attackController.NStart();
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
    }
    public void SetInput(InputModel inputModel)
    {
        _movementController.SetInput(inputModel);
        _animationController.Update(inputModel);
    }

    public AnimationModel GetAnimationModel()
    {
        return _animationController.GetAnimationModel();
    }
}