using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageImageScript : MonoBehaviour {

    private Text tutorialMsgText;

    private void Awake()
    {
        Transform tempTransform = gameObject.transform.Find("tutorialMsgText");

        if (tempTransform == null) return;
        tutorialMsgText = tempTransform.gameObject.GetComponent<Text>();
    }

    

    public enum EnumMessageSize { BIG , NORMAL , SMALL}

    public Vector2 BigMessageBox;
    public Vector2 NormalMessageBox;
    public Vector2 SmallMessageBox;

    public void PrintMessage(string s, EnumMessageSize enumMessageSize)
    {

        // 1. 이미지 크기 설정
        SetImageSize(enumMessageSize);

        // 2. 텍스트 설정
        tutorialMsgText.text = s;
        
    }

    private void SetImageSize(EnumMessageSize enumMessageSize)
    {
        switch (enumMessageSize)
        {

            case EnumMessageSize.BIG:
                gameObject.transform.localPosition = new Vector3(BigMessageBox.x, BigMessageBox.y, 0);
                break;

            case EnumMessageSize.NORMAL:
                gameObject.transform.localPosition = new Vector3(NormalMessageBox.x, NormalMessageBox.y, 0);
                break;

            case EnumMessageSize.SMALL:
                gameObject.transform.localPosition = new Vector3(SmallMessageBox.x, SmallMessageBox.y, 0);
                break;
        }
    }

    
}
