using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public DamageController damageController;

    [SerializeField] public float Health;
    // Start is called before the first frame update
    void Start()
    {
        //damageController.Initialize(100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        //Health = damageController.Health;
    }
}
