using UnityEngine;

namespace Controllers.Character
{
    public class MovementController
    {
        private CharacterController _characterController;

        private float _moveSpeed;
        private float _gravity = Physics.gravity.y;
        private float _jumpHeight;

        private Transform _transform;
        private Vector3 _groundDirection;

        private bool _isJumping;

        private Vector3 _playerVelocity;
        bool _isGrounded;

        private Vector2 _inputs;

        public MovementController(CharacterController characterController, float moveSpeed, float jumpHeight, Transform transform)
        {
            this._characterController = characterController;
            this._moveSpeed = moveSpeed;
            this._jumpHeight = jumpHeight;
            this._transform = transform;
        }

        public void Start()
        {
            _gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        }
        public void FixedUpdate()
        {
            Vector2 floatInput = _inputs;
            Move(floatInput);
            Jump();
            _isGrounded = _characterController.isGrounded;
        }
        private void Move(Vector2 _inputDirection)
        {
            Vector3 direction = ((_transform.forward * _inputDirection.y + _transform.right * _inputDirection.x) * _moveSpeed) * Time.fixedDeltaTime;
            if (_isGrounded)
            {
                _groundDirection = direction;
            }
            _characterController.Move(_groundDirection);
        }
        public void Jump()
        {
            _playerVelocity.y += _gravity;
            if (_isGrounded)
            {
                _playerVelocity.y = 0f;
            }
            if (_isGrounded && _isJumping)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            }
            _characterController.Move(_playerVelocity);
        }
        public void SetInput(InputModel inputModel)
        {
            _inputs = inputModel.JoystickAxis;
            _transform.rotation = inputModel.Rotation;
            _isJumping = inputModel.IsJumping;
        }
    }
    
}

