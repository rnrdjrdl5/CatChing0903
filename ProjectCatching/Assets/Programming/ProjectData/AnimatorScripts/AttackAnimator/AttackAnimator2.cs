using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimator2 : StateMachineBehaviour
{
    private bool beginExit = false;
    private bool waitingToBegin = false;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (animator.IsInTransition(layerIndex))
            waitingToBegin = true;

        else
            waitingToBegin = false;

        beginExit = false;


        animator.SetBool("isAttack", false);

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (!beginExit)
        {

            if (animator.IsInTransition(layerIndex))
            {

                if (!waitingToBegin)
                {

                    beginExit = true;
                }
            }

            else if (waitingToBegin)
            {

                waitingToBegin = false;
            }
        }
    }

}

// beginExit : Start 이후에만 Update문이 작동할 수 있도록 함
// waitingToBegin : 전이를 완전히 사용한 이후에만 Update문을 사용할 수 있도록 함.
