using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseImagePanelScript  {

    public int MaxMouseImage = 5;

    /**** MouseImagePanel ****/
    public GameObject MouseImagePanel;
    public void InitMouseImagePanel() { MouseImagePanel = UIManager.GetInstance().UICanvas.transform.Find("MouseImagePanel").gameObject; }

    public GameObject[] MouseImage;
    public void InitMouseImage()
    {
        MouseImage = new GameObject[MaxMouseImage];
        for (int i = 0; i < MaxMouseImage; i++)
        {
            MouseImage[i] = MouseImagePanel.transform.Find("MouseImage" + (i + 1).ToString()).gameObject;
        }
    }

    public GameObject[] MouseOffImage;
    public void InitMouseOffImage()
    {
        MouseOffImage = new GameObject[MaxMouseImage];
        for (int i = 0; i < MaxMouseImage; i++)
        {
            MouseOffImage[i] = MouseImagePanel.transform.Find("MouseOffImage" + (i + 1).ToString()).gameObject;
        }
    }

    public GameObject[] MouseRopeImage;
    public void InitMouseRopeImage()
    {

        MouseRopeImage = new GameObject[MaxMouseImage];

        for (int i = 0; i < MaxMouseImage; i++)
        {
            MouseRopeImage[i] = MouseImagePanel.transform.Find("MouseRopeImage" + (i + 1).ToString()).gameObject;
        }
    }


    public void UpdateMouseState()
    {


        for (int i = 0; i < MaxMouseImage; i++)
        {
            OffMouseImage(i);
        }

        // 1. 쥐 이미지 상태 결정하기.
        for (int i = 0; i < MaxMouseImage; i++)
        {

            // 플레이어 안들어온인원 있으면.
            if (PhotonManager.GetInstance().MousePlayerListOneSort.Count <= i)
                return;

            CheckPlayerLeftGame(i);

            CheckPlayerLive(i);
        }
/*
        // 2. 쥐 순위 정해주기. , 
        List<PhotonPlayer> tempPhotonPlayers = InitTempSortMouseList();

        PointSortMousePlayer(tempPhotonPlayers);

        */
        

        

    }

    // 플레이어 나갔으면 죽은걸로 처리.
    private void CheckPlayerLeftGame(int index)
    {
        // 플레이어 나갔으면 죽은걸로 처리
        if (PhotonManager.GetInstance().MousePlayerListOneSort[index] == null)
            MouseDeadImage(index);

    }

    // 플레이어 살아있는지 확인
    private void CheckPlayerLive(int index)
    {

        

        if (PhotonManager.GetInstance().MousePlayerListOneSort[index] == null)
            return;

        string PlayerType = (string)PhotonManager.GetInstance().MousePlayerListOneSort[index].CustomProperties["PlayerType"];

        if (PlayerType == "Dead")
            MouseDeadImage(index);

        else if (PlayerType == "Rope")
            ChangeMouseRopeImage
                (index);

        else
            MouseLiveImage(index);
    }

    // 죽었을 떄 이미지 변경
    private void MouseDeadImage(int index)
    {
        // 이미지 꺼버림.
        MouseImage[index].SetActive(false);
        MouseRopeImage[index].SetActive(false);
        MouseOffImage[index].SetActive(true);

    }

    private void ChangeMouseRopeImage(int index)
    {
        // 이미지 킨다.
        MouseRopeImage[index].SetActive(true);
        MouseImage[index].SetActive(false);
        MouseOffImage[index].SetActive(false);
    }

    // 살았을 때 이미지 변경
    private void MouseLiveImage(int index)
    {
        // 이미지 킨다.
        MouseImage[index].SetActive(true);
        MouseRopeImage[index].SetActive(false);
        MouseOffImage[index].SetActive(false);
    }
    
    // 쥐 이미지 꺼버림.
    private void OffMouseImage(int index)
    {
        // 이미지 꺼버림.
        MouseImage[index].SetActive(false);
        MouseOffImage[index].SetActive(false);
    }

    // 나간 사람 제외한 소트용 임시 리스트 생성 ,
    private List<PhotonPlayer> InitTempSortMouseList()
    {
        List<PhotonPlayer> photonPlayers = new List<PhotonPlayer>();
        for (int i = 0; i < PhotonManager.GetInstance().MousePlayerListOneSort.Count; i++)
        {
            if(photonPlayers[i] != null)
                photonPlayers.Add(PhotonManager.GetInstance().MousePlayerListOneSort[i]);
        }

        return photonPlayers;
    }

    public void InitData()
    {
        InitMouseImagePanel();
        InitMouseImage();
        InitMouseOffImage();
        InitMouseRopeImage();

        UIManager.GetInstance().UpdateEvent += UpdateMouseState;
    }
}
