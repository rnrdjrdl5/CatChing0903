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
        nowAction.MessageText = EditorGUILayout.TextArea(nowAction.MessageText);
    }
}
