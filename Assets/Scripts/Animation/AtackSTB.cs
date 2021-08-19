using UnityEngine;

public class AtackSTB : StateMachineBehaviour
{
    public int MaxIndAtack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetFloat("Speed") <= 0.6f) animator.SetInteger("AttackInd", Random.Range(0,MaxIndAtack));
        else animator.SetInteger("AttackInd", 4);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
        animator.SetBool("EndAttack", true);
    }
}
