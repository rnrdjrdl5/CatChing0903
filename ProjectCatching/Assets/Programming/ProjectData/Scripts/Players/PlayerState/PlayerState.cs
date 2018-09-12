using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState : MonoBehaviour {


    public float ShakeTime = 0.5f;
    public float ShakeTick = 0.1f;
    public float ShakePower = 0.3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        Transform[] tr = gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i].name == "PlayerFrontCameraPosition")
            {
                PlayerFrontCameraPosition = tr[i].gameObject;
                break;
            }
        }


    }

    private void Start()
    {
        PhotonView photonView = gameObject.GetPhotonView();

        if (photonView.isMine)
            return;

        Transform[] tr = GetComponentsInChildren<Transform>();
        for (int i = 0; i < tr.Length; i++)
        {
            if(tr[i].gameObject.name != "RescueCollider")
            tr[i].gameObject.layer = LayerMask.NameToLayer("OtherPlayer");
        }

    }
}
