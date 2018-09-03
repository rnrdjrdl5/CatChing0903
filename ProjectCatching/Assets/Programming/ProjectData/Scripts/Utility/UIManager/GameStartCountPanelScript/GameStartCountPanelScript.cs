using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartCountPanelScript{

    const int COUNT = 3;

    public GameObject GameStartCountPanel { get; set; }
    public void InitGameStartCountPanel()
    {
        GameStartCountPanel = UIManager.GetInstance().UICanvas.transform.Find("GameStartCountPanel").gameObject;
    }

    public GameObject[] Count { get; set; }
    public void InitCount()
    {

        Count = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            Count[i] = GameStartCountPanel.transform.Find("Count" + (i+1)).gameObject;
        }
    }

    public GameObject Start { get; set; }
    public void InitStart()
    {
        Start = GameStartCountPanel.transform.Find("Start").gameObject;
    }

    public void InitData()
    {
        InitGameStartCountPanel();

        InitCount();
        InitStart();
    }



}
