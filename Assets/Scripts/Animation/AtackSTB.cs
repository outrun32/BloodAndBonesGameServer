using UnityEngine;

public class AtackSTB : StateMachineBehaviour
{
    public int MaxIndAtack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AttackInd", Random.Range(0,MaxIndAtack));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
