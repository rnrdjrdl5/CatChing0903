using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class PhotonManager

{    // UI삭제하고 일정 시간 대기하는 트리거 사용
    [PunRPC]
    void RPCActionCheckGameFinish(int Type)
    {

        StartCoroutine("GameResultUI", Type);

    }

    IEnumerator GameResultUI(int Type)
    {
        DeleteResult(Type);


        GameFinishType = SetGameWinLoseResult(Type);

        SpringArmObject.GetInstance().GetSystemSoundManager().FadeOutSound();
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.UI_TIMEOVER_1);


        yield return new WaitForSeconds(FinishGame_Between_WinLoseUI);


        // 1. 패널 보여주기
        uIManager.gameResultPanelScript.GameResultPanel.SetActive(true);

        // 2. 게임종료 이벤트 발생
        GameFinishEvent((int)GameFinishType);

        yield return new WaitForSeconds(WinLoseUI_Between_FinishFadeOut);

        uIManager.gameResultPanelScript.GameResultPanel.SetActive(false);
        uIManager.endStatePanelScript.SetEndState(false, EndStatePanelScript.ResultType.BREAK);
        uIManager.fadeImageScript.SetAlpha(1.0f);
        uIManager.fadeImageScript.FadeImage.SetActive(true);




        PlayVideoEndScene((int)GameFinishType);
        // 영상재생
    }

    void PlayVideoEndScene(int winType)
    {
        GameObject go = null;


        if (winType == 0)
        {
            go = VideoManager.GetInstance().winLoseVideoScript.CatWinVideo;

            SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound
                (SoundManager.EnumEffectSound.UI_CAT_WIN);

        }

        else if (winType == 1)
        {
            go = VideoManager.GetInstance().winLoseVideoScript.MouseWinVideo;

            SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound
                (SoundManager.EnumEffectSound.UI_MOUSE_WIN);
        }



        go.SetActive(true);

        AutoDestroyVideo autoDestroyVideo = go.GetComponent<AutoDestroyVideo>();
        autoDestroyVideo.AttachEvent(ExitGameRoom);



    }





    // 개인 사용자용 액션들


    void ExitGameRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // 플레이어 삭제
    void DeleteResult(int i)
    {



        uIManager.hpPanelScript.SetHealthPoint(false);



        uIManager.limitTimePanelScript.SetLimitTime(false);
        uIManager.SetAim(false);
        uIManager.gradePanelScript.GradePanel.SetActive(false);
        uIManager.skillPanelScript.SkillPanel.SetActive(false);

        uIManager.pressImagePanelScript.PressImagePanel.SetActive(false);


        // 쥐 남은 수 끄기
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(false);

        uIManager.deadOutLinePanelScript.DeadOutLinePanel.SetActive(false);




        // 플레이어 Result UI 설정
        uIManager.endStatePanelScript.SetEndState(true, (EndStatePanelScript.ResultType)i);

        uIManager.OverlayCanvas.SetActive(false);


    }


    // 게임결과
    public EnumGameFinish SetGameWinLoseResult(int ResultType)
    {
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];
        EnumGameFinish GameResult = EnumGameFinish.MOUSEWIN;

        GameResult = PlayerGameResult(ResultType);

        return GameResult;

    }

    // 게임결과
    public EnumGameFinish PlayerGameResult(int type)
    {
        EnumGameFinish enumGameFinish = EnumGameFinish.CATWIN;

        switch (type)
        {
            case 0:
                enumGameFinish = EnumGameFinish.MOUSEWIN;
                break;

            case 1:
                enumGameFinish = EnumGameFinish.CATWIN;
                break;

            case 2:
                enumGameFinish = TimeOutGameResult();
                break;
                

        }

        return enumGameFinish;

    }

    public EnumGameFinish TimeOutGameResult()
    {
        float CatGradeScore = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Cat")
            {
                CatGradeScore = (float)PhotonNetwork.playerList[i].CustomProperties["CatScore"];
                break;
            }
        }

        float CatGradePersent = (float)CatGradeScore / (float)PhotonManager.GetInstance().MaxCatScore * 100;

        EnumGameFinish enumGameFinish = EnumGameFinish.CATWIN;

        if (CatGradePersent > GameTimeOutCondition)
            enumGameFinish = EnumGameFinish.CATWIN;

        else
            enumGameFinish = EnumGameFinish.MOUSEWIN;

        return enumGameFinish;
    }


}
