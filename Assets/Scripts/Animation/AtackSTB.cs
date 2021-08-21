using UnityEngine;

public class AtackSTB : StateMachineBehaviour
{
    public int MaxIndAtack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StartAttack", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
        animator.SetBool("EndAttack", true);
        animator.SetBool("StartAttack", false);
    }
}
