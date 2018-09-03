using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressImagePanelScript{

    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas()
    {
        InGameCanvas = UIManager.GetInstance().InGameCanvas;
        
    }

    public GameObject PressImagePanel { get; set; }

    public void InitPressImagePanel()
    {
        PressImagePanel = InGameCanvas.transform.Find("PressImagePanel").gameObject;
    }

    public GameObject PressImage { get; set; }
    public void InitPressImage()
    {
        PressImage = PressImagePanel.transform.Find("PressImage").gameObject;
    }

    public GameObject RescueImage { get; set; }
    public void InitRescueImage()
    {
        RescueImage = PressImagePanel.transform.Find("RescueImage").gameObject;
    }

    

    public void InitData()
    {
        InitInGameCanvas();

        InitPressImagePanel();

        InitPressImage();
        InitRescueImage();


    }


}
