using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObjectDamage : MonoBehaviour
{

    public delegate void ChangeEffectDele(float a, int Type);
    public ChangeEffectDele ChangeEffectEvent;

    // 충돌물체의 데미지 관련 정보.
    // 충돌물체의 데미지.   
    public float collisionObjectDamage;

    public void SetObjectDamage(float COD) { collisionObjectDamage = COD; }
    public float GetObjectDamage() { return collisionObjectDamage; }

    // 충돌물체의 데미지를 받는 횟수. 
    public float CollisionObjectDamageNumber;

    public void SetObjectDamageNumber(float SODN) { CollisionObjectDamageNumber = SODN; }
    public float GetObjectDamageNumber() { return CollisionObjectDamageNumber; }

    public void DecreaseObjectDamageNumber()
    {
        if (CollisionObjectDamageNumber >= 0)
            CollisionObjectDamageNumber -= 1;
    }

    public void ResetObject()
    {
        collisionObjectDamage = 0;

        CollisionObjectDamageNumber = 0;
    }

    public PoolingManager.EffctType EffectType { get; set; }
    public SoundManager.EnumRandomEffectSound effectSoundType { get; set; }

}
