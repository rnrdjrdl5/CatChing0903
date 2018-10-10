using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialAction {

    // 기본 정보
    public GameObject tutorialCanvas;
    public GameObject playerObject;

    // 액션타입.
    public enum EnumTutorialAction { MESSAGE, WAIT, DEBUG, EMOTION, DRAW_IMAGE, PRACTICE_SKILL, RESET_PRACTICE , RESCUE}
    public EnumTutorialAction tutorialActionType;

    // 텍스트용
    public string messageText;
    public enum EnumMessageSize { SMALL, NORMAL, BIG };     // 주의점 , MessageImageScript와 다른 열거형  사용, 순서를 맞추자.
    public EnumMessageSize messageSizeType;

    // 메세지 박스
    public GameObject messageObject;
    public MessageImageScript messageImageScript;

    // 대기시간
    public float waitTime;


    // 타겟
    public enum EnumTutorialAI { TOMATO };
    public EnumTutorialAI tutorialAIType;
    public GameObject aIObject;



    // 이모티콘 
    public enum EnumEmoticon { ANGRY, HAPPY, HI, MERONG, SAD };
    public EnumEmoticon emoticonType;


    // 이미지 생성
    public GameObject imageObject;
    public float imageXPosition;
    public float imageYPosition;

    // 스킬 연습
    public enum EnumPracticeSkill { MOUSE_SPREAD, NINJA_HIDE }
    public EnumPracticeSkill PracticeSkillType;


    public float UseAction()
    {
        float returnTime = 0f;

        switch (tutorialActionType)
        {
            case EnumTutorialAction.DEBUG:
                Debug.Log("DebugLog");
                break;

            case EnumTutorialAction.WAIT:
                returnTime = waitTime;
                break;

            case EnumTutorialAction.MESSAGE:
                messageObject.SetActive(true);
                messageImageScript.PrintMessage(messageText, (MessageImageScript.EnumMessageSize)messageSizeType);
                break;

            case EnumTutorialAction.EMOTION:
                UseEmotion();
                break;

            case EnumTutorialAction.DRAW_IMAGE:
                DrawImage();
                break;

            case EnumTutorialAction.PRACTICE_SKILL:
                PracticeSkill();
                break;

            case EnumTutorialAction.RESET_PRACTICE:
                ResetSkill();
                break;

            case EnumTutorialAction.RESCUE:
                Rescue();
                break;

        }

        return returnTime;
    }



    void UseEmotion()
    {
        aIObject.GetComponent<AIEmotions>().UseEmotion((int)emoticonType);
    }

    void DrawImage()
    {
        Vector3 position = new Vector3(imageXPosition, imageYPosition, 0);

        // 생성할 방법이 필요하다. 그러면?
        //GameObject go = Instantiate(imageObject, position, Quaternion.identity);

        //go.transform.parent = tutorialCanvas.transform;
    }

    void PracticeSkill()
    {
        switch (PracticeSkillType)
        {
            case EnumPracticeSkill.MOUSE_SPREAD:
                playerObject.GetComponent<MouseSpread>().coolDown.PracticeCoolDown();
                break;
            case EnumPracticeSkill.NINJA_HIDE:
                playerObject.GetComponent<NinjaHide>().coolDown.PracticeCoolDown();
                break;

        }
    }

    void ResetSkill()
    {
        switch (PracticeSkillType)
        {
            case EnumPracticeSkill.MOUSE_SPREAD:
                playerObject.GetComponent<MouseSpread>().coolDown.ResetCoolDown();
                break;
            case EnumPracticeSkill.NINJA_HIDE:
                playerObject.GetComponent<NinjaHide>().coolDown.ResetCoolDown();
                break;
        }
    }

    void Rescue()
    {
        
    }






}
