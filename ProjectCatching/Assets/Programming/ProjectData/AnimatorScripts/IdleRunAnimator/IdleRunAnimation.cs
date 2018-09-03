using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRunAnimation : StateMachineBehaviour {

    PlayerState TS;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        TS = animator.gameObject.GetComponent<PlayerState>();

        if (animator.GetFloat("DirectionX") == 0 && animator.GetFloat("DirectionY") == 0)
        {
            TS.SetPlayerCondition(PlayerState.ConditionEnum.IDLE);
        }
        else
        {
            TS.SetPlayerCondition(PlayerState.ConditionEnum.RUN);
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        

        /*if (TS.GetPlayerCondition() == PlayerState.ConditionEnum.IDLE ||
            TS.GetPlayerCondition() == PlayerState.ConditionEnum.RUN)
        {*/
        if (!animator.IsInTransition(layerIndex))
        {
            TS = animator.gameObject.GetComponent<PlayerState>();

            if (animator.GetFloat("DirectionX") == 0 && animator.GetFloat("DirectionY") == 0)
            {
                TS.SetPlayerCondition(PlayerState.ConditionEnum.IDLE);
            }
            else
            {
                TS.SetPlayerCondition(PlayerState.ConditionEnum.RUN);
            }
        }
       // }

    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
