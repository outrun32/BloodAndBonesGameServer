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
            Debug.LogWarning(other.tag);
            foreach (string tag in tags)
            { 
                if (other.gameObject.CompareTag(tag))
                {
                    Debug.LogWarning("Attack Player");
                    ReturnObjectEvent?.Invoke(other.gameObject);
                }
            }
        }
    }
}
