using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerongEmoticon : ShowEmoticon {
    protected override void Awake()
    {
        base.Awake();

        EmoticonType = PoolingManager.EnumEmoticon.MERONG;
    }



    public override void UseSkill()
    {
        base.UseSkill();
        photonView.RPC("RPCMerongEmoticon", PhotonTargets.All);
    }

    [PunRPC]
    void RPCMerongEmoticon()
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
