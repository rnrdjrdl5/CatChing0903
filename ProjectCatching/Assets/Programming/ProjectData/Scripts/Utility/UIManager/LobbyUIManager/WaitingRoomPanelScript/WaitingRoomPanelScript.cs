using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomPanelScript{
    private LobbyUIManager lobbyUIManager;

    public GameObject WaitingRoomPanel { get; set; }
    public void InitWaitingRoomPanel() { WaitingRoomPanel = lobbyUIManager.UICanvas.transform.Find("WaitingRoomPanel").gameObject; }

    public void InitData()
    {

        lobbyUIManager = LobbyUIManager.GetInstance();

        InitWaitingRoomPanel();
    }

    public void SetActive(bool isActive)
    {

        WaitingRoomPanel.SetActive(isActive);
    }

    public void FadeOutEffect()
    {

    }

    public void FadeInEffect()
    {

    }



}
