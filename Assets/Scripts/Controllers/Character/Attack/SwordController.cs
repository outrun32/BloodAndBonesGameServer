using System;
using UnityEngine;

namespace Controllers.Character.Attack
{
    public delegate void ReturnObject(GameObject gameObject);
    public class SwordController : MonoBehaviour
    {
        public event ReturnObject ReturnObjectEvent;
        public string[] tags;
        private void OnTriggerExit(Collider other)
        {
            foreach (string tag in tags)
            { 
                if (other.gameObject.CompareTag(tag))
                {
                    ReturnObjectEvent?.Invoke(other.gameObject);
                }
            }
        }
    }
}
