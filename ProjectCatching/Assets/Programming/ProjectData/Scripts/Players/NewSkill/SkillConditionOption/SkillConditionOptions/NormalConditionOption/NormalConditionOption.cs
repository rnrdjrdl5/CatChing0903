using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class NormalConditionOption : DefaultSkillConditionOption {
    public override bool CheckCondition()
    {
        {
            if (defaultSkill.GetphotonView() == null) return false;
            if (defaultSkill.GetphotonView().isMine)
            {
                // 2. 키가 눌린 경우
                if (defaultSkill.InputKey.IsUseKey())
                {

                    //3. 쿨타임 사용중이지 않을 때

                    if (!defaultSkill.coolDown.GetisUseCoolDown())
                    {

                        return true;

                    }
                }

            }
            
            return false;
        }

    }

    
}
