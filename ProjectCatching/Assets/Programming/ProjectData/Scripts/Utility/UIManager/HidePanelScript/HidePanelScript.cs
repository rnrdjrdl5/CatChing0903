using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanelScript {

    UIEffect uIEffect;

    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas()
    {
        InGameCanvas = UIManager.GetInstance().InGameCanvas;
    }

    public GameObject HidePanel { get; set; }
    public void InitHidePanel()
    {
        HidePanel = InGameCanvas.transform.Find("HidePanel").gameObject;
    }

    public GameObject BackgroundImage { get; set; }
    public void InitBackgroundImage()
    {
        BackgroundImage = HidePanel.transform.Find("BackgroundImage").gameObject;
    }

    public GameObject HideWordPanel { get; set; }
    public void InitHideWordPanel()
    {
        HideWordPanel = HidePanel.transform.Find("HideWordPanel").gameObject;
    }

    public GameObject HideMouseImage { get; set; }
    public void InitHideMouseImage()
    {
        HideMouseImage = HidePanel.transform.Find("HideMouseImage").gameObject;
    }



    public GameObject HideEffectPanel { get; set; }
    public void InitHideEffectPanel() { HideEffectPanel = InGameCanvas.transform.Find("HideEffectPanel").gameObject; }

    public void InitData()
    {
        InitInGameCanvas();

        InitHidePanel();
        InitBackgroundImage();
        InitHideWordPanel();
        InitHideMouseImage();

        InitHideEffectPanel();

        uIEffect = new UIEffect();

    }

    public void SetCutSceneEffect()
    {

        uIEffect.AddMoveEffectNode(HidePanel,
            Vector2.right * -HidePanel.GetComponent<RectTransform>().rect.width,
            Vector2.zero, 0.5f);

        uIEffect.AddWaitEffectNode(2.0f);

        uIEffect.AddMoveEffectNode(HidePanel,
            Vector2.zero,
            Vector2.right * -HidePanel.GetComponent<RectTransform>().rect.width,
            0.5f);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;
    }
}
