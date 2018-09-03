using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStatePanelScript{

    /**** Type ****/

    public enum ResultType { BREAK, KILL, TIMEOVER }

    /**** EndStatePanel ****/

    public GameObject EndStatePanel { set; get; }
    public void InitEndStatePanel()
    {
        EndStatePanel = UIManager.GetInstance().ResultUI.transform.Find("EndStatePanel").gameObject;
    }


    public GameObject AllBreakImage { set; get; }
    public void InitAllBreakImage() { AllBreakImage = EndStatePanel.transform.Find("AllBreakImage").gameObject; }
    public GameObject AllKillImage { set; get; }
    public void InitAllKillImage() { AllKillImage = EndStatePanel.transform.Find("AllKillImage").gameObject; }
    public GameObject TimeOverImage { set; get; }
    public void InitTimeOverImage() { TimeOverImage = EndStatePanel.transform.Find("TimeOverImage").gameObject; }

    public void SetEndState(bool isActive, ResultType rT)
    {
        switch (rT)
        {

            case ResultType.BREAK:
                AllBreakImage.SetActive(isActive);
                break;
            case ResultType.KILL:
                AllKillImage.SetActive(isActive);
                break;
            case ResultType.TIMEOVER:
                TimeOverImage.SetActive(isActive);
                break;
        }
        Debug.Log((ResultType)rT);

        EndStatePanel.SetActive(isActive);
    }

    public void InitData()
    {
        InitEndStatePanel();

        InitAllBreakImage();
        InitAllKillImage();
        InitTimeOverImage();
    }


}
