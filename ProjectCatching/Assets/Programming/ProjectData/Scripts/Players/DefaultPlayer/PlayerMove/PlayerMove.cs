using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove : Photon.PunBehaviour
{

    private void Awake()
    {
        SetAwake();
    }

    // Use this for initialization
    void Start()
    {
        SetStart();
    }


    bool UpMove = false;
    bool RightMove = false;
    // Update is called once per frame
    void Update()
    {

        if (springArmObject != null)
        {

            Vector3 playerForward = gameObject.transform.forward;
            playerForward.y = springArmObject.armCamera.transform.forward.y;
        }


        CheckResetCanKey();

        PlayerMoveAnimation();

        PlayerTransform();


        if (UpMove)
            HSpeed = 1;

        if (RightMove)
            VSpeed = 1;
    }

    

}
