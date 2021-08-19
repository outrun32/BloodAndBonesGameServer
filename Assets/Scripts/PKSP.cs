using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class PKSP : MonoBehaviour
{
    public MeleePlayerController MeleePlayerController;
    public void SetPosition()
    {
        MeleePlayerController.SetPosition();
    }
}
