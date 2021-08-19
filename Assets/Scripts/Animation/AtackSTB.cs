using UnityEngine;

public class AtackSTB : StateMachineBehaviour
{
    public int MaxIndAtack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AttackInd", animator.GetFloat("SpeedY") <= 0.6f ? Random.Range(0, MaxIndAtack) : 4);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
        animator.SetBool("EndAttack", true);
    }
}
