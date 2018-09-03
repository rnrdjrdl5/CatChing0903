using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueAnimator : StateMachineBehaviour {

	  //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        animator.GetComponent<PlayerState>().SetPlayerCondition(PlayerState.ConditionEnum.RESCUE);
        animator.GetComponent<PlayerMove>().ResetMoveSpeed();


        RescuePlayer rescuePlayer = animator.gameObject.GetComponent<RescuePlayer>();

        rescuePlayer.CreateHelpEffect();

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        RescuePlayer rescuePlayer = animator.gameObject.GetComponent<RescuePlayer>();

        rescuePlayer.DeleteHelpEffect();


        // 정상적인 경로로 입력이 들어오면 해당되지않음.
        if (animator.GetInteger("RescueType") != 1)
            return;

        animator.SetInteger("RescueType", 0);

        rescuePlayer.CancelEvent();


    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
