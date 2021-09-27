using System;
using System.Runtime.InteropServices;
using Controllers.Character;
using Controllers.Character.Attack;
using UnityEngine;

namespace Controllers
{
    public class MeleePlayerController : Player
    {
        [Header("Melee Attack Settings")]
        [SerializeField] private MeleeAttackController _meleeAttackController;

        public override void Start()
        {
            _attackController = _meleeAttackController;
            base.Start();
        }
        public void OnDisable()
        {
            OnDisableN();
        }
    }
}
