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

            // 1. 액션 수행
            float waitTime = tutorialActions[i].UseAction();
            float tempTime = 0.0f;
            // 대기시간이 지날 떄 까지 대기
            while (true)
            {
                if (tempTime >= waitTime) break;
                else tempTime += Time.deltaTime;
            }
        }
    }
}
