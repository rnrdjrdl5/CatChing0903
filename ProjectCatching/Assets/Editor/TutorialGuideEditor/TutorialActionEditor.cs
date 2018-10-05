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

        //2. 어떤이모티콘
        nowAction.emoticonType = (TutorialAction.EnumEmoticon)EditorGUILayout.EnumPopup
        ("이모티콘 종류",
        nowAction.emoticonType);
    }
}
