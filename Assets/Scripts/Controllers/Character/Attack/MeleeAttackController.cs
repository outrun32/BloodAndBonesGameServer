using System;
using System.Collections.Generic;
using Interfaces.Attack;
using UnityEngine;

namespace Controllers.Character.Attack
{
    public class MeleeAttackController : MonoBehaviour, IAttack
    {
        [SerializeField] private List<SwordController> _swordControllers;
        
        private List<GameObject> _objects = new List<GameObject>();
        private float _damage = 0;
        private bool _isAttack = false;
        public void NStart()
        {
            foreach (SwordController swordController in _swordControllers)
            {
                swordController.ReturnObjectEvent += SetDamage; 
                swordController.StopTracking();
            }
        }

        public void SetAttack(int index)
        {
            for (int i = 0; i < _swordControllers.Count; i++)
            {
                SwordController swordController = _swordControllers[i];
                if(i == index) swordController.StartTracking();
            }
        }
        public void Attack(float damage)
        {
            _damage = damage;
            _isAttack = true;
        }

        public void EndAttack()
        {
            _objects.Clear();
            _isAttack = false;
            for (int i = 0; i < _swordControllers.Count; i++)
            {
                _swordControllers[i].StopTracking();
            }
        }
        private void SetDamage(GameObject target)
        {
            //Debug.LogWarning("SetDamage");
            if (_isAttack && target != this.gameObject)
            {
                //Debug.LogWarning("Check");
                if (!_objects.Contains(target))
                {
                    //Debug.LogWarning("Damage PLAYER");
                    _objects.Add(target);
                    target.SendMessage("AddDamage", _damage);
                }
            }
        }
        private void OnDisable()
        {
            foreach (SwordController swordController in _swordControllers)
            {
                swordController.ReturnObjectEvent -= SetDamage;
            }
        }

      
        
    }
}
