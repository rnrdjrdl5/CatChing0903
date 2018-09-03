using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunDebuff : PlayerDefaultDebuff
{

    PlayerBodyPart playerBodyPart;

    protected override void Awake()
    {
        base.Awake();
        playerBodyPart = GetComponent<PlayerBodyPart>();
    }


    protected override void Start()
    {
        base.Start();
        CreateDebuffEffect("strun_main_01", playerBodyPart.UpHeadPosition.transform);
    }

   

    protected override void Update()
    {
        base.Update();
        Debug.Log("스턴중!!");
        Debug.Log("Time : " + NowDebuffTime);
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("StunOnOff", false);
        Debug.Log("***스턴끝 *****");

        //if (DebuffEffect != null)
        //{
        //    PoolingManager.GetInstance().PushObject(DebuffEffect);
        //}
        PopDebuffEffect();
    }

    
    /*public void CreateEffect()
    {
        DebuffEffect = PoolingManager.GetInstance().PopObject("strun_main_01");

        DebuffEffect.transform.position =
            playerBodyPart.PlayerHead.transform.position - transform.up * 0.5f;

        DebuffEffect.transform.SetParent(playerBodyPart.PlayerHead.transform);

    }*/
    

}
