using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelScript{

    private LobbyUIManager lobbyUIManager;

    private GameObject TitlePanel { get; set; }
    
    private GameObject AnyKeyImage { get; set; }
    private GameObject CatLogoImage { get; set; }


    private UIEffect FadeTitleEffect { get; set; }

    public void InitData()
    {
        lobbyUIManager = LobbyUIManager.GetInstance();

        GameObject UI = lobbyUIManager.UICanvas;

        TitlePanel = UI.transform.Find("TitlePanel").gameObject;

        AnyKeyImage = TitlePanel.transform.Find("AnyKeyImage").gameObject;
        CatLogoImage = TitlePanel.transform.Find("CatLogoImage").gameObject;

        FadeTitleEffect = new UIEffect();
    }

    public void FadeOutEffect()
    {

        FadeTitleEffect.AddFadeEffectNode(AnyKeyImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        FadeTitleEffect.AddFadeEffectNode(CatLogoImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);
        lobbyUIManager.UpdateEvent += FadeTitleEffect.EffectEventLobby;
    }

    public void SetActive(bool isActive)
    {
        AnyKeyImage.SetActive(false);
        CatLogoImage.SetActive(false);
        TitlePanel.SetActive(false);
    }
}
