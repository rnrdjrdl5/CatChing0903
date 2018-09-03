using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어 포지션, 
/// 애니메이션 인 경우 어느 위치에서만 상호작용이 가능해야함.
/// 이 스크립트로 위치에 있는지 판단함.
/// </summary>

public class PlayerPositionScript : MonoBehaviour {




    public bool IsCanInter { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (!other.gameObject.GetPhotonView().isMine)
            return;

        IsCanInter = true;
        Debug.Log("1");
            

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag != "Player")
            return;

        if (!other.gameObject.GetPhotonView().isMine)
            return;

        IsCanInter = false;
        Debug.Log("2");

    }
}
