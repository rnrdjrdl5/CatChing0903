using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroggyDebuff : PlayerDefaultDebuff
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("isGroggy", false);
    }
}
