using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuide : MonoBehaviour {

    public TutorialElement[] mouseTutorialElements;
    public int maxMouseTutorialCount;

    public TutorialElement[] catTutorialElements;
    public int maxCatTutorialCount;




    private int nowTutorialCount;

    public enum EunmTutorialPlayer { CAT, MOUSE };

    public EunmTutorialPlayer tutorialPlayerType;


    // 메세지 오브젝트, 스크립트
    public GameObject messageObject;
    MessageImageScript messageImageScript;

    void InitMessageData()
    {
        GameObject tco = GameObject.Find("TextCanvas");

        if (tco == null) return;
        Transform tr = tco.transform.Find("TutorialMsgImage");

        if (tr == null) return;
        messageObject = tr.gameObject;

        messageImageScript = messageObject.GetComponent<MessageImageScript>();
    }


    // 레이캐스트 유틸리티
    private PointToLocation pointToLocation;


    private void Awake()
    {
        InitMessageData();

        pointToLocation = new PointToLocation();
    }


    // 하위 속성들 설정
    void SettingElements()
    {
        int nowCount = mouseTutorialElements.Length;

        for (int i = 0; i < nowCount; i++)
        {

            // 액션 속성들 설정
            int actionCount = mouseTutorialElements[i].tutorialActions.Length;

            for (int mte = 0; mte < actionCount; mte++) {


                mouseTutorialElements[i].tutorialActions[mte].messageImageScript = messageImageScript;
                mouseTutorialElements[i].tutorialActions[mte].messageObject = messageObject;
            }

            // 조건 설정들 설정
            int conditionCount = mouseTutorialElements[i].tutorialConditions.Length;

            for (int mtc = 0; mtc < conditionCount; mtc++)
            {

                mouseTutorialElements[i].tutorialConditions[mtc].pointToLocation = pointToLocation;
            }


            
        }
    }

    public void StartTutorial()
    {
        SettingElements();

        StartCoroutine("TutorialLoop");

        
    }

    IEnumerator TutorialLoop()
    {


        while (true)
        {

            // 1. 컨디션 모두 확인
            if (mouseTutorialElements[nowTutorialCount].CheckCondition())
            {
                //2. 액션 모두 사용
                mouseTutorialElements[nowTutorialCount].UseAction();

                nowTutorialCount++;

                if (nowTutorialCount >= mouseTutorialElements.Length) break;

            }

            yield return null;
            
        }

        yield break;
    }
}
