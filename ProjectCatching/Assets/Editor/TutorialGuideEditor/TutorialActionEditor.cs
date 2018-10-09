 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public partial class TutorialGuideEditor
{
    void SettingActionInspector(TutorialAction nowAction)
    {
        switch (nowAction.tutorialActionType)
        {
            case TutorialAction.EnumTutorialAction.MESSAGE:
                MessageInspector(nowAction);
                break;
            case TutorialAction.EnumTutorialAction.WAIT:
                WaitInspector(nowAction);
                break;
            case TutorialAction.EnumTutorialAction.EMOTION:
                EmotionInspector(nowAction);
                break;
            case TutorialAction.EnumTutorialAction.DRAW_IMAGE:
                DrawImage(nowAction);
                break;
        }
    }

    void MessageInspector(TutorialAction nowAction)
    {
        // 1. 텍스트 타입 설정
        nowAction.messageSizeType = (TutorialAction.EnumMessageSize)EditorGUILayout.EnumPopup
        ("텍스트 창 타입",
        nowAction.messageSizeType);

        // 2. 텍스트 설정
        EditorGUILayout.LabelField("텍스트 내용");
        nowAction.messageText = EditorGUILayout.TextArea(nowAction.messageText);
    }

    void WaitInspector(TutorialAction nowAction)
    {
        nowAction.waitTime = EditorGUILayout.FloatField("대기시간", nowAction.waitTime);
    }

    void EmotionInspector(TutorialAction nowAction)
    {
        //1. 대상
        nowAction.tutorialAIType = (TutorialAction.EnumTutorialAI)EditorGUILayout.EnumPopup
        ("이모티콘 AI",
        nowAction.tutorialAIType);

        // 1-1 대상을 토대로 설정
        int tutorialAIType = (int)nowAction.tutorialAIType;

        // 1-2 대상의 오브젝트 선정
        if (tutorialAIType <= tutorialAI.AI.Length - 1)
        {
            nowAction.aIObject = tutorialAI.AI[tutorialAIType];
        }

        //2. 어떤이모티콘
            nowAction.emoticonType = (TutorialAction.EnumEmoticon)EditorGUILayout.EnumPopup
        ("이모티콘 종류",
        nowAction.emoticonType);
    }

    void DrawImage(TutorialAction nowAction)
    {
        // 1. 어떤 이미지를 보여줄건지 선택하고.
        nowAction.imageObject = EditorGUILayout.ObjectField("이미지 선택",
            nowAction.imageObject, typeof(GameObject), false) as GameObject;


        // 2. 어디 좌표에 이미지를 보여줄건지 선택한다.
        nowAction.imageXPosition = EditorGUILayout.FloatField("X축 선택",
            nowAction.imageXPosition);
        nowAction.imageYPosition = EditorGUILayout.FloatField("Y축 선택",
            nowAction.imageYPosition);

        



    }
}
