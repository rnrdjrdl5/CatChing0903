using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillContinueConditionOption{

    // 스킬 조건을 외부에서 정할 수 있도록 합니다.
    public enum EnumSkillContinueConditionOption { NONE , CHANNELING }

    public EnumSkillContinueConditionOption skillContinueConditionType;

    /* 스킬이 사용되었는지 판단합니다.

      스킬이 사용되었다면 그때부터 체크에 들어갑니다.*/


    // 실제로 스킬 조건을 판단합니다.
    private DefaultSkillContinueConditionOption skillConditionContinueOption;

    public DefaultSkillContinueConditionOption GetskillConditionContinueOption() { return skillConditionContinueOption; }
    public void SetskillConditionContinueOption(DefaultSkillContinueConditionOption DSCO) { skillConditionContinueOption = DSCO; }

    // 처음으로 열거형에 따라서 스킬 조건 스크립트를 초기화합니다.
    public void InitDefaultSkill(DefaultNewSkill DS)
    {

        // 2. 스크립트의 정보값을 대입합니다.
        skillConditionContinueOption.SettingDefaultNewSkill(DS);
    }



    // 지속조건을 체크합니다.
    // 조건은 열거형에 따라 달라집니다.
    public bool CheckContinueCondition()
    {
        return skillConditionContinueOption.CheckContinueCondition();
    }

    // 해제 조건을 체크합니다.
    public bool CheckContinueExit()
    {
        return skillConditionContinueOption.CheckContinueExit();
    }


    //  조건 체크 시 사용되는 함수입니다.
    bool EqualSkillConditionType(EnumSkillContinueConditionOption SCO)
    {
        if (skillContinueConditionType == SCO)
            return true;
        else
            return false;
    }
}
