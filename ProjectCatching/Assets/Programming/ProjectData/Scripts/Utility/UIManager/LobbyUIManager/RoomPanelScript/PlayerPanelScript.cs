using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelScript {

    private LobbyUIManager lobbyUIManager;

    public GameObject PlayerPanel { get; set; }

    public GameObject MeImage { get; set; }
    public GameObject ChairImage { get; set; }
    public GameObject CatImage { get; set; }
    public GameObject MasterImage { get; set; }

    private UIEffect MeImageEffect;
    private UIEffect ChairImageEffect;
    private UIEffect CatImageEffect;
    private UIEffect MasterImageEffect;

    public void InitData(int PlayerPanelNumber)
    {

        lobbyUIManager = LobbyUIManager.GetInstance();

        PlayerPanel = lobbyUIManager.roomPanelScript.RoomPanel.transform.Find("PlayerPanel" + (PlayerPanelNumber + 1)).gameObject;

        MeImage = PlayerPanel.transform.Find("MeImage").gameObject;
        ChairImage = PlayerPanel.transform.Find("ChairImage").gameObject;
        CatImage = PlayerPanel.transform.Find("CatImage").gameObject;
        MasterImage = PlayerPanel.transform.Find("MasterImage").gameObject;

        MeImageEffect = new UIEffect();
        ChairImageEffect = new UIEffect();
        CatImageEffect = new UIEffect();
        MasterImageEffect = new UIEffect();
    }

    public void SetActive(bool isActive)
    {

        MeImage.SetActive(isActive);
        ChairImage.SetActive(isActive);
        CatImage.SetActive(isActive);
        MasterImage.SetActive(isActive);
    }

    private void FadeInEffect()
    {

        MeImageEffect.AddFadeEffectNode(MeImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        ChairImageEffect.AddFadeEffectNode(ChairImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        CatImageEffect.AddFadeEffectNode(CatImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);
        MasterImageEffect.AddFadeEffectNode(MasterImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);

        lobbyUIManager.UpdateEvent += MeImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += ChairImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += CatImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += MasterImageEffect.EffectEventLobby;

    }


    private void FadeOutEffect()
    {

        MeImageEffect.AddFadeEffectNode(MeImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        ChairImageEffect.AddFadeEffectNode(ChairImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        CatImageEffect.AddFadeEffectNode(CatImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        MasterImageEffect.AddFadeEffectNode(MasterImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);

        lobbyUIManager.UpdateEvent += MeImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += ChairImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += CatImageEffect.EffectEventLobby;
        lobbyUIManager.UpdateEvent += MasterImageEffect.EffectEventLobby;

    }

    public void CallFadeInEffect()
    {
        FadeInEffect();
    }

    public void CallFadeOutEffect()
    {
        FadeOutEffect();
    }

}
