using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DefaultNewSkill을 상속받습니다.
public class NewThrowFryingPan : DefaultNewSkill
{
    // 스킬의 옵션이 결정됩니다.
    // 스킬에서 사용하지 않는 옵션은 조정해봤자 의미 없습니다.
    public TorqueProjectileState torqueProjectileState ;

    private PointToLocation PTL;

    public GameObject fryFan;
    public GameObject GetfryFan() { return fryFan; }
    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);

        // 애니메이션 포톤 뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isThrowFryingPan", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);

        PTL = new PointToLocation();

        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name == "Object_Fryfan")
                fryFan = transforms[i].gameObject;
        }
    }

    // 재정의
    public override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
             playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)) &&
             playerState.GetisUseAttack() == false &&
             playerState.isCanActive == true) 

        {
            return true;
        }
        else
            return false;
    }

    // 재정의
    public override void UseSkill()
    {
        playerState.SetisUseAttack(true);
        animator.SetBool("isThrowFryingPan", true);
    }

    IEnumerator ActivefryFan()
    {

        yield return new WaitForSeconds(0.3f);
        fryFan.SetActive(true);
        yield break;
    }


    // 애니메이션 이벤트입니다.
    void CreateFryingPan()
    {
        fryFan.SetActive(false);

        StartCoroutine("ActivefryFan");
        // 클라이언트 주인인 경우
        if (photonView.isMine)
        {

            // 모든 클라이언트에게 공격을 전송
            photonView.RPC("RPCCreateFryingPan", PhotonTargets.All, PhotonNetwork.player.ID, PTL.FindMouseCursorPosition(SpringArmObject.GetInstance().armCamera));
        }
    }

    void OffFryingPan()
    {
        fryFan.SetActive(true);
        playerState.SetisUseAttack(false);
        animator.SetBool("isThrowFryingPan", false);
    }



    /**** RPC ****/

    [PunRPC]
    void RPCCreateFryingPan(int ID , Vector3 PanDir)
    {
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;


        Quaternion BulletRotation = transform.rotation;

        Vector3 v3 = Vector3.zero;
        v3.x = -45;
        v3.y = 90 + transform.rotation.eulerAngles.y;
        v3.z = 0;


        GameObject FryPan = PoolingManager.GetInstance().PopObject("FryPan");

        GameObject cameraObject = SpringArmObject.GetInstance().armCamera;

        FryPan.transform.position = transform.position +
            transform.right * playerManager.PlayerViewPosition.x +
            transform.up * playerManager.PlayerViewPosition.y;
            
            //transform.position + (BulletDefaultPlace);
        FryPan.transform.rotation = Quaternion.Euler(v3);

        torqueProjectileState.SetData(FryPan, gameObject, ID, PanDir);

        // 발사체에 디버프를 넣습니다.
         AddDebuffComponent(FryPan);

    }

}
