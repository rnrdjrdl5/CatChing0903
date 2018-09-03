using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotMoveDebuff : PlayerDefaultDebuff
{

    PlayerBodyPart playerBodyPart;

    protected override void Awake()
    {
        base.Awake();
        playerBodyPart = GetComponent<PlayerBodyPart>();
    }

    protected override void Start()
    {
        CreateDebuffEffect("strun_main_01", playerBodyPart.UpHeadPosition.transform);
    }

    protected override void Update()
    {
        base.Update();
        // 속박 하기.
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("isNotMove", false);
        PopDebuffEffect();
    }

}
