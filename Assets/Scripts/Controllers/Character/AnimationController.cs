using UnityEngine;

namespace Controllers.Character
{
    public delegate void ReturnBool(bool value);
    public class AnimationController
    {
        public ReturnBool StopAttackEvent;
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
        }

        public AnimationModel GetAnimationModel()
        {
            if (_isAttacking)
            {
                if (_animator.GetBool("EndAttack"))
                {
                    _animator.SetBool("EndAttack", false);
                    StopAttackEvent?.Invoke(true);
                }

                
            }
            else _isAttacking = _animator.GetBool("Attack");
            return new AnimationModel(_animator.GetFloat("Speed"),
                _animator.GetFloat("SpeedX"),
                _animator.GetFloat("SpeedY"),
                _animator.GetInteger("AttackInd"),
                _animator.GetInteger("BlockInd"),
                _animator.GetBool("Attack"),
                _animator.GetBool("SuperAttack"));
        }

        public void Damage(int value)
        {
            _animator.SetInteger("BlockInd", value);
        }
    }
}
