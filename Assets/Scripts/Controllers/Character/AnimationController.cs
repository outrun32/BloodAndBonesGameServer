using Delegates;
using UnityEngine;

namespace Controllers.Character
{
    public class AnimationController
    {
        public ReturnVoid StopAttackEvent;
        private bool _isAttacking = false;
        private Animator _animator;

        public AnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void Update(InputModel inputModel)
        {
            _animator.SetFloat("SpeedX", inputModel.JoystickAxis.x);
            _animator.SetFloat("SpeedY", inputModel.JoystickAxis.y);
            _animator.SetFloat("Speed", inputModel.JoystickAxis.magnitude);
            _animator.SetBool("Jump", inputModel.IsJumping);
            _animator.SetBool("Attack", inputModel.IsAttacking);
            _animator.SetBool("SuperAttack", inputModel.IsSuperAtacking);
            _animator.SetBool("Block", inputModel.IsBlocking);
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
            return new AnimationModel(_animator.GetFloat("Speed"),
                _animator.GetFloat("SpeedX"),
                _animator.GetFloat("SpeedY"),
                _animator.GetInteger("AttackInd"),
                _animator.GetInteger("HitInd"),
                _animator.GetBool("Attack"),
                _animator.GetBool("SuperAttack"),
                _animator.GetBool("Block"),
                _animator.GetBool("BlockImpact"),
                _animator.GetBool("Death"));
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
