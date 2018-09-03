using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoolDown
{
    public int SkillNumber { get; set; }     // 플레이어 의 스킬 중 몇번째의 스킬인지.

    public delegate void DeleDecrease(float NowTime ,float MaxTime, int SkillNumber);
    public delegate void DeleCanUseSkill(int SkillNumber);

    public DeleDecrease DecreaseEvent;

    public DeleCanUseSkill CanUseSkillEvent;
    public DeleCanUseSkill CanNotUseSkillEvent;


    // 쿨타임 돌아가는지 여부를 판단합니다.
    private bool isUseCoolDown = false;

    public void SetisUseCoolDown(bool cd) { isUseCoolDown = cd; }
    public bool GetisUseCoolDown() { return isUseCoolDown; }


    // 현재 쿨타임을 나타내줍니다.
    private float NowCoolDown;
    
    public float GetNowCoolDown() { return NowCoolDown; }
    public void SetNowCoolDown(float NCD) { NowCoolDown = NCD; }


    // 최대 쿨타임을 나타냅니다.
    public float MaxCoolDown;

    // 시간 당 쿨타임 감소
    public void DecreaseCoolDown()
    {
        // 쿨타임이 줄어드는 상태라면
        if(isUseCoolDown)
        {

            NowCoolDown -= Time.deltaTime;

            // Decrease이벤트를 위해 음수로 나오지 않게 함
            if (NowCoolDown <= 0)
                NowCoolDown = 0;

            if(DecreaseEvent != null)   
                 DecreaseEvent(NowCoolDown, MaxCoolDown, SkillNumber);


            if (NowCoolDown <= 0)
            {
                isUseCoolDown = false;
                NowCoolDown = 0;
                if(CanUseSkillEvent!= null)
                     CanUseSkillEvent(SkillNumber);
            }
        }
    }


    public void CalcCoolDown()
    {
        NowCoolDown = MaxCoolDown;
    }

}
