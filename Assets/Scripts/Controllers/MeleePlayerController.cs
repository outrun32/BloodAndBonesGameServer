using System.Runtime.InteropServices;
using Controllers.Character.Attack;
using UnityEngine;

namespace Controllers
{
    public class MeleePlayerController : Player
    {
        [Header("Melee Attack Settings")]
        [SerializeField] private MeleeAttackController _meleeAttackController;
        [SerializeField] private float _distanceTeleport = 1;

        public override void Start()
        {
            _attackController = _meleeAttackController;
            base.Start();
        }
        public void SetPosition()
        {
            _movementController.StopMoveUntil();
        }
    }
}
