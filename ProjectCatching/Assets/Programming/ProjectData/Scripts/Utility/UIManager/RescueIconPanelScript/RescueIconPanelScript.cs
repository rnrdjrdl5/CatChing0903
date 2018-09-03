using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueIconPanelScript{

    const int MAXPLAYER = 6;

    public GameObject RescueIconPanel { get; set; }
    public void InitRescueIconPanel()
    {
        RescueIconPanel = UIManager.GetInstance().OverlayCanvas.transform.Find("RescueIconPanel").gameObject;
    }

    public GameObject[] RescueSet { get; set; }
    public void InitRescueSet()
    {

        RescueSet = new GameObject[MAXPLAYER];

        for (int i = 0; i < MAXPLAYER; i++)
        {
            RescueSet[i] = RescueIconPanel.transform.Find("RescueSet" + (i + 1)).gameObject;
        }
    }

    public Image[] RescueIconsImage { get; set; }
    public void InitRescueIconsImage()
    {

        RescueIconsImage = new Image[MAXPLAYER];

        for (int i = 0; i < MAXPLAYER; i++)
            RescueIconsImage[i] = RescueSet[i].transform.Find("RescueIcon").GetComponent<Image>();
    }

    public void InitData()
    {
        InitRescueIconPanel();
        InitRescueSet();
        InitRescueIconsImage();
    }


}
