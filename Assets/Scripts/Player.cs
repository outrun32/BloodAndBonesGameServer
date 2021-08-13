using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public CharacterController characterController;

    [SerializeField]private float moveSpeed = 5f;
    public float gravity = Physics.gravity.y;
    [SerializeField]private float _jumpHeight;

    private Vector3 groundDirection;

    private bool _isJumping;

    private Vector3 playerVelocity;
    bool _isGrounded;

    private float[] _inputs;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        _inputs = new float[2];
    }

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
    }


    public void FixedUpdate()
    {
        Vector2 floatInput = new Vector2(_inputs[0], _inputs[1]);
        Move(floatInput);
        Jump();
        _isGrounded = characterController.isGrounded;

        //Отправка данных
        SendPlayerData();
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 direction = ((transform.forward * _inputDirection.y + transform.right * _inputDirection.x) * moveSpeed) * Time.fixedDeltaTime;
        if (_isGrounded)
        {
            groundDirection = direction;
        }
        characterController.Move(groundDirection);
    }

    private void SendPlayerData()
    {
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void Jump()
    {
        playerVelocity.y += gravity;
        if (_isGrounded)
        {
            playerVelocity.y = 0f;
        }
        Debug.Log($"Controller: {characterController.isGrounded}");
        Debug.Log($"is Jumping: {_isJumping}");
        if (_isGrounded && _isJumping)
        {
            playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * gravity);
        }
        characterController.Move(playerVelocity);
    }

    public void SetInput(float[] inputs, Quaternion rotation, bool isJumping)
    {
        _inputs = inputs;
        transform.rotation = rotation;
        _isJumping = isJumping;
    }
}