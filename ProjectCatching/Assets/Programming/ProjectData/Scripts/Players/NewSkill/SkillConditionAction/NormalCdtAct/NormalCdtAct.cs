using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCdtAct : DefaultConditionAction
{
    public override void InitCondition(DefaultNewSkill DNS)
    {
        // 스킬 조건 스크립트 초기화
        base.InitCondition(DNS);

        // 스킬 스크립트 설정
        SetdefaultNewSkill(DNS);

        //조건 설정
        skillConditionOption.SetskillCdtOpt(new NormalConditionOption());
        skillConditionOption.InitDefaulSkill(DNS);

        skillCtnCdtOption.SetskillConditionContinueOption(new NoneCtnCdtOption());
        skillCtnCdtOption.InitDefaultSkill(DNS);

    }

    public override void ConditionAction()
    {

        
        // 플레이어 스킬 사용 조건

        // 스킬조건
        if(skillConditionOption.CheckCondition())
        {
            // 플레이어 상태 확인
            if (defaultNewSkill.CheckState())
            {

                //스킬 사용
                defaultNewSkill.UseSkill();

                // 쿨타임 적용
                defaultNewSkill.coolDown.CalcCoolDown();

                // 쿨타임 시작
                defaultNewSkill.coolDown.SetisUseCoolDown(true);

                // 쿨타임 시작되었다는 이벤트 발생
                if(defaultNewSkill.coolDown.CanNotUseSkillEvent != null)
                     defaultNewSkill.coolDown.CanNotUseSkillEvent(defaultNewSkill.coolDown.SkillNumber);
            }
        }


        // 스킬 쿨타임 갱신
        if (defaultNewSkill.GetphotonView().isMine)
        {
            defaultNewSkill.coolDown.DecreaseCoolDown();
        }
    }

}
