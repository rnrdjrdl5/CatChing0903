using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeWait : Behaviour{

    public GameObject TargetObject { get; set; }

    public float EmoticonFadeInOut { get; set; }
    public float EmoticonWait { get; set; }

    public delegate void FinishDele();

    public FinishDele FinishEvent { get; set; }

    private enum FadeType { FADEIN, WAIT, FADEOUT }

    public IEnumerator CreateFadeWait()
    {
        float NowTime = 0.0f;

        FadeType fadeType = FadeType.FADEIN;

        while (true)
        {
           
            if (fadeType == FadeType.FADEIN)
            {
                Debug.Log("!");
                TargetObject.transform.localScale = Vector3.one * (NowTime / EmoticonFadeInOut);
                if (NowTime >= EmoticonFadeInOut)
                {
                    fadeType = FadeType.WAIT;
                    NowTime = 0.0f;
                }
            }

            else if (fadeType == FadeType.WAIT)
            {
                if (NowTime >= EmoticonWait)
                {
                    fadeType = FadeType.FADEOUT;
                    NowTime = 0.0f;
                }
            }

            else if (fadeType == FadeType.FADEOUT)
            {
                Debug.Log("!###");
                TargetObject.transform.localScale = Vector3.one - Vector3.one * (NowTime / EmoticonFadeInOut);

                if (NowTime >= EmoticonFadeInOut)
                {
                    NowTime = 0.0f;
                    FinishEvent();
                    yield break;
                }
            }


            NowTime += Time.deltaTime;
            yield return null;
        }
    }
}
