using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaHide : DefaultNewSkill {

    public HideState hideState;

    public UIEffect uIEffect;

    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);

        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isHide", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);

    }

    public override bool CheckState()
    {
        if ( ( playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) || 
               playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)  ) && 

                playerState.isCanActive == true &&

                playerState.GetWeaponType() != PlayerState.WeaponEnum.ROPE)

        {
            return true;
        }

        else
            return false;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        // 콜백으로 뺴서 사용하기?
        UIManager.GetInstance().hidePanelScript.SetCutSceneEffect();

        animator.SetBool("isHide", true);
    }

    void OnHide()
    {

        if (photonView.isMine)
        {

            photonView.RPC("RPCHideBuff", PhotonTargets.All);

            StartCoroutine("HideUIEffect");
        }
    }





    void OffHide()
    {
        animator.SetBool("isHide", false);
    }

    void HideEffect()
    {

        GameObject ninjaHideEffect = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.NINJA_HIDE);

        ninjaHideEffect.transform.position = transform.position;
    }

    [PunRPC]
    void RPCHideBuff()
    {

        if (!photonView.isMine)
        {

            SkinnedMeshRenderer[] smr = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < smr.Length; i++)
            {
                smr[i].enabled = false;
            }
        }

        PlayerHideBuff playerHideBuff = GetComponent<PlayerHideBuff>();

        if (playerHideBuff == null)
            playerHideBuff = gameObject.AddComponent<PlayerHideBuff>();

        playerHideBuff.SetMaxDebuffTime(hideState.HideMaxTime);
        playerHideBuff.SetNowDebuffTime(0.0f);
    }

    IEnumerator HideUIEffect()
    {
        UIManager.GetInstance().hidePanelScript.HideEffectPanel.SetActive(true);

        yield return new WaitForSeconds( hideState.HideMaxTime );

        UIManager.GetInstance().hidePanelScript.HideEffectPanel.SetActive(false);

        yield return null;
    }


}
