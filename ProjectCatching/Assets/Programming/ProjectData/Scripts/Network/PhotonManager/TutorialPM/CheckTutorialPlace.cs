using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어가 위치에 들어왔는지 체크함.
public class CheckTutorialPlace : MonoBehaviour {


    public bool isClear;

    private void Awake()
    {
        isClear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isClear = true;
        }
    }
}
