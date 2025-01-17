using System;
using Delegates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Character
{   
    public class AnimationController: IAnimationContoller
    {
        private int _maxIndAttack = 0;
       
        private bool _isAttacking = false;
        private Animator _animator;

        public ReturnVoid StopAttackEvent;
        public AnimationController(int maxIndAttack)
        {
            _maxIndAttack = maxIndAttack;
        }

        public void Init(Animator animator, AnimatorSettings animatorSettingsS)
        {
            _animator = animator;
        }

        public void Update(InputModel inputModel)
        {
            _animator.SetFloat("SpeedX", inputModel.JoystickAxis.x);
            _animator.SetFloat("SpeedY", inputModel.JoystickAxis.y);
            _animator.SetFloat("Speed", inputModel.JoystickAxis.magnitude);
            _animator.SetBool("Jump", inputModel.IsJumping);
            if (!_animator.GetBool("StartAttack"))
            {
                if (inputModel.IsAttacking)
                {
                    _animator.SetInteger("AttackInd",
                        _animator.GetFloat("SpeedY") <= 0.6f ? Random.Range(0, _maxIndAttack) : 4);
                    _animator.SetBool("Attack", inputModel.IsAttacking);
                }
                
            }
            else _animator.SetBool("Attack", inputModel.IsAttacking);
            _animator.SetBool("SuperAttack", inputModel.IsSuperAtacking);
            _animator.SetBool("Block", inputModel.IsBlocking);
        }

        public float GetSpeedValue(Vector2 axis)
        {
            return Mathf.Sqrt(axis.x*axis.x + axis.y * axis.y);
        }
        public AnimationModel GetAnimationModel()
        {
            if (_isAttacking)
            {
                if (_animator.GetBool("EndAttack"))
                {
                    _animator.SetBool("EndAttack", false);
                    StopAttackEvent?.Invoke();
                }

                
            }
            else _isAttacking = _animator.GetBool("Attack");

            return new AnimationModel();
            /*return new AnimationModel(_animator.GetFloat("Speed"),
                _animator.GetFloat("SpeedX"),
                _animator.GetFloat("SpeedY"),
                _animator.GetInteger("AttackInd"),
                _animator.GetInteger("HitInd"),
                _animator.GetBool("Attack"),
                _animator.GetBool("SuperAttack"),
                _animator.GetBool("Block"),
                _animator.GetBool("BlockImpact"),
                _animator.GetBool("Death"));*/
        }

        public void SendMassage<T>(AnimationMessages message, T value)
        {
            switch (message)
            {
                case AnimationMessages.Damage:
                    Damage(Convert.ToInt32(value));
                    break;
                case AnimationMessages.Death:
                    Death();
                    break;
            }
        }
        public void Damage(int value)
        {
            _animator.SetInteger("HitInd", value);
        }
        public void Death()
        {
            _animator.SetBool("Death", true);
        }
    }
}
