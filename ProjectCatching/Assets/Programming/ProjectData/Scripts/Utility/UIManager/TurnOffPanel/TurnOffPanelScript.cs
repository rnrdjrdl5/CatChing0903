using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPanelScript : MonoBehaviour {

    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas() { InGameCanvas = UIManager.GetInstance().InGameCanvas; }

    public GameObject TurnOffPanel { get; set; }
    public void InitTurnOffPanel()
    {
        TurnOffPanel = InGameCanvas.transform.Find("TurnOffPanel").gameObject;
    }

    public GameObject CartoonLine { get; set; }
    public void InitCartoonLine()
    {
        CartoonLine = TurnOffPanel.transform.Find("CartoonLine").gameObject;
    }


    public GameObject BlackBackGround { get; set; }
    public void InitBlackBackGround()
    {
        BlackBackGround = TurnOffPanel.transform.Find("BlackBackGround").gameObject;
    }



    public GameObject TurnOffLighting { get; set; }
    public void InitTurnOffLighting()
    {
        TurnOffLighting = CartoonLine.transform.Find("TurnOffLighting").gameObject;
    }

    public GameObject TurnOffBipIcon { get; set; }
    public void InitTurnOffBipIcon()
    {
        TurnOffBipIcon = CartoonLine.transform.Find("TurnOffBipIcon").gameObject;
    }

    public void InitData()
    {

        InitInGameCanvas();

        InitTurnOffPanel();
        InitCartoonLine();

        InitBlackBackGround();

        InitTurnOffLighting();
        InitTurnOffBipIcon();
    }




}
