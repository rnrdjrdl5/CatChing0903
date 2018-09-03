using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerManager : MonoBehaviour {

    

    // Use this for initialization
    private void Awake()
    {
        SetFallowCamera();


    }
    private void Start()
    {

        // player들 나오면 PhotonManager에 추가한다. 
        PhotonManager photonManager = PhotonManager.GetInstance();

        if(photonManager != null)
            photonManager.AllPlayers.Add(gameObject);

        SetSpringArm();




    }

    private void Update()
    {


    }
}
