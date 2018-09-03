using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneCtnCdtOption : DefaultSkillContinueConditionOption {

    // 아무것도 없는 진행형


    public override bool CheckContinueCondition()
    {
        return false;
    }
}
