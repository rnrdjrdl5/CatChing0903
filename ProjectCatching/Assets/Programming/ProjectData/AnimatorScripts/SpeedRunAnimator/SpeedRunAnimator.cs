using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRunAnimator : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerState>().SetPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // 달리기 스크립트 받아오기
        NewSpeedRun newSpeedRun = animator.GetComponent<NewSpeedRun>();

        // 플레이어 이동 스크립트 받아오기


        // 1. 이동속도 원래대로 돌림
        newSpeedRun.GetplayerMove().PlayerSpeed = newSpeedRun.GetPlayerOriginalSpeed();
        newSpeedRun.GetplayerMove().PlayerBackSpeed = newSpeedRun.GetPlayerOriginalBackSpeed();

        // 2. 애니메이션 일반 상태로 돌림
        animator.SetBool("isSpeedRun", false);

        // 3. 애니메이션 속도 복구시킴
        newSpeedRun.GetplayerMove().SetAniSpeedUp(newSpeedRun.GetOriginalAniSpeed());

        // 4. 지속시간 초기화
        newSpeedRun.SetCheckTime(0.0f);
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
/*	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }*/
}
