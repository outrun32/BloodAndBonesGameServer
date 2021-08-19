using System;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Controllers.Character.Attack
{
    public delegate void ReturnObject(GameObject gameObject);
    public class SwordController : MonoBehaviour
    {
        public event ReturnObject ReturnObjectEvent;
        public string[] tags;
        public Collider Collider;
        private void OnTriggerEnter(Collider other)
        {
            //Debug.LogWarning(other.name);
            foreach (string tag in tags)
            { 
                if (other.gameObject.CompareTag(tag))
                {
                    //Debug.LogWarning("ATTACK PLAYER");
                    ReturnObjectEvent?.Invoke(other.gameObject);
                }
            }
        }

         public void StartTracking()
         {
             Collider.enabled = true;
         }

        public void StopTracking()
        {
            Collider.enabled = false;

        }
    }
}
