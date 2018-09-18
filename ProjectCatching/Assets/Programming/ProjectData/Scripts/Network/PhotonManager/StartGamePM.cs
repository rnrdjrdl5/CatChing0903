using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PhotonManager
{
    // 게임 시작 후 플레이어가 생성될 때 까지 기다립니다.
    [PunRPC]
    void RPCActionCheckGameStart()
    {

        // 서버 ++ ) 고양이 쥐 지정, 생성
        if (PhotonNetwork.isMasterClient)
        {


            int BossPlayer = -1;

            // 랜덤 플레이어 찾기
            while (true)
            {
                BossPlayer = Random.Range(0, PhotonNetwork.playerList.Length);

                if ((bool)PhotonNetwork.playerList[BossPlayer].CustomProperties["UseBoss"] == false)
                {
                    break;
                }
            }

            // 해쉬 생성
            ExitGames.Client.Photon.Hashtable CatHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Cat" } };
            ExitGames.Client.Photon.Hashtable MouseHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Mouse" } };


            // 해쉬 대입
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if (BossPlayer == i)
                {
                    PhotonNetwork.playerList[i].SetCustomProperties(CatHash);
                }
                else
                {
                    PhotonNetwork.playerList[i].SetCustomProperties(MouseHash);
                }
            }
            // 플레이어 생성
            photonView.RPC("RPCCreatePlayer", PhotonTargets.All);




        }


        // 플레이어 생성 완료
        condition = new Condition(CheckLoading);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCPreStartCount");
        StartCoroutine(IEnumCoro);
    }


    // 생성완료 
    bool CheckLoading()
    {
        bool isLoading = true;

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            string CreatePlayerState = (string)PhotonNetwork.playerList[i].CustomProperties["Offset"];

            if (CreatePlayerState != "LoadingComplete")
            {
                isLoading = false;
            }
        }

        return isLoading;

    }

}
