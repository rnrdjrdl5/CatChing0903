using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueBarPanelScript{

    public RescuePlayer rescuePlayer;

    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas()
    {
        InGameCanvas = UIManager.GetInstance().InGameCanvas;
    }

    public GameObject RescueBarPanel { get; set; }
    public void InitRescueBarPanel()
    {
        RescueBarPanel = InGameCanvas.transform.Find("RescueBarPanel").gameObject;
    }

    public GameObject RescueGauge { get; set; }
    public void InitRescueGauge()
    {
        RescueGauge = RescueBarPanel.transform.Find("RescueGauge").gameObject;
    }

    public Image RescueGaugeImage { get; set; }
    public void InitRescueGaugeImage()
    {
        RescueGaugeImage = RescueGauge.GetComponent<Image>();
    }

    public void InitData()
    {

        InitInGameCanvas();

        InitRescueBarPanel();

        InitRescueGauge();
        InitRescueGaugeImage();
    }

    public void SetEvent(GameObject gameObject)
    {
        rescuePlayer = gameObject.GetComponent<RescuePlayer>();
        if (rescuePlayer == null)
        {
            Debug.LogWarning("-----------에러-----------");
            return;
        }
        rescuePlayer.UpdateRescueEvent = UIEvent;
    }

    public void UIEvent(float now, float max)
    {
        if (rescuePlayer.UpdateRescueEvent == null)
            return;

        RescueGaugeImage.fillAmount = now / max;
    }

    public void SetActive(bool active)
    {
        RescueBarPanel.SetActive(active);
        RescueGauge.SetActive(active);
    }
}
