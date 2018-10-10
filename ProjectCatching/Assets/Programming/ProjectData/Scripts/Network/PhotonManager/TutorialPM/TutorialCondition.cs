using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialCondition{

    // 최대거리
    const float INFINIFY_DISTANCE = 10000f;


    // 기본정보
    public GameObject playerObject;


    // 플레이어 도착위치 타입 지정
    public enum EnumTutorialPlace
    { ONE, TWO, THREE, FOUR, FIVE };
    public EnumTutorialPlace tutorialPlaceType;


    // 조건
    public enum EnumTutorialCondition {
        PLACE, ALWAYS, ONMOUSE , USEACTIVE, USEINTERACTIVE
    };
    public EnumTutorialCondition tutorialConditionType;

    // 이동 위치
    public GameObject tutorialPlace;
    public CheckTutorialPlace checkTutorialPlace;       // 에디터에서 설정해서 받아옴.

    // 레이캐스트 유틸리티
    public PointToLocation pointToLocation;

    // 마우스 올려놓기 대상
    public enum EnumOnMouse { TOMATO};
    public EnumOnMouse onMouseType;

    // 액티브 사용 시
    public enum EnumActive { SPEEDRUN};
    public EnumActive activeType;
    public float activeMount;

    // 스킬 사용시간
    public float playerActiveMount;
    public void IncreateTime()
    {
        playerActiveMount += Time.deltaTime;
    }
    public void IncreateMount()
    {
        playerActiveMount++;
    }

    public void ResetMount()
    {
        playerActiveMount = 0;
    }


    // 상호작용 설정
    public InteractiveState.EnumInteractiveObject interactiveObjectType;
    public int interactiveMount;

    public int[] intersMount;           // 각 상호작용 마다 갯수 파악 용
    public int MAX_INTERS = 100;        // 상호작용의 최대개수
    public void IncreaseInter(int data)
    {
        intersMount[data]++;
    }


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

        if (tutorialConditionType == EnumTutorialCondition.USEACTIVE)
        {

            // 해당스킬사용했는지?
            if (activeType == EnumActive.SPEEDRUN)
            {
                if (activeMount <= playerActiveMount)
                    return true;
            }

        }

        if (tutorialConditionType == EnumTutorialCondition.USEINTERACTIVE)
        {
            if (intersMount[(int)interactiveObjectType] >= interactiveMount)
                return true;
        }


            return false;
    }
}
