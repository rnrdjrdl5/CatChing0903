using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideDebuff : PlayerDefaultDebuff
{

    protected override void Update()
    {
        base.Update();
        Debug.Log("넘어짐.");
        Debug.Log("Time : " + NowDebuffTime);
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("isSlide", false);
        Debug.Log("넘어짐 해제");
        if (DebuffEffect != null)
        {
            PoolingManager.GetInstance().PushObject(DebuffEffect);
        }
    }
}
