using UnityEngine;

public class SetIntExitSTB : StateMachineBehaviour
{
    public string Name;
    public int Value;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(Name, Value);
    }
}
