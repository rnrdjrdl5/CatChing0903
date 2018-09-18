﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PhotonManager
{
    // 캐릭터 생성
    [PunRPC]
    void RPCCreatePlayer()
    {

        // 쥐플레이어 결정, 고양이 제외한 플레이어 넘버 결정.
        InitMousePlayerListOneSort();

        // 플레이어 생성
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        // 고양이는 추가로 고양이가 될 수 없도록 해쉬값 생성
        if (PlayerType == "Cat")
        {
            CurrentPlayer = PhotonNetwork.Instantiate("Cat/CatBoss", CatLocation.transform.position, CatLocation.transform.localRotation, 0);
            CurrentPlayer.GetComponent<PlayerMove>().SetPlayerRotateEuler(CatLocation.transform.localRotation.eulerAngles.y);

            PhotonNetwork.player.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "UseBoss", true } });


            AddPlayerScore("CatScore", MaxCatScore);

        }

        else if (PlayerType == "Mouse")
        {
            int playerNumber = -1;

            for (int i = 0; i < MousePlayerListOneSort.Count; i++)
            {

                if (MousePlayerListOneSort[i].ID == PhotonNetwork.player.ID)
                {
                    playerNumber = i;
                }
            }

            if (playerNumber == -1)
                Debug.LogWarning("---에러---");


            CurrentPlayer = PhotonNetwork.Instantiate("Mouse/MouseRunner" + (playerNumber + 1), MouseLocation[playerNumber].transform.position, MouseLocation[playerNumber].transform.localRotation, 0);
            CurrentPlayer.GetComponent<PlayerMove>().SetPlayerRotateEuler(MouseLocation[playerNumber].transform.localRotation.eulerAngles.y);


        }


        // 오브젝트 랜덤 스폰
        RandomObjectSpawn randomObjectSpawn = GetComponent<RandomObjectSpawn>();
        randomObjectSpawn.ObjectSpawn();

        uIManager.selectCharPanelScript.PlayerType = PlayerType;

        uIManager.selectCharPanelScript.isUseDelay = true;


        // 인원수에 따른 물체 삭제
        if (objectManager != null)
        {

            objectManager.DeleteObjPropPlayer();
            Debug.Log("수행완료");

            objectManager.RegisterObjectMount();
            Debug.Log("수행완료2");

            objectManager.CalcObjectMag();
            Debug.Log("수행완료3");
        }

        // 인원수에 따른 변동 조절
        GameTimeOutCondition = mouseWinScoreCondition[MousePlayerListOneSort.Count -1];
        playTimerNumber = playTimes[MousePlayerListOneSort.Count -1];



    }

    public void AddPlayerScore(string HashName, int PlusScore)
    {
        float NowScore = (float)PhotonNetwork.player.CustomProperties[HashName];

        ExitGames.Client.Photon.Hashtable NextScore = new ExitGames.Client.Photon.Hashtable { { HashName, NowScore + PlusScore } };
        PhotonNetwork.player.SetCustomProperties(NextScore);
    }


}