using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Interfaces.Attack
{
    public interface IAttack
    {
        public void NStart();
        public void Attack(float damage);
        public void EndAttack();
    }
}
