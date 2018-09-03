using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapState{

    // 모든 스킬은 해당 옵션중 필요한 옵션만 사용하게 됩니다.
    [Header(" - 오브젝트")]
    [Tooltip(" - 쥐덪의 FBX파일입니다. ")]
    public GameObject ProjectileObject;

    [Header(" - 스킬 속성")]
    [Tooltip(" - 쥐덪 데미지입니다.")]
    public float ObjectDamage = 10.0f;

    [Tooltip(" - 쥐덪 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;

    [Tooltip(" - 쥐덪 충돌 제한 횟수입니다.")]
    public float CollisionNumber = 1.0f;

    [Tooltip(" - 쥐덪 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    [Tooltip(" - 쥐덪 자연  소멸 시간입니다.")]
    public float DestroyTime = 3.0f;

    [Tooltip(" - 쥐덫 최대 설치 개수 ")]
    public int MaxTrap = 3;

    // 데이터를 설정합니다.
    public void SetData(GameObject CollisionGameObject, GameObject PlayerObject , int ID)
    {
        CollisionObject CollisionObjectScript = CollisionGameObject.GetComponent<CollisionObject>();

        CollisionObjectDamage CollisionObjectDamageScript = CollisionGameObject.GetComponent<CollisionObjectDamage>();

        CollisionObjectTime CollisionObjectTimeScript = CollisionGameObject.GetComponent<CollisionObjectTime>();

        NumberOfCollisions NumberOfCollisionsScript = CollisionGameObject.GetComponent<NumberOfCollisions>();

        CollisionAnimator collisionAnimator = CollisionGameObject.GetComponent<CollisionAnimator>();

        if (CollisionObjectScript != null)
        {
            CollisionObjectScript.SetCollisionReCheckTime(ReCheckTime);
            CollisionObjectScript.SetUsePlayer("Player" + PlayerObject.GetPhotonView().viewID);
            CollisionObjectScript.PlayerIOwnerID = ID;
        }

        if (CollisionObjectDamageScript != null)
        {
            CollisionObjectDamageScript.SetObjectDamage(ObjectDamage);
            CollisionObjectDamageScript.SetObjectDamageNumber(ObjectDamageNumber);
            CollisionObjectDamageScript.EffectType = PoolingManager.EffctType.TRAP_EFFECT;
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
