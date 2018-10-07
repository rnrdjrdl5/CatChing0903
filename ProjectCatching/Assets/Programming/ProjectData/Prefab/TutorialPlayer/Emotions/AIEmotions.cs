using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEmotions : MonoBehaviour {

    const float EmoticonFadeInOut = 0.5f;
    const float EmotionWait = 1.5f;


    
    public GameObject emotionPosition { get; set; }
    public GameObject emotionObject{ get; set; }

    public ObjectFadeWait objectFadeWait { get; set; }
    public void FadeFinish()
    {
        PoolingManager.GetInstance().PushObject(emotionObject);
    }



    public enum EnumEmotion { HI };     // Acion이나 ActionEditor와 동일하게 맞출 것

    private void Awake()
    {
        emotionPosition = transform.Find("EmoticonPosition").gameObject;

        objectFadeWait = new ObjectFadeWait
        {
            EmoticonFadeInOut = EmoticonFadeInOut,
            EmoticonWait = EmotionWait,
            FinishEvent = FadeFinish
        };
    }

    public void UseEmotion(int emotionType)
    {
        
        emotionObject = PoolingManager.GetInstance().CreateEmoticon((PoolingManager.EnumEmoticon)emotionType);
        emotionObject.transform.SetParent(emotionPosition.transform);
        emotionObject.transform.localPosition = Vector3.zero;
        emotionObject.transform.localScale = Vector3.zero;

        objectFadeWait.TargetObject = emotionObject;

        StartCoroutine(objectFadeWait.CreateFadeWait());
    }

}
