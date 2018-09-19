using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarbleState {

    // 대상 여부.

    [Header(" - 오브젝트")]
    [Tooltip(" -FBX이름입니다.. ")]
    public string ObjectName;

    [Header(" - 스킬 속성")]
    [Tooltip(" - 데미지입니다.")]
    public float MarbleDamage = 10.0f;

    [Tooltip(" - 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;

    [Tooltip(" - 충돌 제한 횟수입니다.")]
    public float CollisionNumber = 1.0f;

    [Tooltip(" - 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    [Tooltip(" - 자연  소멸 시간입니다.")]
    public float DestroyTime = 3.0f;




    // 데이터를 설정합니다.
    public void SetData(GameObject CollisionGameObject, GameObject PlayerObject, int ID)
    {
        CollisionObject CollisionObjectScript = CollisionGameObject.GetComponent<CollisionObject>();



        CollisionObjectDamage CollisionObjectDamageScript = CollisionGameObject.GetComponent<CollisionObjectDamage>();

        CollisionObjectTime CollisionObjectTimeScript = CollisionGameObject.GetComponent<CollisionObjectTime>();

        NumberOfCollisions NumberOfCollisionsScript = CollisionGameObject.GetComponent<NumberOfCollisions>();

        if (CollisionObjectScript != null)
        {
            CollisionObjectScript.SetCollisionReCheckTime(ReCheckTime);
            CollisionObjectScript.SetUsePlayer("Player" + PlayerObject.GetPhotonView().viewID);
            CollisionObjectScript.PlayerIOwnerID = ID;
            CollisionObjectScript.SetNoUseCollisionType(CollisionObject.NoUseCollision.MOUSE);
            CollisionObjectScript.SetNoUseCollisionType(CollisionObject.NoUseCollision.ROPE);
        }

        if (CollisionObjectDamageScript != null)
        {
            CollisionObjectDamageScript.SetObjectDamage(MarbleDamage);
            CollisionObjectDamageScript.SetObjectDamageNumber(ObjectDamageNumber);
            CollisionObjectDamageScript.EffectType = PoolingManager.EffctType.BALL_HIT;
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
