using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelingCtnCdtOption : DefaultSkillContinueConditionOption {

    // 채널링 조건 잡기

        // 여기서는 스킬 사용 시 마나량이 충분한지만 파악,
        // 마나가 없어 해제되는 건 따로 뺐다.
    public override bool CheckContinueCondition()
    {
        // 1. 본인인가?

        if(defaultSkill.GetphotonView().isMine)
        {

            // 2. 스킬 사용중인가?
            if (GetisUseCtnSkill() == true)
            {
                // 3. 마나가 충분한가?

                if (defaultSkill.MaxCtnCoolTime > defaultSkill.GetNowCtnCoolTime() + defaultSkill.IncreaseCoolTimeTick * Time.deltaTime)
                {
                    return true;
                }
            }

            
        }
        return false;
    }






    override public bool CheckContinueExit()
    {
        if (defaultSkill.GetphotonView().isMine)
        {
            // 2. 키가 눌린 경우
            if (defaultSkill.ExitInputKey.IsUseKey())
            {
                return true;
            }
        }
        return false;
    }
}
