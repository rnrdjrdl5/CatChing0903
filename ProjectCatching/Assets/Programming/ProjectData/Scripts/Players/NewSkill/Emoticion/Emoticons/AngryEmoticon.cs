using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryEmoticon : ShowEmoticon {

    protected override void Awake()
    {
        base.Awake();

        EmoticonType = PoolingManager.EnumEmoticon.ANGRY;
    }



    public override void UseSkill()
    {
        base.UseSkill();
        photonView.RPC("RPCAngryEmoticon", PhotonTargets.All);
    }

    [PunRPC]
    void RPCAngryEmoticon()
    {

        playerState.SetisUseEmoticon(true);

        EmoticonObject = PoolingManager.GetInstance().CreateEmoticon(EmoticonType);
        EmoticonObject.transform.SetParent(EmotionPosition.transform);
        EmoticonObject.transform.localPosition = Vector3.zero;
        EmoticonObject.transform.localScale = Vector3.zero;

        objectFadeWait.TargetObject = EmoticonObject;

        StartCoroutine(objectFadeWait.CreateFadeWait());
    }
}
