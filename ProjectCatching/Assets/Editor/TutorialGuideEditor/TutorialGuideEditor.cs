using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TutorialGuide))]
public partial class TutorialGuideEditor : Editor {

    TutorialGuide tutorialGuide;
    TutorialPlace tutorialPlace;
    

    private void OnEnable()
    {
        tutorialGuide = (TutorialGuide)target;
        tutorialPlace = tutorialGuide.GetComponent<TutorialPlace>();
    }

    public override void OnInspectorGUI()
    {

        // 임시로 쥐로 설정한다.
        TutorialElement[] tutorialPlayer = tutorialGuide.mouseTutorialElements;


        // 1. 쥐 튜토리얼 수 받기
        DynamicTutorialElement();

        int count = tutorialGuide.maxMouseTutorialCount;

        EditorGUI.indentLevel++;

        for (int i = 0; i < count; i++)
        {

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- " + (i+1) + " 번째 튜토리얼 사항" + " ---");


            // 간단하게 튜토리얼 속성 사용
            TutorialElement nowElement = tutorialPlayer[i];

            // 조건 동적생성
            DynamicTutorialCondition(nowElement);

            // 액션 동적생성
            DynamicTutorialAction(nowElement);



            EditorGUILayout.Space();
            EditorGUILayout.Space();


            // 조건 설정
            int conditionCount = nowElement.maxTutorialCondition;

            for (int dtc = 0; dtc < conditionCount; dtc++)
            {

                // 간단하게 사용하기 위해 정의
                TutorialCondition nowCondition = nowElement.tutorialConditions[dtc];

                // 1. 타입 지정
                nowCondition.tutorialConditionType =
                    (TutorialCondition.EnumTutorialCondition)EditorGUILayout.EnumPopup
                    ("조건 정의",
                    nowCondition.tutorialConditionType);

                // 2. 타입별로 인스펙터 조절
                SettingConditionInspector(nowCondition); 

                EditorGUILayout.Space();
            }



            EditorGUILayout.Space();
            EditorGUILayout.LabelField("-------------------");
            EditorGUILayout.Space();



            int actionCount = nowElement.maxTutorialAction;

            for (int dta = 0; dta < actionCount; dta++)
            {
                // 간단하게 설정하기
                TutorialAction nowAction = nowElement.tutorialActions[dta];


                // 1. 타입 지정
                nowAction.tutorialActionType = 
                    (TutorialAction.EnumTutorialAction)EditorGUILayout.EnumPopup
                    ("액션 정의",
                    nowAction.tutorialActionType);

                // 2. 타입별로 인스펙터 조절
                SettingActionInspector(nowAction);

                EditorGUILayout.Space();
            }
            

        }
        EditorGUI.indentLevel--;
    }


    void DynamicTutorialElement()
    {
        // 동적할당 조건 - 변수가 바뀌거나, 기존에 없는경우.
        int beforeMount = tutorialGuide.maxMouseTutorialCount;

        int nowMount =
            EditorGUILayout.IntField("쥐 최대 튜토리얼 수", tutorialGuide.maxMouseTutorialCount);


        // 값 할당은 0이상인경우. 
        if (nowMount >= 0)
        {
            tutorialGuide.maxMouseTutorialCount = nowMount;


            // 1. 기존 값 백업
            TutorialElement[] tempTutorialElement = tutorialGuide.mouseTutorialElements;

            // 2. 새 값 생성
            tutorialGuide.mouseTutorialElements = new TutorialElement[nowMount];

            // 3. 0이면 null처리
            if (nowMount == 0) tutorialGuide.mouseTutorialElements = null;

            for (int i = 0; i < nowMount; i++)
            {

                // 기존 값이 있으면 기존 값 사용
                if (i < beforeMount)
                    tutorialGuide.mouseTutorialElements[i] = tempTutorialElement[i];

                // 새 값이면 할당
                else
                    tutorialGuide.mouseTutorialElements[i] = new TutorialElement();
            }


        }
    }

    void DynamicTutorialAction(TutorialElement tutorialElement)
    {
        // 동적할당 조건 - 변수가 바뀌거나, 기존에 없는경우.
        int beforeMount = tutorialElement.maxTutorialAction;

        int nowMount =
            EditorGUILayout.IntField("튜토리얼 수행작업", tutorialElement.maxTutorialAction);


        // 값 할당은 0이상인경우. 
        if (nowMount >= 0)
        {
            tutorialElement.maxTutorialAction = nowMount;

            // 값이 커지면 동적할당.
            // 1 이상인경우 동적할당
            // 동적할당 시 기존 값은 전달해야한다.


            // 1. 기존 값 백업
            TutorialAction[] tempTutorialAction = tutorialElement.tutorialActions;

            // 2. 새 값 생성
            tutorialElement.tutorialActions = new TutorialAction[nowMount];

            // 3. 0이면 null처리
            if (nowMount == 0) tutorialElement.tutorialActions = null;

            for (int i = 0; i < nowMount; i++)
            {

                // 기존 값이 있으면 기존 값 사용
                if (i < beforeMount)
                    tutorialElement.tutorialActions[i] = tempTutorialAction[i];

                // 새 값이면 할당
                else
                    tutorialElement.tutorialActions[i] = new TutorialAction();
            }


        }
    }

    void DynamicTutorialCondition(TutorialElement tutorialElement)
    {
        // 동적할당 조건 - 변수가 바뀌거나, 기존에 없는경우.
        int beforeMount = tutorialElement.maxTutorialCondition;

        int nowMount =
            EditorGUILayout.IntField("튜토리얼 조건", tutorialElement.maxTutorialCondition);



            tutorialElement.maxTutorialCondition = nowMount;

        // 값이 커지면 동적할당.
        // 1 이상인경우 동적할당
        // 동적할당 시 기존 값은 전달해야한다.
        if (nowMount >= 0)
        {

            // 1. 기존 값 백업
            TutorialCondition[] tempTutorialCondition = tutorialElement.tutorialConditions;

            // 2. 새 값 생성
            tutorialElement.tutorialConditions = new TutorialCondition[nowMount];

            // 3. 0이면 null처리
            if (nowMount == 0) tutorialElement.tutorialConditions = null;

            for (int i = 0; i < nowMount; i++)
            {

                // 기존 값이 있으면 기존 값 사용
                if (i < beforeMount)
                    tutorialElement.tutorialConditions[i] = tempTutorialCondition[i];

                // 새 값이면 할당
                else
                    tutorialElement.tutorialConditions[i] = new TutorialCondition();
            }


        }
    }
}


