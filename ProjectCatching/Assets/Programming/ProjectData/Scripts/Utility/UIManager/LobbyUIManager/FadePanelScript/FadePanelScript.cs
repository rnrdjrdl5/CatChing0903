using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelScript : MonoBehaviour {

    private LobbyUIManager lobbyUIManager;
    private UIEffect FadeImageEffect;


    public GameObject FadePanel { get; set; }
    public GameObject FadeImage { get; set; }

    public void InitFadePanel() { FadePanel = lobbyUIManager.UICanvas.transform.Find("FadePanel").gameObject; }
    public void InitFadeImage() { FadeImage = FadePanel.transform.Find("FadeImage").gameObject; }
    

    public void InitData()
    {
        lobbyUIManager = LobbyUIManager.GetInstance();

        InitFadePanel();

        InitFadeImage();
        FadeImageEffect = new UIEffect();

    }

    public void SetActive(bool isActive)
    {

        FadeImage.SetActive(isActive);
        FadePanel.SetActive(isActive);
    }

    public void FadeInEffect()
    {
        Debug.Log("Fdea");
        FadeImageEffect.AddFadeEffectNode(FadeImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.IN);

        lobbyUIManager.UpdateEvent += FadeImageEffect.EffectEventLobby;
    }

    public void FadeOutEffect()
    {
        Debug.Log("Fdea");
        FadeImageEffect.AddFadeEffectNode(FadeImage, lobbyUIManager.UIFadeTime, UIEffectNode.EnumFade.OUT);

        lobbyUIManager.UpdateEvent += FadeImageEffect.EffectEventLobby;
    }
}
