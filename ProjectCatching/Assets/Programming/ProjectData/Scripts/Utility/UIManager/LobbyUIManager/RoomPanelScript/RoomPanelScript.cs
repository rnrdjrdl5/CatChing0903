using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanelScript{

    

    private LobbyUIManager lobbyUIManager;

    public GameObject RoomPanel { get; set; }

    public GameObject StartImage { get;set;}
    public GameObject HelpImage { get; set; }
    public GameObject ExitImage { get; set; }

    public PlayerPanelScript[] playerPanelScripts;

    private UIEffect StartImageEffect;
    private UIEffect HelpImageEffect;
    private UIEffect ExitImageEffect;

    public void SetActive(bool isActive)
    {
        RoomPanel.SetActive(isActive);

        StartImage.SetActive(isActive);
        HelpImage.SetActive(isActive);
        ExitImage.SetActive(isActive);

        //for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        for(int i = 0; i < LobbyUIManager.MAXPLAYER; i++)
        {
            if(i < PhotonNetwork.playerList.Length) playerPanelScripts[i].SetActive(isActive);
            else playerPanelScripts[i].SetActive(false);
        }

    }

    public void InitData()
    {

        lobbyUIManager = LobbyUIManager.GetInstance();

        RoomPanel = lobbyUIManager.UICanvas.transform.Find("RoomPanel").gameObject;

        StartImage = RoomPanel.transform.Find("StartImage").gameObject;
        HelpImage = RoomPanel.transform.Find("HelpImage").gameObject;
        ExitImage = RoomPanel.transform.Find("ExitImage").gameObject;

        InitPlayerPanelScript();

        StartImageEffect = new UIEffect();
        ExitImageEffect = new UIEffect();
        HelpImageEffect = new UIEffect();

    }

    public void FadeInEffect()
    {
        Debug.Log("Fdea");
        StartImageEffect.AddFadeEffectNode(StartImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        ExitImageEffect.AddFadeEffectNode(ExitImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        HelpImageEffect.AddFadeEffectNode(HelpImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        PlayerPanelUIEffect(UIEffectNode.EnumFade.IN);

        lobbyUIManager.UpdateEvent += StartImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += ExitImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += HelpImageEffect.EffectEventLobby;


    }

    public void FadeOutEffect()
    {
        StartImageEffect.AddFadeEffectNode(StartImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        ExitImageEffect.AddFadeEffectNode(ExitImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        HelpImageEffect.AddFadeEffectNode(HelpImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        PlayerPanelUIEffect(UIEffectNode.EnumFade.OUT);

        lobbyUIManager.UpdateEvent += StartImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += ExitImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += HelpImageEffect.EffectEventLobby;
    }

    private void InitPlayerPanelScript()
    {
        playerPanelScripts = new PlayerPanelScript[LobbyUIManager.MAXPLAYER];

        for (int i = 0; i < LobbyUIManager.MAXPLAYER; i++)
        {
            playerPanelScripts[i] = new PlayerPanelScript();
            playerPanelScripts[i].InitData(i);

        }
    }

    private void PlayerPanelUIEffect(UIEffectNode.EnumFade enumFade)
    {


        for (int i = 0; i < LobbyUIManager.MAXPLAYER; i++)
        {


            if (enumFade == UIEffectNode.EnumFade.IN)  playerPanelScripts[i].CallFadeInEffect();

            else if (enumFade == UIEffectNode.EnumFade.OUT) playerPanelScripts[i].CallFadeOutEffect();
        }
    }




}
