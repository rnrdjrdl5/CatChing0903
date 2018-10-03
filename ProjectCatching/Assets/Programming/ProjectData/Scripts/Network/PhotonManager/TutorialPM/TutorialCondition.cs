using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialCondition{

    // 플레이어 도착위치 타입 지정
    public enum EnumTutorialPlace
    { ONE, TWO, THREE, FOUR, FIVE };
    public EnumTutorialPlace tutorialPlaceType;


    // 조건
    public enum EnumTutorialCondition { PLACE, ALWAYS };
    public EnumTutorialCondition tutorialConditionType;


    // 이동 위치
    public GameObject tutorialPlace;
    public CheckTutorialPlace checkTutorialPlace;       // 에디터에서 설정해서 받아옴.

    public bool CheckCondition()
    {

        if (tutorialConditionType == EnumTutorialCondition.PLACE)
        {
            if (checkTutorialPlace.isClear)
            {
                checkTutorialPlace.isClear = false;

                return true;
            }

        }

        if (tutorialConditionType == EnumTutorialCondition.ALWAYS)
        {
            return true;
        }

            return false;
    }
}
