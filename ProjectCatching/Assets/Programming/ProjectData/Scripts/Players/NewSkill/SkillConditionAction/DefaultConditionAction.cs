using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**** 스킬에서 사용 ****/

[System.Serializable]
public class DefaultConditionAction{

    

    /**** public ****/

    protected SkillConditionOption skillConditionOption;               // 스킬 조건
    protected SkillContinueConditionOption skillCtnCdtOption;           // 지속스킬 조건


    /**** protected ****/

    protected DefaultNewSkill defaultNewSkill;              // 스킬 참조용 스크립트



    /**** 접근자 ****/

    public DefaultNewSkill GetdefaultNewSkill() { return defaultNewSkill; }
    public void SetdefaultNewSkill(DefaultNewSkill DNS) { defaultNewSkill = DNS; }




    /**** 가상함수 ****/

    // 조건 , 액션 사용
    public virtual void ConditionAction()
    {
        
    }

    // 조건 생성
    public virtual void InitCondition(DefaultNewSkill DNS)
    {
        skillConditionOption = new SkillConditionOption();
        skillCtnCdtOption = new SkillContinueConditionOption();
    }

   
}
