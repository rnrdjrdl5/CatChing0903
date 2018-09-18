using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttack : DefaultNewSkill {

    public AttackState attackState;
    public SoundManager catSoundManager;


    public GameObject FryPan;

    private CollisionObject collisionObject;
    private CollisionObjectDamage collisionObjectDamage;

    private PoolingManager.EffctType AttackEffectType = PoolingManager.EffctType.ATTACKLINE1;
    public void ResetAttackEffectType() { AttackEffectType = PoolingManager.EffctType.ATTACKLINE1; }


    private bool isUsingPreAttack;

    



    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);

        catSoundManager = GetComponent<SoundManager>();

        isUsingPreAttack = false;
    }

    // 공격 조건을 재정의합니다.
    public override bool CheckState()
    {

        // 공격했다면.
        if ((playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
        playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
        playerState.EqualPlayerCondition(PlayerState.ConditionEnum.JUMP)) &&

        playerState.isCanActive == true)
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    public override void UseSkill()
    {
        isUsingPreAttack = true;
    }

    protected override void Update()
    {
        base.Update();


        if (isUsingPreAttack &&
            playerState.GetisUseAttack() == false)
        {
            playerState.SetisUseAttack(true);
            isUsingPreAttack = false;
            photonView.RPC("RPCAttack", PhotonTargets.All, (int)AttackEffectType);
        }

    }

    




    // Animation Events

    public void SetCanAttack(int EffectType)
    {
        if (!photonView.isMine) return;

        playerState.SetisUseAttack(false);
        AttackEffectType = (PoolingManager.EffctType)EffectType;
    }

    public void CreateFryPanOption()
    {

        // 모두다 실행한다. 하지만  특정 사람만 준다. 

        // 프라이팬에 데미지와 정보 스크립트를 추가합니다.
        collisionObjectDamage = FryPan.AddComponent<CollisionObjectDamage>();
        collisionObject = FryPan.AddComponent<CollisionObject>();

        // 정보 스크립트에 수치를 대입합니다.
        //attackState.SetData(collisionObjectDamage, collisionObject, photonView, PhotonNetwork.player.ID, gameObject);
        attackState.SetData(collisionObjectDamage, collisionObject, photonView,photonView.owner.ID, gameObject);
        // 디버프를 추가합니다. 공격에서는 경직 디버프가 들어있습니다.
        AddDebuffComponent(FryPan);
    }

    public void DeleteFryPanOption()
    {
        // 데미지가 있다면 삭제합니다.
        if (collisionObjectDamage != null)
            Destroy(collisionObjectDamage);

        // 정보가 있다면 삭제합니다.
        if (collisionObject != null)
            Destroy(collisionObject);

        // 데미지 디버프가 있다면 저장합니다.
        CollisionDamagedDebuff CDD = FryPan.GetComponent<CollisionDamagedDebuff>();

        // 저장한 디버프가존재한다면 
        if (CDD != null)
        {
            // 삭제합니다.
            Destroy(CDD);
        }

        // 모든 체크 대상을 불러옵니다.
        CollisionReCheck[] collisionRechecks = FryPan.GetComponents<CollisionReCheck>();

        // 루프를 돌면서 삭제합니다.
        for (int i = collisionRechecks.Length - 1; i >= 0; i--)
        {
            Destroy(collisionRechecks[i]);
        }
    }

    // Photon RPCs

    [PunRPC]
    private void RPCAttack(int attackCount)
    {

        animator.SetBool("isAttack", true);

        // 상태에 따라 오브젝트 풀에서 빼오기.
        GameObject effectObject = PoolingManager.GetInstance().CreateEffect((PoolingManager.EffctType)attackCount);

        // 이펙트 위치 지정
        effectObject.transform.position = transform.position;



        effectObject.transform.rotation = Quaternion.Euler(effectObject.transform.rotation.eulerAngles +
            effectObject.transform.rotation.eulerAngles);

        effectObject.transform.SetParent(transform);

        // 이펙트 진행
        effectObject.GetComponent<Animator>().SetBool("UseAction", true);


    }

    public void CheckChangeEffect(float damage, int WeapontType)
    {
        
        collisionObjectDamage = FryPan.GetComponent<CollisionObjectDamage>();

        if (collisionObjectDamage == null) return;

        if(damage <= 0 && WeapontType == 0)
            collisionObjectDamage.EffectType = PoolingManager.EffctType.ATTACK_FINISH;
        else
            collisionObjectDamage.EffectType = PoolingManager.EffctType.ATTACK;
    }



    public void CallAttackEffectSound()
    {
        catSoundManager.PlayRandomEffectSound
            (SoundManager.EnumEffectSound.EFFECT_CAT_ATTACK_1,
            SoundManager.EnumEffectSound.EFFECT_CAT_ATTACK_2);
    }

}
