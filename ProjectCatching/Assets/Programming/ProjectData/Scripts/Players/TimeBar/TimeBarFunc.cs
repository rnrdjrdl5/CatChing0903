using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TimeBar {

    void UpdateTime()
    {
        if (MaxTime <= NowTime + Time.deltaTime)
        {
            NowTime = MaxTime;
            DestroyObjects();
        }

        else
        {
            NowTime += Time.deltaTime;
        }

    }

    void UpdateTimeBarImage()
    {
        NowTimeBarImage.fillAmount =
            NowTime / MaxTime;
    }


    public void DestroyObjects()
    {
        Destroy(TimeBarPanelObject);

        TimeBarPanelObject = null;

        isCount = false;

        NowTimeBar = null;
        NowTimeBarImage = null;

        NowTime = 0.0f;

        MaxTime = 0.0f;

    }

}
