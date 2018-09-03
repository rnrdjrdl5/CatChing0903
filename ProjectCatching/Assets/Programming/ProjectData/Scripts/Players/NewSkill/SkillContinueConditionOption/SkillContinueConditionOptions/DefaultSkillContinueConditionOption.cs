using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkillContinueConditionOption{


    // 지속형 스킬이 사용중인지 판단합니다.
    private bool isUseCtnSkill = false;

    public bool GetisUseCtnSkill() { return isUseCtnSkill; }
    public void SetisUseCtnSkill(bool UCS) { isUseCtnSkill = UCS; }

    
    

    // 스킬에서 데이터를 사용하기 위해 선언한 변수
    protected DefaultNewSkill defaultSkill;


    public void SettingDefaultNewSkill(DefaultNewSkill dns)
    {
        defaultSkill = dns;
    }

    virtual public bool CheckContinueCondition()
    {
        return false;
    }

    virtual public bool CheckContinueExit()
    {
        return false;
    }





}
