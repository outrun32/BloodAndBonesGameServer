using System;
using Interfaces.Attack;
using UnityEngine;

namespace Controllers.Character.Attack
{
    public class MeleeAttackController : MonoBehaviour, IAttack
    {
        [SerializeField] private SwordController _swordController;
        private float _damage = 0;
        private bool _isAttack = false;
        public void NStart()
        {
            _swordController.ReturnObjectEvent += SetDamage;
        }

        public void Attack(float damage)
        {
            _damage = damage;
            _isAttack = true;
        }

        public void EndAttack()
        {
            _isAttack = false;
        }
        private void SetDamage(GameObject gameObject)
        {
            if (_isAttack) gameObject.SendMessage("AddDamage", _damage);
        }
        private void OnDisable()
        {
            _swordController.ReturnObjectEvent -= SetDamage;
        }
    }
}
