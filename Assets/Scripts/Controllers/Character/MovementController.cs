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
        private bool _canMove;
        private bool _moveUntil;

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
            if (_canMove)
            {
                Move(floatInput);
                Jump();
            }

            if (_moveUntil)
            {
                Debug.Log("_moveUntil");
                Move(Vector2.up);
                Jump();
            }
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

        public void SetCanMove(bool value)
        {
            _canMove = value;
        }

        public void MoveUntil()
        {
            _moveUntil = true;
        }

        public void StopMoveUntil()
        {
            _moveUntil = false;
        }
    }
    
}

