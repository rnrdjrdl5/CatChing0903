using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaucePanelScript : MonoBehaviour {

    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas()
    {
        InGameCanvas = UIManager.GetInstance().InGameCanvas;
    }


    public GameObject SaucePanel { get; set; }
    public void InitSaucePanel()
    {
        SaucePanel = InGameCanvas.transform.Find("SaucePanel").gameObject;
    }

    public GameObject OneSaucePanel { get; set; }
    public void InitOneSaucePanel()
    {
        OneSaucePanel = SaucePanel.transform.Find("OneSaucePanel").gameObject;
    }


    public Image NowOneSauceImage { get; set; }
    public void InitNowOneSauceImage()
    {
        NowOneSauceImage = OneSaucePanel.transform.Find("NowOneSauceImage").GetComponent<Image>(); ;
    }

    public Image MaxOneSauceImage { get; set; }
    public void InitMaxOneSauceImage()
    {
        NowOneSauceImage = OneSaucePanel.transform.Find("MaxOneSauceImage").GetComponent<Image>();
    }

    public void InitData()
    {
        InitInGameCanvas();

        InitSaucePanel();

        InitOneSaucePanel();

        InitMaxOneSauceImage();
        InitNowOneSauceImage();


    }

    public void DecreaseSauceUI(float InterPoint)
    {
        NowOneSauceImage.fillAmount = InterPoint;
    }
}
