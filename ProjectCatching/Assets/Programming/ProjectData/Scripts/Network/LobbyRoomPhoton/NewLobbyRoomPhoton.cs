using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NewLobbyRoomPhoton : Photon.PunBehaviour
{

    private GameObject readyTimeLine;
    private GameObject playTimeLine;

    List<RoomPlayerData> roomPlayerDatas;



    public XMLManager xmlManager;               // XML 전용


    private enum EnumGameState { LOBBY, FINDROOM, ROOM, TITLE }
    private EnumGameState gameStateType;


    private delegate void FadeDelegate();
    private delegate void FadeDelegate_One(bool b);

    private FadeDelegate DeleFadeOut;
    private FadeDelegate_One DeleSetOff;

    private FadeDelegate DeleFadeIn;
    private FadeDelegate_One DeleSetOn;

    private FadeDelegate FinishFadeEvent;

    private LobbyUIManager lobbyUIManager;
    private SoundManager soundManager;



    public float AnimationTime = 1.5f;

    public string gameVersion;

    private bool isUseEvent;
    private GameObject MenuBook;
    private Animator MenuBookAnimator;


    private void Awake()
    {
        gameStateType = EnumGameState.TITLE;

        isUseEvent = true;
        MenuBook = GameObject.Find("MenuBook").gameObject;
        MenuBookAnimator = MenuBook.transform.Find("menu2").GetComponent<Animator>();



        PhotonNetwork.ConnectUsingSettings(gameVersion);

        PhotonNetwork.automaticallySyncScene = true;

        roomPlayerDatas = new List<RoomPlayerData>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        GameObject OpeningObject = GameObject.Find("OpeningObjects").gameObject;

        readyTimeLine = OpeningObject.transform.Find("ReadyTimeLine").gameObject;
        playTimeLine = OpeningObject.transform.Find("PlayTimeLine").gameObject;

        

    }

    // Use this for initialization
    void Start() {

        lobbyUIManager = LobbyUIManager.GetInstance();

        LobbyRoomMouseSetting();

        soundManager.PlayBGSound(SoundManager.EnumBGSound.BG_LOBBY_SOUND);

    }

    // Update is called once per frame
    void Update() {

        if (gameStateType == EnumGameState.ROOM)
        {

            Debug.Log("adsf");
            roomPlayerDatas.Clear();
            InitPlayerList();
            DrawRoomState();
        }

        if (gameStateType == EnumGameState.TITLE)
        {
            if (!Input.anyKeyDown) return;
            if (!isUseEvent) return;

            StartCoroutine("ChangeTitleLobby");

        }
    }



    private void LobbyRoomMouseSetting()
    {

        Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;
    }

    /*** 해당 기능 FSM으로 바꿔서 사용할 수 있음. ****/
    IEnumerator Finish_FadeOut_Start_Animation()
    {

        isUseEvent = false;

        if(DeleFadeOut != null)
        DeleFadeOut();

        yield return new WaitForSeconds(lobbyUIManager.UIFadeTime);
        
        if(DeleSetOff != null)
        DeleSetOff(false);
        // 모두 꺼버린다.

        MenuBookAnimator.SetTrigger("NextTrigger");

        yield return new WaitForSeconds(AnimationTime);

        // 모두 킨다.
        if(DeleSetOn != null)
        DeleSetOn(true);

        if(DeleFadeIn != null)
        DeleFadeIn();

        yield return new WaitForSeconds(lobbyUIManager.UIFadeTime);

        isUseEvent = true;

        if(FinishFadeEvent != null)
            FinishFadeEvent();
    }

    IEnumerator ChangeTitleLobby()
    {

        readyTimeLine.SetActive(false);
        playTimeLine.SetActive(true);

        isUseEvent = false;


        yield return new WaitForSeconds(3.0f);


        gameStateType = EnumGameState.LOBBY;

        DeleSetOn = lobbyUIManager.lobbyPanelScript.SetActive;
        DeleFadeIn = lobbyUIManager.lobbyPanelScript.FadeInEffect;

        DeleSetOn(true);
        DeleFadeIn();
        

        yield return new WaitForSeconds(lobbyUIManager.UIFadeTime);

        isUseEvent = true;
    }

    // 플레이어 리스트 초기화 후 정렬
    void InitPlayerList()
    {

        // 플레이어 정보 생성
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            // 플레이어 데이터 생성 후 삽입
            CreateAddPlayerData(PhotonNetwork.playerList[i]);
        }

        //플레이어 데이터 정렬
        SortPlayerData();
    }

    void SortPlayerData()
    {

        roomPlayerDatas.Sort(
            (RoomPlayerData rp1, RoomPlayerData rp2) =>
            {
                if (rp1.GetPlayerID() > rp2.GetPlayerID())
                    return 1;

                else if (rp1.GetPlayerID() < rp2.GetPlayerID())
                    return -1;

                return 0;
            });

    }

    void DrawRoomState()
    {

        CheckStartButton();

        // 패널  수 
        for (int i = 0; i < lobbyUIManager.roomPanelScript.playerPanelScripts.Length; i++)
        {

            // 유저 접속 패널
            if (i < PhotonNetwork.playerList.Length)
            {

                lobbyUIManager.roomPanelScript.playerPanelScripts[i].SetActive(true);

                if (roomPlayerDatas[i].GetPlayerID() == PhotonNetwork.player.ID)
                    lobbyUIManager.roomPanelScript.playerPanelScripts[i].MeImage.SetActive(true);

                else
                    lobbyUIManager.roomPanelScript.playerPanelScripts[i].MeImage.SetActive(false);


                if (roomPlayerDatas[i].GetIsMaster())
                    lobbyUIManager.roomPanelScript.playerPanelScripts[i].MasterImage.SetActive(true);
                else
                    lobbyUIManager.roomPanelScript.playerPanelScripts[i].MasterImage.SetActive(false);

            }


            // 유저가 접속 안한 패널
            else
            {

                lobbyUIManager.roomPanelScript.playerPanelScripts[i].SetActive(false);
            }

        }
    }

    void CheckStartButton()
    {

        if (PhotonNetwork.isMasterClient)
        {

            //StartPanel 허용한다.
            lobbyUIManager.roomPanelScript.StartImage.GetComponent<Button>().interactable = true;
        }
        else
        {

            lobbyUIManager.roomPanelScript.StartImage.GetComponent<Button>().interactable = false;
        }
    }

    void CreateRandomID()
    {

        xmlManager = new XMLManager();
        List<string> Names = xmlManager.XmlRead();

        while (true)
        {
            string RandomMyName = Names[Random.Range(0, Names.Count)];

            bool isFind = false;


            string SelectMyName = null;
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {

                // 없으면 자기 닉네임으로 선정
                if (PhotonNetwork.playerList[i].NickName != RandomMyName)
                {

                    if (!isFind)
                        SelectMyName = RandomMyName;
                    isFind = true;
                }

                // 하나라도 겹치면 제외
                else
                {
                    Names.Remove(RandomMyName);
                    isFind = false;
                    break;
                }
            }

            // 찾으면 나감.
            if (isFind)
            {
                PhotonNetwork.playerName = SelectMyName;
                break;
            }

        }
    }

    // 플레이어 데이터 생성 후 삽입
    void CreateAddPlayerData(PhotonPlayer pp)
    {
        // 정보 생성
        RoomPlayerData rpd = new RoomPlayerData();

        rpd.InitPlayerData(pp);

        // 리스트에 삽입
        roomPlayerDatas.Add(rpd);
    }

    /***** Photon 함수들 *****/
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        gameStateType = EnumGameState.FINDROOM;

        Debug.Log("게임입장완료");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 6
        };

        PhotonNetwork.CreateRoom("Catching" + Random.Range(0, 1000).ToString(), ro, TypedLobby.Default);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);


        soundManager.PlayRandomEffectSound
            (SoundManager.EnumEffectSound.UI_LOBBYENTER_1,
            SoundManager.EnumEffectSound.UI_LOBBYENTER_2);
            
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_LOBBYLEFT_1);
    }

    public override void OnJoinedRoom()
    {

        base.OnJoinedRoom();

        if (!isUseEvent) return;

        gameStateType = EnumGameState.ROOM;

        DeleFadeOut = lobbyUIManager.waitingRoomPanelScript.FadeOutEffect;
        DeleFadeIn = lobbyUIManager.roomPanelScript.FadeInEffect;

        DeleSetOff = lobbyUIManager.waitingRoomPanelScript.SetActive;
        DeleSetOn = lobbyUIManager.roomPanelScript.SetActive;
        StartCoroutine("Finish_FadeOut_Start_Animation");

        Debug.Log("방에들어옴.");

        // 이름 랜덤 생성
        CreateRandomID();

        // 플레이어 정보 생성
        InitPlayerList();

        // 그리기
        DrawRoomState();

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

        ExitGames.Client.Photon.Hashtable MouseScore = new ExitGames.Client.Photon.Hashtable
        {
            { "MouseScore",0}
        };

        ExitGames.Client.Photon.Hashtable CatScore = new ExitGames.Client.Photon.Hashtable
        {
            { "CatScore",0 }
        };

        ExitGames.Client.Photon.Hashtable Round = new ExitGames.Client.Photon.Hashtable
        {
            { "Round",1 }
        };

        PhotonNetwork.player.SetCustomProperties(PlayerSceneState);
        PhotonNetwork.player.SetCustomProperties(PlayerLoadingState);
        PhotonNetwork.player.SetCustomProperties(PlayerType);
        PhotonNetwork.player.SetCustomProperties(UseBoss);
        PhotonNetwork.player.SetCustomProperties(MouseScore);
        PhotonNetwork.player.SetCustomProperties(CatScore);

        PhotonNetwork.player.SetCustomProperties(Round);


    }



    /***** UI 이벤트 *****/

    public void ClickEnterLobby()
    {
        if (!isUseEvent) return;

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_BUTTONCLICK_1);

        DeleFadeOut = lobbyUIManager.lobbyPanelScript.FadeOutEffect;
        DeleFadeIn = lobbyUIManager.waitingRoomPanelScript.FadeInEffect;

        DeleSetOff = lobbyUIManager.lobbyPanelScript.SetActive;
        DeleSetOn = lobbyUIManager.waitingRoomPanelScript.SetActive;
        StartCoroutine("Finish_FadeOut_Start_Animation");
        FinishFadeEvent = WaitingRoomEvent;





    }



    public void ClickExitClient()
    {
        if (!isUseEvent) return;        

        Application.Quit();
    }

    public void ClickGameOption()
    {
        if (!isUseEvent) return;

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_BUTTONCLICK_1);

        Debug.Log("게임옵션");
        // 게임옵션
    }

    public void ClickGameStart()
    {

        if (!isUseEvent) return;

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_BUTTONCLICK_1);

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        //PhotonNetwork.LoadLevel("Loading");
        photonView.RPC("RPCLoadingScene",PhotonTargets.All);
    }

    public void ClickGameHelp()
    {
        if (!isUseEvent) return;

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_BUTTONCLICK_1);
    }

    public void ClickGameExit()
    {


        if (!isUseEvent) return;

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_BUTTONCLICK_EXIT_1);


        PhotonNetwork.LeaveRoom();
        gameStateType = EnumGameState.LOBBY;

        DeleFadeOut = lobbyUIManager.roomPanelScript.FadeOutEffect;
        DeleFadeIn = lobbyUIManager.lobbyPanelScript.FadeInEffect;

        DeleSetOff = lobbyUIManager.roomPanelScript.SetActive;
        DeleSetOn = lobbyUIManager.lobbyPanelScript.SetActive;

        StartCoroutine("Finish_FadeOut_Start_Animation");
    }

    /**** Fade 이벤트 ****/

    public void WaitingRoomEvent()
    {
        gameStateType = EnumGameState.FINDROOM;
        PhotonNetwork.JoinLobby();
        FinishFadeEvent = null;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Loading");
    }


    /**** RPC ****/
    [PunRPC]
    public void RPCLoadingScene()
    {
        DeleFadeOut = lobbyUIManager.roomPanelScript.FadeOutEffect;
        DeleFadeIn = null;
        DeleSetOff = lobbyUIManager.roomPanelScript.SetActive;
        DeleSetOn = null;
        FinishFadeEvent = ChangeScene;
        StartCoroutine("Finish_FadeOut_Start_Animation");
        soundManager.FadeOutSound();
        
    }








}
