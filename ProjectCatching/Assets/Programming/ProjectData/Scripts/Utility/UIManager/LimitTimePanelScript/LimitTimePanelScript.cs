using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitTimePanelScript
{
    /**** LimitTime ****/

    public GameObject LimitTimePanel { set; get; }
    public void InitLimitTimePanel() { LimitTimePanel = UIManager.GetInstance().UICanvas.transform.Find("LimitTimePanel").gameObject; }


    public GameObject LimitMinuteText { get; set; }
    public void InitLimitMinuteText() { LimitMinuteText = LimitTimePanel.transform.Find("LimitMinuteText").gameObject; }
    private Text LimitMinuteTextText { get; set; }
    public void InitLimitMinuteTextText() { LimitMinuteTextText = LimitMinuteText.GetComponent<Text>(); }

    public GameObject LimitSecondText { get; set; }
    public void InitLimitSecondText() { LimitSecondText = LimitTimePanel.transform.Find("LimitSecondText").gameObject; }
    private Text LimitSecondTextText { get; set; }
    public void InitLimitSecondTextText() { LimitSecondTextText = LimitSecondText.GetComponent<Text>(); }


    public GameObject LimitTimeLineImage { get; set; }
    public void InitLimitTimeLineImage() { LimitTimeLineImage = LimitTimePanel.transform.Find("LimitTimeLineImage").gameObject; }

    public GameObject LimitTimeDoublePointImage { get; set; }
    public void InitLimitTimeDoublePointImage() { LimitTimeDoublePointImage = LimitTimePanel.transform.Find("LimitTimeDoublePointImage").gameObject; }






    public void SetLimitTime(bool isActive)
    {
        LimitTimePanel.SetActive(isActive);

        LimitMinuteText.SetActive(isActive);
        LimitSecondText.SetActive(isActive);

        LimitTimeLineImage.SetActive(isActive);
        LimitTimeDoublePointImage.SetActive(isActive);

    }

    public void InitData()
    {
        InitLimitTimePanel();

        InitLimitMinuteText();
        InitLimitMinuteTextText();
        InitLimitSecondText();
        InitLimitSecondTextText();

        InitLimitTimeLineImage();
        InitLimitTimeDoublePointImage();
    }

    public void TimeTickUpdateEvent(int timeData)
    {
        if (LimitTimePanel.activeInHierarchy == false)
            return;

        // 타임 이벤트 에 따른 이미지 로 전환해주기.,

        int MinuteTime = 0;

        while (true){

            if (timeData < 60)
                break;

            timeData -= 60;
            MinuteTime += 1;               
        }

        LimitMinuteTextText.text = MinuteTime.ToString();
        LimitSecondTextText.text = timeData.ToString();


    }
}
