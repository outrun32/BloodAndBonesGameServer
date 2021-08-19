using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntEnterSMB : StateMachineBehaviour
{
    public string Name;
    public int Value;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(Name, Value);
    }
}
