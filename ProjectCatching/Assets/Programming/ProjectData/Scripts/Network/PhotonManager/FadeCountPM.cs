using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PhotonManager
{

    // 생성 완료 후 Fade 처리, 카운트 처리 부분
    [PunRPC]
    void RPCPreStartCount()
    {

        StartCoroutine("ChaneBookUIUsedFade");
    }

    IEnumerator ChaneBookUIUsedFade()
    {

        uIManager.fadeImageScript.FadeImage.SetActive(true);
        uIManager.fadeImageScript.SetAlpha(0);


        UIEffect uIEffect = new UIEffect();
        uIEffect.AddFadeEffectNode(uIManager.fadeImageScript.FadeImage, MenuUIFadeInFadeOut, UIEffectNode.EnumFade.IN);
        uIEffect.AddUIEffectCustom(OffMenuUIActive);
        uIEffect.AddFadeEffectNode(uIManager.fadeImageScript.FadeImage, MenuUIFadeInFadeOut, UIEffectNode.EnumFade.OUT);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;

        yield return new WaitForSeconds(MenuUIFadeInFadeOut * 2);

        uIManager.fadeImageScript.SetAlpha(1.0f);
        uIManager.fadeImageScript.FadeImage.SetActive(false);

        SpringArmObject.GetInstance().GetSystemSoundManager().PlayBGSound(SoundManager.EnumBGSound.BG_INGAME_SOUND);
        StartGamePlayCount();
    }

    void StartGamePlayCount()
    {

        UIManager.GetInstance().gameStartCountPanelScript.GameStartCountPanel.SetActive(true);
        TimerValue = 3.0f;

        condition = new Condition(CheckTimeWait);
        conditionLoop = new ConditionLoop(DecreateTimeCountImageAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckCreatePlayer");
        StartCoroutine(IEnumCoro);
    }

    // 대기시간 기다릴 때 이 모두 흘렀는지 파악
    bool CheckTimeWait()
    {
        if (TimerValue <= 0)
        {
            TimerValue = 0;
            return true;
        }
        else
            return false;
    }

    void OffMenuUIActive()
    {
        uIManager.menuUIPanelScript.OffActive();
        uIManager.selectCharPanelScript.OffActive();

        uIManager.hpPanelScript.SetHealthPoint(true);



        uIManager.limitTimePanelScript.SetLimitTime(true);
        UpdateTimeEvent = uIManager.limitTimePanelScript.TimeTickUpdateEvent;

        uIManager.SetAim(true);
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(true);

        uIManager.gradePanelScript.GradePanel.SetActive(true);
        uIManager.gradePanelScript.SetActiveObjects(true);

        uIManager.skillPanelScript.SkillPanel.SetActive(true);

        uIManager.pressImagePanelScript.PressImagePanel.SetActive(true);
    }
}
