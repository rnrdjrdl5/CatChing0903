using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionObject
{
    public void ResetObject()
    {
        CollisionReCheckTime = 0.0f;

        UsePlayer = null;

        PlayerIOwnerID = 0;

    }

    public void ResetSkillOption()
    {

        CollisionObjectDamage collisionObjectDamage = GetComponent<CollisionObjectDamage>();
        NumberOfCollisions numberOfCollisions = GetComponent<NumberOfCollisions>();
        CollisionStunDebuff collisionStunDebuff = GetComponent<CollisionStunDebuff>();
        CollisionNotMoveDebuff collisionNotMoveDebuff = GetComponent<CollisionNotMoveDebuff>();
        CollisionDamagedDebuff collisionDamagedDebuff = GetComponent<CollisionDamagedDebuff>();
        CollisionGroggyDebuff collisionGroggyDebuff = GetComponent<CollisionGroggyDebuff>();
        CollisionTorqueMove collisionTorqueMove = GetComponent<CollisionTorqueMove>();

        // 정보 초기화 필요
        this.ResetObject();

        if (collisionObjectDamage != null)
            collisionObjectDamage.ResetObject();

        if (numberOfCollisions != null)
            numberOfCollisions.ResetObject();

        if (collisionTorqueMove != null)
            collisionTorqueMove.ResetObject();



        if (collisionStunDebuff != null)
            Destroy(collisionStunDebuff);

        if (collisionNotMoveDebuff != null)
            Destroy(collisionNotMoveDebuff);

        if (collisionDamagedDebuff != null)
            Destroy(collisionDamagedDebuff);

        if (collisionGroggyDebuff != null)
            Destroy(collisionGroggyDebuff);

        // ReCheck 스크립트 받아옴
        CollisionReCheck[] CRCs = GetComponents<CollisionReCheck>();

        for (int i = CRCs.Length - 1; i >= 0; i--)
        {
            Destroy(CRCs[i]);
        }


        CollisionObjectTime collisionObjectTime = GetComponent<CollisionObjectTime>();

        if (collisionObjectTime != null)
            collisionObjectTime.ResetObject();
    }




}
