using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageImageScript : MonoBehaviour {

    private RectTransform rectTransform;
    private CanvasScaler canvasScaler;
    private Image image;

    private Text tutorialMsgText;


    private void Awake()
    {
        canvasScaler = gameObject.transform.root.GetComponent<CanvasScaler>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();


        Transform tempTransform = gameObject.transform.Find("tutorialMsgText");

        if (tempTransform == null) return;
        tutorialMsgText = tempTransform.gameObject.GetComponent<Text>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PrintMessage("Hello~!~", EnumMessageSize.BIG);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintMessage("Nice~!~", EnumMessageSize.NORMAL);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PrintMessage("ToMeet~!~", EnumMessageSize.SMALL);
        }
    }



    public enum EnumMessageSize { SMALL , NORMAL , BIG }

    public float[] messageBoxSize;
    public float sizeOffset;

    public Sprite[] messageBoxSprite;


    public Vector2 msgOffset;

    public void PrintMessage(string s, EnumMessageSize enumMessageSize)
    {

        // 1. 이미지 크기 설정.
        rectTransform.sizeDelta = new Vector2(messageBoxSize[(int)enumMessageSize], rectTransform.sizeDelta.y);

        // 2. 이미지 위치 설정
        transform.localPosition = new Vector3( (canvasScaler.referenceResolution.x  - messageBoxSize[(int)enumMessageSize] - sizeOffset) /2 ,
            transform.localPosition.y, 0);

        // 3. 이미지 설정
        image.sprite = messageBoxSprite[(int)enumMessageSize];


        // 1. 텍스트 설정
        tutorialMsgText.text = s;

        // 2. 텍스트 위치 보정
        tutorialMsgText.gameObject.transform.localPosition = new Vector3(
            (-messageBoxSize[(int)enumMessageSize] + msgOffset.x) / 2, 
            rectTransform.sizeDelta.y / 2 - msgOffset.y,  
            0);
            


    }


    
}
