using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialElement{


    // 조건
    public TutorialCondition[] tutorialConditions;
    public int maxTutorialCondition;

    // 액션
    public TutorialAction[] tutorialActions;
    public int maxTutorialAction;


    // 튜토리얼 시작
    public bool CheckCondition()
    {
        int nowCount = tutorialConditions.Length;

        for (int i = 0; i < nowCount; i++)
        {
            if (tutorialConditions[i].CheckCondition() == false) return false;
        }


        return true;
    }


    public void UseAction()
    {

        // 카운트만큼 action 사용
        int nowCount = tutorialActions.Length;

        for (int i = 0; i < nowCount; i++)
        {

            tutorialActions[i].UseAction();
        }
    }
}
