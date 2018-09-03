using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLight : DefaultNewSkill
{
    // 불 끌때 각종 옵션입니다.
    public TurnOffLightState turnOffLightState;

    // 캔버스입니다. 불 끄는 ui가 들어갈 위치.
    private GameObject InGameCanvas;

    protected override void Awake()
    {
        base.Awake();

        InGameCanvas = GameObject.Find("InGameCanvas").gameObject;

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);
    }

    public override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)) &&
            playerState.isCanActive == true)

        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {

        // 쥐를 제외한 모든 플레이어에게 사용
        for (int i = 0; i < PhotonManager.GetInstance().AllPlayers.Count; i++)
        {
            if(!PhotonManager.GetInstance().AllPlayers[i].name.Contains("Cat"))
                PhotonManager.GetInstance().AllPlayers[i].GetComponent<PlayerState>().AddDebuffState(DefaultPlayerSkillDebuff.EnumSkillDebuff.GROGGY, 0.1f);
        }


         
        photonView.RPC("RPCTurnOffLight", PhotonTargets.All);
        Debug.Log("스킬시작");
    }

    // RPC입니다.
    [PunRPC]
    void RPCTurnOffLight()
    {
        CreateTurnOffPanel();
    }

    public void CreateTurnOffPanel()
    {


        TurnOffPanelScript tops = UIManager.GetInstance().turnOffPanelScript;

        GameObject turnOffPanelObject = tops.TurnOffPanel;

        turnOffPanelObject.SetActive(true);


        if (photonView.isMine)
        {
            tops.BlackBackGround.SetActive(false);
        }

        TurnOffPanel turnOffPanel =  turnOffPanelObject.GetComponent<TurnOffPanel>();
        if (turnOffPanel == null)
            return;


        turnOffPanel.SetTurnOffTime(turnOffLightState.TurnOffTime);



        turnOffPanel.SetisUseTurnOff(true);

        turnOffPanel.StartCutScene();


        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.EFFECT_CAT_TURNOFF);


    }
}
