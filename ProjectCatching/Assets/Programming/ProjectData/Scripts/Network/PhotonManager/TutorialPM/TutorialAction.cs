using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialAction{

    // 액션타입.
    public enum EnumTutorialAction { MESSAGE , WAIT, DEBUG}
    public EnumTutorialAction tutorialActionType;

    // 텍스트용
    public string MessageText;
    public enum EnumMessageSize { SMALL, NORMAL, BIG };
    public EnumMessageSize messageSizeType;


    public void UseAction()
    {

        switch (tutorialActionType)
        {
            case EnumTutorialAction.DEBUG:
                Debug.Log("DebugLog");
                break;

        }
    }
}
