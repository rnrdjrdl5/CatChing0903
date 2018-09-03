using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**************************
 * 작성일 2018. 04. 03 
 * 작성자 : 반재억
 * 
 * 목적 : 쥐덪스킬 제작
 * **************************/
public class CatTrap : DefaultNewSkill
{


    public TrapState trapState;

    private PlayerMove playerMove;
    private SoundManager playerSoundManager;

    List<GameObject> Traps;

    

    protected override void Awake()
    {

        // 부모 Awake 사용
        base.Awake();
        
        // Normal 형태 조건 , 액션문 사용
        defaultCdtAct = new NormalCdtAct();

        // 값 설정
        defaultCdtAct.InitCondition(this);

        // 트랩 애니메이션뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isCatTrap", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);

        // 플레이어 무브 받아오기
        playerMove = GetComponent<PlayerMove>();

        // 플레이어 트랩 초기화
        Traps = new List<GameObject>();

        playerSoundManager = GetComponent<SoundManager>();


    }

    public override bool CheckState()
    {
        if (
             (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) )&&
            playerState.GetisUseAttack() == false &&
            playerState.isCanActive == true)
        {
            return true;
        }

        else
            return false;
    }

    public override void UseSkill()
    {
        animator.SetBool("isCatTrap", true);
        playerMove.ResetMoveSpeed();
    }

    void CreateTrap()
    {

        // 본인인 경우
        if(photonView.isMine)
        {
            // 비어있는 리스트 제거
            CheckEmptyTraps();

            // 트랩 개수 확인 후 제거
            CheckEraseTrap();


            // 모든 클라이언트에 전송
            photonView.RPC("RPCCreateTrap", PhotonTargets.All , PhotonNetwork.player.ID);
        }
    }

    void OffTrap()
    {

        animator.SetBool("isCatTrap", false);
    }

    void CheckEmptyTraps()
    {

        // 비어있는 트랩을 찾는다.

        for (int i = Traps.Count - 1; i >= 0; i--)
        {

            // 비활성화 트랩 찾으면 리스트에서 제거.
            if (Traps[i].activeSelf == false)
            {
                Debug.Log("비활성화 제거완료");
                Traps.Remove(Traps[i]);
            }
        }
    }

    void CheckEraseTrap()
    {
        while (true)
        {

            // 최대 개수보다 최소 하나이상 없을 때 나감.
            if (Traps.Count < trapState.MaxTrap)
                break;

            // 최대 갯수랑 같거나 많으면 리스트에서 하나씩 삭제함.
            else
            {
                Debug.Log("아.");

                Debug.Log("리스트 크기 : " + Traps.Count);
                Traps[0].GetComponent<Animator>().SetBool("isAction", false);
                photonView.RPC("RPCEraseTrap", PhotonTargets.All, Traps[0].GetComponent<ObjectIDScript>().ID);
                break;
            }
        }
    }





    /**** RPC ****/
    [PunRPC]
    void RPCCreateTrap(int ID)
    {
   
        float BulletDistance = 0f;
        float CharacterHeight = 0f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;

        GameObject Trap = PoolingManager.GetInstance().PopObject("Trap");

        Trap.transform.position = transform.position + (BulletDefaultPlace);
        Trap.transform.rotation = Quaternion.identity;


        trapState.SetData(Trap, gameObject, ID);

        // 발사체에 디버프를 넣습니다.
        AddDebuffComponent(Trap);

        // 본인은 추가로 list에 등록
        if (photonView.isMine)
        {
            Traps.Add(Trap);
        }


    }

    [PunRPC]
    void RPCEraseTrap(int ID)
    {

        Debug.Log("ID ! : " + ID);
        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(ID);
        Debug.Log("go : " + go);

        go.GetComponent<CollisionObject>().ResetSkillOption();

        PoolingManager.GetInstance().PushObject(go);

    }


    public void CallTrapSound()
    {

        playerSoundManager.PlayEffectSound(SoundManager.EnumEffectSound.EFFECT_CAT_TRAP_1);
    }
}
