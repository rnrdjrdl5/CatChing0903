using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEmoticon : DefaultNewSkill
{

    public float EmoticonFadeInOut = 0.5f;
    public float EmotionWait = 1.5f;

    protected PoolingManager.EnumEmoticon EmoticonType;

    protected ObjectFadeWait objectFadeWait;

    protected  GameObject EmotionPosition;

    protected GameObject EmoticonObject;



    protected override void Awake()
    {
        
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);
        objectFadeWait = new ObjectFadeWait
        {
            EmoticonFadeInOut = EmoticonFadeInOut,
            EmoticonWait = EmotionWait,
            FinishEvent = FadeFinish
        };

        EmotionPosition = transform.Find("EmoticonPosition").gameObject;
    }

    public override bool CheckState()
    {
        if (!playerState.GetisUseEmoticon())
            return true;

        else
            return false;
        
    }

    public override void UseSkill()
    {
        //photonView.RPC("RPCCreateEmoticon", PhotonTargets.All);
    }

    public void FadeFinish()
    {

        playerState.SetisUseEmoticon(false);
        PoolingManager.GetInstance().PushObject(EmoticonObject);
    }

    public int EmoticonAnimation()
    {

        int AniType = 0;
        switch (EmoticonType)
        {
            case PoolingManager.EnumEmoticon.SAD:
                AniType = 1;
                break;
        }
        if (AniType == 0)
            Debug.Log("이모션 애니메이션 오류 ");

        return AniType;
    }

    public bool IsCanUseEmoticonAnim()
    {
        if (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))
            return true;

        else
            return false;
    }

    [PunRPC]
    void RPCCreateEmoticon()
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
