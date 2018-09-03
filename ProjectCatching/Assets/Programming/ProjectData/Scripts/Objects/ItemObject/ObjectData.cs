using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    // 모든 스킬은 해당 옵션중 필요한 옵션만 사용하게 됩니다.
    [Header(" - 회복량")]
    public float ObjectDamage = 10.0f;

    [Header(" - 데미지(회복량) 사용 가능 횟수")]
    public float ObjectDamageNumber = 10.0f;

    [Header(" - 충돌 횟수")]
    public float CollisionNumber = 1.0f;

    [Header(" - 충돌 재사용 대기시간")]
    public float ReCheckTime = 1000f;

    [Header(" - 자연 소멸 시간")]
    public float DestroyTime = 3.0f;

    public void SetData(int ownerID ,string ObjectName)
    {


        CollisionObject CollisionObjectScript = GetComponent<CollisionObject>();

        CollisionObjectDamage CollisionObjectDamageScript = GetComponent<CollisionObjectDamage>();

        CollisionObjectTime CollisionObjectTimeScript = GetComponent<CollisionObjectTime>();

        NumberOfCollisions NumberOfCollisionsScript = GetComponent<NumberOfCollisions>();

        CollisionAnimator collisionAnimator = GetComponent<CollisionAnimator>();

        if (CollisionObjectScript != null)
        {
            CollisionObjectScript.SetCollisionReCheckTime(ReCheckTime);
            CollisionObjectScript.PlayerIOwnerID = ownerID;
            if (ObjectName == "YSK_FX_Cheese_0528")
            {
                CollisionObjectScript.SetNoUseCollisionType(CollisionObject.NoUseCollision.CAT);
                CollisionObjectScript.SetNoUseCollisionType(CollisionObject.NoUseCollision.ROPE);
            }
        }

        if (CollisionObjectDamageScript != null)
        {
            CollisionObjectDamageScript.SetObjectDamage(ObjectDamage * -1.0f);
            CollisionObjectDamageScript.SetObjectDamageNumber(ObjectDamageNumber);
            CollisionObjectDamageScript.EffectType = PoolingManager.EffctType.CHEESE_HEALING_EFFECT;
        }

        if (CollisionObjectTimeScript != null)
        {
            CollisionObjectTimeScript.SetObjectTime(DestroyTime);
        }

        if (NumberOfCollisionsScript != null)
        {
            NumberOfCollisionsScript.SetNumberOfCollisions(CollisionNumber);
        }
    }
}
