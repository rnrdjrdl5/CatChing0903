using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialCondition{

    const float INFINIFY_DISTANCE = 10000f;



    // 플레이어 도착위치 타입 지정
    public enum EnumTutorialPlace
    { ONE, TWO, THREE, FOUR, FIVE };
    public EnumTutorialPlace tutorialPlaceType;


    // 조건
    public enum EnumTutorialCondition { PLACE, ALWAYS, ONMOUSE};
    public EnumTutorialCondition tutorialConditionType;

    // 이동 위치
    public GameObject tutorialPlace;
    public CheckTutorialPlace checkTutorialPlace;       // 에디터에서 설정해서 받아옴.

    // 레이캐스트 유틸리티
    public PointToLocation pointToLocation;

    // 마우스 올려놓기 대상
    public enum EnumOnMouse { TOMATO};
    public EnumOnMouse onMouseType;
    

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

        if (tutorialConditionType == EnumTutorialCondition.ONMOUSE)
        {
            string targetLayerName = null;

            // 1. 이름 설정
            if (onMouseType == EnumOnMouse.TOMATO)
                targetLayerName = "OtherPlayer";
            
            // 2. 이름으로 레이 발사 , 성공시 true.
            if (pointToLocation.FindObject(INFINIFY_DISTANCE, targetLayerName, SpringArmObject.GetInstance().armCamera) != null)
                return true;

        }

            return false;
    }
}
