using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimator : StateMachineBehaviour
{
    private FindObject findObject;
    private PlayerState playerState;
    private InteractiveState interactiveState;
    private NewInteractionSkill newInteractionSkill;
    private PhotonView photonView;

    void InitPhotonView(Animator animator)
    {
        photonView = animator.gameObject.GetComponent<PhotonView>();
    }

    void InitScripts(Animator animator)
    {

            findObject = animator.gameObject.GetComponent<FindObject>();


            playerState = animator.gameObject.GetComponent<PlayerState>();
    }

    void InitinteractiveState(Animator animator)
    {
         interactiveState = newInteractionSkill.GetinteractiveState();

    }

    void InitNewInteractionSkill(Animator animator)
    {
        newInteractionSkill = animator.GetComponent<NewInteractionSkill>();
    }


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        // 포톤뷰 스크립트 받기

        InitPhotonView(animator);

        // 스킬오브젝트 받기
        InitNewInteractionSkill(animator);

        // 본인이 아니면
        if (!photonView.isMine)
        {
            Debug.Log("번호 : " + newInteractionSkill.GetinterViewID());

            // 스킬오브젝트 . 오브젝트매니저 . 찾기 ( id로 ) 
            GameObject go = newInteractionSkill.GetobjectManager().FindObject(
                newInteractionSkill.GetinterViewID());



            Debug.Log(go);

            // skill에 상호작용물체, 상호작용  스크립트 등록.
            newInteractionSkill.SetinteractiveObject(go);
            newInteractionSkill.SetinteractiveState(go.GetComponent<InteractiveState>());


        }

        // 애니메이션 exit 사용 시 사용하기.
        // 상호작용 상태 받기
        //InitinteractiveState(animator);



        // 본인이면
        if (photonView.isMine)
        {

            // 다른 각종 스크립트 받기
            InitScripts(animator);

            // 상태이상 변경
            playerState.SetPlayerCondition(PlayerState.ConditionEnum.INTERACTION);
        }
        
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetInteger("InteractionType") == 0) return;

        
        animator.SetInteger("InteractionType", 0);
        Debug.Log("int수정 : " + animator.GetInteger("InteractionType"));

        EffectAniManager effectAniManager = animator.GetComponent<EffectAniManager>();

        effectAniManager.DestroyFirstEffect();

        PlayerMove playerMove = animator.GetComponent<PlayerMove>();

        if (playerMove == null) return;

        playerMove.ResetSkill();
    }

}
