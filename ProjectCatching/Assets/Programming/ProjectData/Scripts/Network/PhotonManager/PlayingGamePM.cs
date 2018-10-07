using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PhotonManager
{


    // UI 보여주고 게임 종료 조건 파악하는 트리거 사용
    [PunRPC]
    void RPCActionCheckCreatePlayer()
    {
        StopCoroutine(IEnumCoro);


        GameStartCountPanelScript countScript = UIManager.GetInstance().gameStartCountPanelScript;

        for (int i = 0; i < countScript.Count.Length; i++)
        {
            countScript.Count[i].SetActive(false);
        }

        //스타트 이미지.
        StartCoroutine(WaitStartImage());

        CurrentPlayer.GetComponent<PlayerState>().isCanActive = true;


        // 고양이 플레이어 변수 설정
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

            if (PlayerType == "Cat")
            {

                CatPhotonPlayer = PhotonNetwork.playerList[i];
                break;
            }

        }



            // 게임 종료 조건 시작
            condition = new Condition(CheckGameFinish);
            conditionLoop = new ConditionLoop(NoAction);
            rPCActionType = new RPCActionType(MasterResultCheck);
            IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameFinish");
            StartCoroutine(IEnumCoro);


            StartCoroutine(Timer());


            

    }


    // 게임 끝났는지 파악,
    bool CheckGameFinish()
    {
        if (CheckMouseAllDead() || CheckEndTimer() || CheckAllBreak() || CheckCatDead())
            return true;

        else
            return false;
    }


   
    int MasterResultCheck()
    {
        int Type = -1;

        if (CheckAllBreak() || CheckCatDead())
            Type = 0;

        else if (CheckMouseAllDead())
            Type = 1;

        else if (CheckEndTimer())
            Type = 2;

        if (Type == -1)
        {
            Debug.LogWarning("에러발생");
            Type = 0;
        }

        return Type;
    }



    // 일정 게이지 이하일 때 
    bool CheckAllBreak()
    {
        if (CatPhotonPlayer == null)
        {
            Debug.LogWarning("에러");
            return false;
        }

        //float CatGradeScore = (float)CatPhotonPlayer.CustomProperties["CatScore"];
        float CatGradeScore = (float)PhotonNetwork.player.CustomProperties["StoreScore"];


        float CatGradePersent;

        if (CatGradeScore <= 0)
            CatGradePersent = 0;

        else
            CatGradePersent = CatGradeScore / PhotonManager.GetInstance().MaxCatScore * 100;


        if (CatGradePersent <= GameBreakCondition)
            return true;

        else
            return false;

    }

    // 모든 쥐가죽었는지 판단
    bool CheckMouseAllDead()
    {
        bool isFinish = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

            if (PlayerType == "Mouse")
            {
                isFinish = false;
            }
        }

        return isFinish;
    }

    bool CheckEndTimer()
    {
        if (playTimerNumber <= 0)
            return true;
        else
            return false;
    }


    bool isLeftCatPlayer = false;

    bool CheckCatDead()
    {
        return isLeftCatPlayer;
    }



    IEnumerator WaitStartImage()
    {
        UIManager.GetInstance().gameStartCountPanelScript.Start.SetActive(true);

        yield return new WaitForSeconds(StartImage_WaitTime);

        UIManager.GetInstance().gameStartCountPanelScript.Start.SetActive(false);
        UIManager.GetInstance().gameStartCountPanelScript.GameStartCountPanel.SetActive(false);

        yield break;
    }


    [PunRPC]
    void RPCTutoPlayingGame()
    {
        CurrentPlayer.GetComponent<PlayerState>().isCanActive = true;

        GameObject go = GameObject.Find("TutorialGuide");
        if (go == null) return;

        TutorialGuide tg = go.GetComponent<TutorialGuide>();
        if (tg == null) return;


        tg.playerObject = CurrentPlayer;
        SetTutorialGuide(tg);

        tg.StartTutorial();
    }

    public void SetTutorialGuide(TutorialGuide tg)
    {
        // 1. 스킬에 필요한 데이터와 플레이어의 데이터를 연결시킨다.

        // 종류찾기.
        int tutorialElementCount = tg.mouseTutorialElements.Length;
        for (int i = 0; i < tutorialElementCount; i++)
        {
            TutorialElement tutorialElement = tg.mouseTutorialElements[i];

            int tutorialCdtCount = tutorialElement.tutorialConditions.Length;
            for (int tcc = 0; tcc < tutorialCdtCount; tcc++)
            {
                TutorialCondition tutorialCondition = tutorialElement.tutorialConditions[tcc];

                //액션 셋팅
                SetCondition(tutorialCondition);
            }

            int tutorialAcCondition = tutorialElement.tutorialActions.Length;
            for (int tac = 0; tac < tutorialAcCondition; tac++)
            {
                TutorialAction tutorialAction = tutorialElement.tutorialActions[tac];

                //조건 셋팅
                SetAction(tutorialAction);

            }
        }
    }

    public void SetCondition(TutorialCondition tutorialCondition)
    {
        if (tutorialCondition.activeType == TutorialCondition.EnumActive.SPEEDRUN)
        {
            NewSpeedRun newSpeedRun = CurrentPlayer.GetComponent<NewSpeedRun>();
            newSpeedRun.EventUseCtnSkill += tutorialCondition.IncreateTime;

            newSpeedRun.EventExitCtnSkill += tutorialCondition.ResetMount;
        }
        
    }

    public void SetAction(TutorialAction tutorialAction)
    {

    }
}
