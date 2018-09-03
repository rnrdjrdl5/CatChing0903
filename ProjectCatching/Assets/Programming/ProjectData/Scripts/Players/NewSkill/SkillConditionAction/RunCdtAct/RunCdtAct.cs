using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCdtAct : DefaultConditionAction
{
    public override void InitCondition(DefaultNewSkill DNS)
    {
        // 스킬 조건 스크립트 인스턴스화
        base.InitCondition(DNS);

        // 스킬 스크립트 설정
        SetdefaultNewSkill(DNS);

        // 조건 설정
        skillConditionOption.SetskillCdtOpt(new NormalConditionOption());

        // 자식 스크립트에 skill 설정
        skillConditionOption.InitDefaulSkill(DNS);      

        //조건 설정
        skillCtnCdtOption.SetskillConditionContinueOption(new ChannelingCtnCdtOption());

        // 자식 스크립트에 skill 설정
        skillCtnCdtOption.InitDefaultSkill(DNS);

    }

    public override void ConditionAction()
    {

        // 스킬 시작 조건 판단
        if(skillConditionOption.CheckCondition())
        {

            // 플레이어 상태확인
            if(defaultNewSkill.CheckState())
            {
                
                // 지속성 스킬 여부 확인
                if(skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == false)
                {

                    Debug.Log("1");
                    // 스킬 사용
                    defaultNewSkill.UseSkill();


                    defaultNewSkill.isUseCtnCoolTime = true;

                    // 지속성 스킬 사용
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(true);
                }
            }
        }

        // 지속성 스킬 조건 판단
        if(skillCtnCdtOption.CheckContinueCondition())
        {

           if(defaultNewSkill.CheckCtnState())
            {
                Debug.Log("2");
                // 지속성 스킬을 사용합니다.
                defaultNewSkill.UseCtnSkill();

                // 마나가 비슷한 쿨타임 줄어들도록.
                defaultNewSkill.UpdateIncreaseCtnCoolTime();
            }
            else
            {
                Debug.Log("3");
                // 퇴장스킬
                defaultNewSkill.ExitCtnSkill();

                // 스킬 사용중 해제
                skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);

                if (defaultNewSkill.isUseCtnCoolTime == true)
                    defaultNewSkill.isUseCtnCoolTime = false;
            }
        }

        // 마나 충분한지 여부
        if(defaultNewSkill.GetphotonView().isMine)
        {
    
            // 2. 스킬 사용중인가?
            if (skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
            {
                Debug.Log("5");
                if (defaultNewSkill.MaxCtnCoolTime <=
                    defaultNewSkill.GetNowCtnCoolTime() + defaultNewSkill.IncreaseCoolTimeTick * Time.deltaTime)
                {
                    Debug.Log("6");

                    defaultNewSkill.ExitCtnSkill();
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);

                    defaultNewSkill.coolDown.CalcCoolDown();

                    defaultNewSkill.coolDown.SetisUseCoolDown(true);

                    defaultNewSkill.coolDown.CanNotUseSkillEvent(defaultNewSkill.coolDown.SkillNumber);

                    if (defaultNewSkill.isUseCtnCoolTime == true)
                        defaultNewSkill.isUseCtnCoolTime = false;
                }
            }
        }
        
        //스킬 사용중 체크
        if(skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
        {
            Debug.Log("7");
            // 스킬 해제 조건 체크
            if (skillCtnCdtOption.CheckContinueExit())
            {

                //  상태판단
                if(defaultNewSkill.CheckCtnState())
                {
                    Debug.Log("8");
                    defaultNewSkill.ExitCtnSkill();
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);

                    if (defaultNewSkill.isUseCtnCoolTime == true)
                        defaultNewSkill.isUseCtnCoolTime = false;
                }
            }
        }


        if (defaultNewSkill.GetphotonView().isMine)
        {
            // 스킬 쿨타임 갱신
            defaultNewSkill.coolDown.DecreaseCoolDown();

            // 쿨타임 비슷한거 회복
            defaultNewSkill.UpdateDecreaseCtnCoolTime();
        }
    }
}
