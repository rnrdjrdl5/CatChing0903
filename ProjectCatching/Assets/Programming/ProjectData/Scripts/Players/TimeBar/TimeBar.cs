using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TimeBar : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // 1. 인스턴트화 시 오브젝트, 프리팹 생성.
        isCount = false;
        InGameCanvas = GameObject.Find("InGameCanvas");

    }
	
	// Update is called once per frame
	void Update () {

        if (isCount)
        {
            // 1. update 갱신 + 파괴 인식
            UpdateTime();
        }

        if (isCount) { 
            // 2. fillamount 갱신
            UpdateTimeBarImage();
        }


    }
}
