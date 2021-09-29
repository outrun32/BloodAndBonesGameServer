using UnityEngine;

namespace Controllers
{
    public class PlayerKnight : MeleePlayerController
    {
        private PlayerKnightAnimationController _playerKnightAnimationController;           
        public override void Start()
        {
            _movementController = new MovementByAnimationController(transform);
            _playerKnightAnimationController = new PlayerKnightAnimationController(transform);
            _animationController = _playerKnightAnimationController;
            _animationController.Init(_animator, AnimatorSettings);
            base.Start();
        }

        public override void Attack()
        {
            //_movementController.SetCanMove(false);
            base.Attack();
            _attackController.Attack(10);
        }

        public override void Damage()
        {
            base.Damage();
            _animationController.SendMassage(AnimationMessages.Damage, 1);
        }
    }
}
