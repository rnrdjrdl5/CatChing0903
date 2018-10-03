using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 씬 내에서만 간단하게 하려고 제작, 실제로는 사용하지 않기

public class TutoTestPhoton : Photon.PunBehaviour
{

	// Use this for initialization
	void Awake () {
        Debug.Log("시작");
        PhotonNetwork.ConnectUsingSettings("Catcadqching!");

        PhotonNetwork.autoJoinLobby = true;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("로비");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        Debug.Log("찾기실패");
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 6
        };

        PhotonNetwork.CreateRoom("Catching" + Random.Range(0, 1000).ToString(), ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // 씬을 위해서 해쉬 생성
        ExitGames.Client.Photon.Hashtable PlayerSceneState = new ExitGames.Client.Photon.Hashtable
        {
            { "Scene", "Room" }
        };

        ExitGames.Client.Photon.Hashtable PlayerLoadingState = new ExitGames.Client.Photon.Hashtable
        {
            { "Offset","NULL" }
        };

        ExitGames.Client.Photon.Hashtable PlayerType = new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerType","NULL" }
        };

        ExitGames.Client.Photon.Hashtable UseBoss = new ExitGames.Client.Photon.Hashtable
        {
            { "UseBoss",false }
        };

        ExitGames.Client.Photon.Hashtable CatScore = new ExitGames.Client.Photon.Hashtable
        {
            { "StoreScore",0f }
        };

        ExitGames.Client.Photon.Hashtable Round = new ExitGames.Client.Photon.Hashtable
        {
            { "Round",1 }
        };

        PhotonNetwork.player.SetCustomProperties(PlayerSceneState);
        PhotonNetwork.player.SetCustomProperties(PlayerLoadingState);
        PhotonNetwork.player.SetCustomProperties(PlayerType);
        PhotonNetwork.player.SetCustomProperties(UseBoss);
        PhotonNetwork.player.SetCustomProperties(CatScore);

        PhotonNetwork.player.SetCustomProperties(Round);

        Debug.Log("입장");

        PhotonManager.GetInstance().TutorialStart();

    }
}
