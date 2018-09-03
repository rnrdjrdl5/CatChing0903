using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanelScript{

    public GameObject LobbyPanel { get; set; }

    public GameObject EnterImage { get; set; }
    public GameObject OptionImage { get; set; }
    public GameObject ExitImage { get; set; }
    public GameObject LogoImage { get; set; }

    private UIEffect EnterImageEffect;
    private UIEffect OptionImageEffect;
    private UIEffect ExitImageEffect;
    private UIEffect LogoImageEffect;

    private LobbyUIManager lobbyUIManager;

    public void InitData()
    {

        lobbyUIManager = LobbyUIManager.GetInstance();

        GameObject UICanvas = lobbyUIManager.UICanvas;

        

        LobbyPanel = UICanvas.transform.Find("LobbyPanel").gameObject;

        EnterImage = LobbyPanel.transform.Find("EnterImage").gameObject;
        OptionImage = LobbyPanel.transform.Find("OptionImage").gameObject;
        ExitImage = LobbyPanel.transform.Find("ExitImage").gameObject;
        LogoImage = LobbyPanel.transform.Find("LogoImage").gameObject;

        EnterImageEffect = new UIEffect();
        OptionImageEffect = new UIEffect();
        ExitImageEffect = new UIEffect();
        LogoImageEffect = new UIEffect();
    }

    public void SetActive(bool isActive)
    {

        LobbyPanel.SetActive(isActive);

        EnterImage.SetActive(isActive);
        OptionImage.SetActive(isActive);
        ExitImage.SetActive(isActive);
        LogoImage.SetActive(isActive);
    }

    public void FadeOutEffect()
    {
        EnterImageEffect.AddFadeEffectNode(EnterImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        lobbyUIManager.UpdateEvent += EnterImageEffect.EffectEventLobby;

        OptionImageEffect.AddFadeEffectNode(OptionImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        lobbyUIManager.UpdateEvent += OptionImageEffect.EffectEventLobby;

        ExitImageEffect.AddFadeEffectNode(ExitImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        lobbyUIManager.UpdateEvent += ExitImageEffect.EffectEventLobby;

        LogoImageEffect.AddFadeEffectNode(LogoImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        lobbyUIManager.UpdateEvent += LogoImageEffect.EffectEventLobby;
    }

    public void FadeInEffect()
    {
        EnterImageEffect.AddFadeEffectNode(EnterImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        lobbyUIManager.UpdateEvent += EnterImageEffect.EffectEventLobby;

        OptionImageEffect.AddFadeEffectNode(OptionImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        lobbyUIManager.UpdateEvent += OptionImageEffect.EffectEventLobby;

        ExitImageEffect.AddFadeEffectNode(ExitImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        lobbyUIManager.UpdateEvent += ExitImageEffect.EffectEventLobby;

        LogoImageEffect.AddFadeEffectNode(LogoImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        lobbyUIManager.UpdateEvent += LogoImageEffect.EffectEventLobby;
    }



}
