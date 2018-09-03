using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState{

    [Header(" - 데미지")]
    [Tooltip(" - 프라이팬의 데미지입니다.")]
    public float AttackDamage;

    [Tooltip(" - 프라이팬의 데미지 충돌 횟수입니다.")]
    public float DamageNumber;

    [Tooltip(" - 같은 대상에게 재충돌체크 제한 시간입니다.")]
    public float RecheckTime;




    public void SetData(CollisionObjectDamage cod,
        CollisionObject co,
        PhotonView pv, int ID,
        GameObject gameObject)

    {
        cod.SetObjectDamage(AttackDamage);
        cod.SetObjectDamageNumber(DamageNumber);
        cod.EffectType = PoolingManager.EffctType.ATTACK;
        cod.effectSoundType = SoundManager.EnumRandomEffectSound.EFFECT_ATTACK_1;

        co.SetUsePlayer("Player" + pv.viewID);
        co.SetCollisionReCheckTime(RecheckTime);
        co.UsePlayerObject = gameObject;

        CatAttack catAttack = gameObject.GetComponent<CatAttack>();
        if (catAttack == null) return;

        cod.ChangeEffectEvent = catAttack.CheckChangeEffect;

        if (pv.isMine)
            co.PlayerIOwnerID = ID;

    }


}
