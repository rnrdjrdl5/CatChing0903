using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TimeBar
{

    private bool isCount;

    public bool GetisCount() { return isCount; }
    public void SetisCount(bool C) { isCount = C; }

    [Header(" - 상호작용 시간 프리팹 등록")]
    [Tooltip(" - 상호작용 시간을 나타낼 UI입니다.")]
    public GameObject TimeBarPanelPrefab;
    private GameObject TimeBarPanelObject;

    // 인게임 캠버스.
    private GameObject InGameCanvas;

    // 타임바 
    private GameObject NowTimeBar;

    // 타임바 이미지 컴포넌트
    private Image NowTimeBarImage;



    // 현재 타임바 시간
    private float NowTime;

    public float GetNowTime() { return NowTime; }
    public void SetNowTime(float NT) { NowTime = NT; }


    // 최대 타임 바 시간

    private float MaxTime;

    public float GetMaxTime() { return MaxTime; }
    public void SetMaxTime(float MT) { MaxTime = MT; }

    




}
