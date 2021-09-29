using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CheckShieldontroller : MonoBehaviour
{
    public OnTriggeredEnterController Shield;

    public string Tag;
    // Start is called before the first frame update
    void Start()
    {
        Shield.OnTriggerEnterEvent += OnTriggeredEnter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggeredEnter(string name, Collider collider)
    {
        if (collider.CompareTag(Tag))
        {
            Debug.Log("Shiled");
        }
    }
}
