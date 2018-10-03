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

    public void StartTutorial()
    {
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
