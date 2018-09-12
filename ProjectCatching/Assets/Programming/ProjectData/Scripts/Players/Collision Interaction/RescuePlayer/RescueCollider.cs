using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueCollider : MonoBehaviour {

    private void Start()
    {
        GameObject rootObject = transform.root.gameObject;

        PhotonView photonView = rootObject.GetComponent<PhotonView>();

        if (photonView == null) return;

        if (photonView.isMine)
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
