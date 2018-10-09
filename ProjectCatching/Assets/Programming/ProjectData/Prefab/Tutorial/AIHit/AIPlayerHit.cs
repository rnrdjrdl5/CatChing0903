using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerHit : MonoBehaviour {

    Animator animator;
    AIHealth aIHealth;

    CollisionObject collisionObject;

    MathUtility mathUtility;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        aIHealth = GetComponent<AIHealth>();
        mathUtility = new MathUtility();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "CheckCollision" &&
            other.tag != "ItemCollision") return;

        collisionObject = other.gameObject.GetComponent<CollisionObject>();

        if (collisionObject == null) return;

        if (ReCheck(other, collisionObject)) return;

        LeftApplyDamage(other.gameObject, false, collisionObject);

        LeftDebuff(other.gameObject, false);

        LeftNumberOfCollision(other.gameObject, false);

        LeftAnimation(other.gameObject, false);

    }

    private bool ReCheck(Collider other, CollisionObject collisionObject)
    {

        // ReCheck 스크립트 받아옴
        CollisionReCheck[] CRCs = other.gameObject.GetComponents<CollisionReCheck>();

        // 충돌 이미 있는지 판단
        foreach (CollisionReCheck crc in CRCs)
        {

            if (crc.GetPlayerObject() == gameObject)
                return true;
        }

        // 충돌 대기시간 추가
        CollisionReCheck CRC = other.gameObject.AddComponent<CollisionReCheck>();

        // 충돌대상 등록
        CRC.SetPlayerObject(gameObject);

        // 충돌 재사용 대기시작 등록
        CRC.SetPlayerReCheckTime(collisionObject.GetCollisionReCheckTime());

        return false;
    }

    private void LeftApplyDamage(GameObject go, bool isItem, CollisionObject collisionObject)
    {
        // 받았는지 체크
        if (collisionObject == null)
            return;

        // 충돌 데미지 받아옴
        CollisionObjectDamage collisionObjectDamage
            = go.gameObject.GetComponent<CollisionObjectDamage>();

        // 이벤트
        if (collisionObjectDamage.ChangeEffectEvent != null)
            collisionObjectDamage.ChangeEffectEvent(aIHealth.health - collisionObjectDamage.GetObjectDamage(), animator.GetInteger("WeaponType"));

        // 데미지
        aIHealth.ApplyDamage(collisionObjectDamage.GetObjectDamage());

        //이펙트, 쉐이크
        CreateEffect((int)collisionObjectDamage.EffectType);

        // 데미지 충돌횟수 감소
        collisionObjectDamage.DecreaseObjectDamageNumber();

    }

    private void CreateEffect(int ET)
    {
        PoolingManager.EffctType effectType = (PoolingManager.EffctType)ET;

        if (effectType == PoolingManager.EffctType.NONE)
            return;



        GameObject effect = PoolingManager.GetInstance().CreateEffect(effectType);

        PoolingManager.GetInstance().CheckCameraShakeEffect(effect);



        float offsetUpPosition = PoolingManager.GetInstance().DecideEffectPosition(effectType);

        effect.transform.position = transform.position + Vector3.up * offsetUpPosition;

        if (PoolingManager.GetInstance().CheckAttachEffect(effectType))
        {
            effect.transform.SetParent(gameObject.transform);
            effect.transform.localPosition += Vector3.up * offsetUpPosition;
        }
        else
            effect.transform.position = transform.position + Vector3.up * offsetUpPosition;

    }

    private void LeftDebuff(GameObject go, bool isItem)
    {


        CollisionNotMoveDebuff collisionNotMoveDebuff = go.GetComponent<CollisionNotMoveDebuff>();
        CollisionStunDebuff collisionStunDebuff = go.GetComponent<CollisionStunDebuff>();
        CollisionDamagedDebuff collisionDamagedDebuff = go.GetComponent<CollisionDamagedDebuff>();
        CollisionGroggyDebuff collisionGroggyDebuff = go.GetComponent<CollisionGroggyDebuff>();
        CollisionSlideDebuff collisionSlideDebuff = go.GetComponent<CollisionSlideDebuff>();


        if (collisionNotMoveDebuff != null)
        {
            NotMoveDebuff(collisionNotMoveDebuff.GetMaxTime());
        }

        if (collisionStunDebuff != null)
        {
            StunDebuff(collisionStunDebuff.GetMaxTime());
        }

        if (collisionDamagedDebuff != null)
        {
            MathUtility.EnumDirVector DirVectorType;

            if (collisionObject.UsePlayerObject != null)
                DirVectorType = mathUtility.VectorDirType(gameObject, collisionObject.UsePlayerObject.transform.position);

            else
            {
                DirVectorType = mathUtility.VectorDirType(gameObject, go.transform.position);
            }




            DamagedDebuff((int)DirVectorType);
        }

        if (collisionGroggyDebuff != null)
        {
            GroggyDebuff(collisionGroggyDebuff.GetMaxTime());
        }

        if (collisionSlideDebuff != null)
        {
            SlideDebuff(collisionSlideDebuff.GetMaxTime());
        }


    }

    private void NotMoveDebuff(float MDT)
    {

        animator.SetBool("isNotMove", true);
        // 속박 받아옴
        PlayerNotMoveDebuff playerNotMoveDebuff = gameObject.GetComponent<PlayerNotMoveDebuff>();

        // 속박 없으면 새로 추가
        if (playerNotMoveDebuff == null)
        {
            playerNotMoveDebuff = gameObject.AddComponent<PlayerNotMoveDebuff>();

        }

        // 속박 설정 추가
        playerNotMoveDebuff.SetMaxDebuffTime(MDT);
        playerNotMoveDebuff.SetNowDebuffTime(0);
    }

    private void StunDebuff(float ST)
    {

        // 스턴 애니메이션 재생
        animator.SetBool("StunOnOff", true);

        // 스턴 디버프 받아오기
        PlayerStunDebuff playerStunDebuff = gameObject.GetComponent<PlayerStunDebuff>();

        // 스턴 없으면 새로 추가
        if (playerStunDebuff == null)
        {
            playerStunDebuff = gameObject.AddComponent<PlayerStunDebuff>();
        }

        // 스턴 속성 설정
        playerStunDebuff.SetMaxDebuffTime(ST);
        playerStunDebuff.SetNowDebuffTime(0);

    }

    private void DamagedDebuff(int DirVectorType)
    {

        if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.UP)
        {
            animator.SetInteger("DamagedType", 3);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.DOWN)
        {
            animator.SetInteger("DamagedType", 4);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.LEFT)
        {
            animator.SetInteger("DamagedType", 2);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.RIGHT)
        {
            animator.SetInteger("DamagedType", 1);
        }
    }

    private void GroggyDebuff(float GD)
    {

        // Groggy 설정

        // 경직 애니메이션 재생
        animator.SetBool("isGroggy", true);

        // 경직 디버프 받아오기
        PlayerGroggyDebuff playerGroggyDebuff = gameObject.GetComponent<PlayerGroggyDebuff>();

        // 경직 없으면 새로 추가
        if (playerGroggyDebuff == null)
        {
            playerGroggyDebuff = gameObject.AddComponent<PlayerGroggyDebuff>();
        }

        // 경직 속성 설정
        playerGroggyDebuff.SetMaxDebuffTime(GD);
        playerGroggyDebuff.SetNowDebuffTime(0);
    }

    private void SlideDebuff(float SD)
    {
        animator.SetBool("isSlide", true);
        PlayerSlideDebuff playerSlideDebuff = gameObject.GetComponent<PlayerSlideDebuff>();

        if (playerSlideDebuff == null)
        {
            playerSlideDebuff = gameObject.AddComponent<PlayerSlideDebuff>();
        }

        // 경직 속성 설정
        playerSlideDebuff.SetMaxDebuffTime(SD);
        playerSlideDebuff.SetNowDebuffTime(0);
    }

    private void LeftNumberOfCollision(GameObject go, bool isItem)
    {
        // 충돌 횟수 받아옴
        NumberOfCollisions numberOfCollisions = go.GetComponent<NumberOfCollisions>();
        CollisionAnimator collisionAnimator = go.GetComponent<CollisionAnimator>();


        if (numberOfCollisions == null)
            return;

        // 감소
        numberOfCollisions.DeCreaseNumberOfCollisions();


        if (numberOfCollisions.GetNumberOfCollisions() == 0 &&
            collisionAnimator == null)
        {
            PoolingManager.GetInstance().PushObject(go);
        }
    }

    private void LeftAnimation(GameObject go, bool isItem)
    {

        CollisionAnimator collisionAnimator = go.GetComponent<CollisionAnimator>();

        if (collisionAnimator == null)
            return;

        collisionAnimator.SetAnimatorMode();
    }
}
