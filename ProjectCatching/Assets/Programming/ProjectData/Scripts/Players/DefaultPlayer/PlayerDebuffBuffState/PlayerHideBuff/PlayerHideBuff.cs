using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideBuff : PlayerDefaultDebuff
{

    protected override void ExitDebuff()
    {

        SkinnedMeshRenderer[] smr = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < smr.Length; i++)
        {
            smr[i].enabled = true;
        }
    }

}
