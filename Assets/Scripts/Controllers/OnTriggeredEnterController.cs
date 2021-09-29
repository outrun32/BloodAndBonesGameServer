using System;
using System.Collections;
using System.Collections.Generic;
using Delegates;
using UnityEngine;

public class OnTriggeredEnterController : MonoBehaviour
{
    public string Name;
    public ReturnNameAndCollider OnTriggerEnterEvent;
        
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"ON TRIGGERED ENTER УУУ СУКА: {other.name}");
        OnTriggerEnterEvent?.Invoke(Name, other);
    }
}
