using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultPlayerSkillDebuff
{
    // 스킬 디버프 속성입니다. 디버프 추가 시 이 속성을 보고 추가합니다.
    public enum EnumSkillDebuff { STUN , DAMAGED , NOTMOVE , GROGGY , SLIDE}
    
    public EnumSkillDebuff SkillDebuffType;

    public EnumSkillDebuff GetSkillDebuffType() { return SkillDebuffType; }
    public void SetSkillDebuffType(EnumSkillDebuff ESD) { SkillDebuffType = ESD; }

    // 디버프 창에서 확인 용으로 쓰인다.
    public bool EqualSkillDebuffType(EnumSkillDebuff ESD)
    {
        if (SkillDebuffType == ESD)
            return true;
        else
            return false;
    }


    // 최대 시간입니다.
    public float MaxTime;

    public float GetMaxTime() { return MaxTime; }
    public void SetMaxTime(float MT) { MaxTime = MT; }
    

}
