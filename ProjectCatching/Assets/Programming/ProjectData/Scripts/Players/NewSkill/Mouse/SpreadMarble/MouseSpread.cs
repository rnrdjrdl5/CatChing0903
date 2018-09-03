using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpread : DefaultNewSkill {

    public MarbleState marbleState;


    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);

        // 애니메이션 포톤 뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isUseMarble", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    public override bool CheckState()
    {
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
             playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)) &&
                animator.GetBool("isUseMarble") == false &&
                playerState.isCanActive == true &&
                playerState.GetWeaponType() != PlayerState.WeaponEnum.ROPE)

        {
            return true;
        }
        else
            return false;
    }

    // 재정의
    public override void UseSkill()
    {
        animator.SetBool("isUseMarble", true);
    }

    // 애니메이션 이벤트

    void CreateMarble()
    {

        // 클라이언트 주인인 경우
        if (photonView.isMine)
        {

            // 모든 클라이언트에게 공격을 전송
            photonView.RPC("RPCCreateMarble", PhotonTargets.All, PhotonNetwork.player.ID);
        }
    }

    void OffSpread()
    {

        animator.SetBool("isUseMarble", false);
    }

    // RPC
    [PunRPC]
    void RPCCreateMarble(int ID)
    {
        float CharacterHeight = 0.1f;

        Vector3 BulletDefaultPlace = Vector3.zero;

        BulletDefaultPlace.y += CharacterHeight;


        Quaternion BulletRotation = transform.rotation;

        GameObject marble = PoolingManager.GetInstance().PopObject(marbleState.ObjectName);

        marble.transform.position = transform.position + (BulletDefaultPlace);

        marbleState.SetData(marble, gameObject, ID);

        // 발사체에 디버프를 넣습니다.
        AddDebuffComponent(marble);


        // 이펙트
        GameObject ballDropEffect = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.BALL_DROP);

        ballDropEffect.transform.position = transform.position;
    }
}
