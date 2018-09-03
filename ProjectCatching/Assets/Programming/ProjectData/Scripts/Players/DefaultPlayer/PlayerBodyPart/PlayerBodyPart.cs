using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPart : MonoBehaviour {

    public GameObject PlayerLeftHand { get; set; }
    public GameObject PlayerRightHand { get; set; }
    public GameObject PlayerHead { get; set; }

    public GameObject HandRope;
    public GameObject LegRope;

    public GameObject UpHeadPosition { get; set; }

    private void Awake()
    {
        Transform UpHeadTr = gameObject.transform.Find("UpHeadPosition");
        if (UpHeadTr != null) UpHeadPosition = UpHeadTr.gameObject;

        Transform[] tr = GetComponentsInChildren<Transform>();

        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i].gameObject.name == "WeaponLHand")
            {
                PlayerLeftHand = tr[i].gameObject;

            }


            else if (tr[i].gameObject.name == "WeaponRHand")
            {
                PlayerRightHand = tr[i].gameObject;

            }
        }
    }
}
