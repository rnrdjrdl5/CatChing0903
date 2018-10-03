using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PhotonManager : Photon.PunBehaviour , IPunObservable
{

    // 싱글톤
    private static PhotonManager photonManager;
    public static PhotonManager GetInstance() {
        if (photonManager == null) return null;

        return photonManager; }



    // 드라마틱 카메라 모드 
    public GameObject DramaticCameraPrefab;
    private GameObject DramaticCameraObject;

    bool isObserverMode = true;


    //이벤트들
    public delegate void GameFinishDele(int type);
    public delegate void UpdateTimeDele(int timeData);

    public GameFinishDele GameFinishEvent;
    public UpdateTimeDele UpdateTimeEvent;
    




    delegate bool Condition();
    delegate void ConditionLoop();
    delegate int RPCActionType();
    
    delegate void ActionMine_Param(params object[] obj);
    delegate void ActionMine();

    Condition condition;
    ConditionLoop conditionLoop;
    RPCActionType rPCActionType;

    ActionMine actionMine;
    ActionMine_Param actionMine_Param;

    /**** public ****/
    
    public int MaxCatScore;                     // 최대 스코어



    public float FinishGame_Between_WinLoseUI = 2.0f;           // 승리 패배 후 날아오는 이펙트 사이 대기시간
    public float WinLoseUI_Between_FinishFadeOut = 2.0f;        // 승리패배 날아오는 이펙트 , FadeOut 사이 대기시간
    public float FinishFadeOut_Between_ExitGame = 5.0f;        // 승리패배 날아오는 이펙트 , FadeOut 사이 대기시간

    public float StartImage_WaitTime = 1.5f;

    public List<GameObject> AllPlayers { get; set; }            // 플레이어들(오브젝트)을 가리키는 변수 , 플레이어에게 무슨 효과를 주려고 할 때.
    public List<PhotonPlayer> MousePlayerListOneSort { get; set; }         // 쥐를 담는 리스트, 1회 정렬 이외에 하지 않음.

    public PhotonPlayer CatPhotonPlayer;


    public GameObject[] MouseLocation;
    public GameObject CatLocation;

    [Tooltip(" 0-> 1 , 1->2 ... 4->5 로 사용")]
    public int[] playTimes;       // 플레이어 별 시간 수
    private int playTimerNumber;              // 플레이 타이머
    


    public float GameBreakCondition;        // 전부 브레이크판단

    public float MenuUIFadeInFadeOut = 1.0f;

    [Tooltip(" 0-> 1 , 1->2 ... 4->5 로 사용")]
    public float[] mouseWinScoreCondition;  // 타임아웃 판단 인원수별로 조절

    public float GameTimeOutCondition { get; set; }      // 타임아웃 시 판단게이지

    /**** Private ****/
    private UIManager uIManager;                // UI 매니저

    private float TimerValue;               // 대기시간 기다리는 용도


    IEnumerator IEnumCoro;

    private GameObject CurrentPlayer;               // 사용자 플레이어 포톤매니저에서 등록
    private ObjectManager objectManager;            // 오브젝트 매니저

    public enum EnumGameFinish { CATWIN, MOUSEWIN };
    private EnumGameFinish GameFinishType;


    private bool isUse30Second = false;


    // 튜토리얼모드인지 여부 확인
    public bool isTutorial;

    /**** 접근자 ****/

    public void SetCurrentPlayer(GameObject Go)
    {
        CurrentPlayer = Go;
    }

    public GameObject GetCurrentPlayer()
    {
        return CurrentPlayer;
    }


    /**** 유니티 함수 ****/

    private void Awake()
    {
        photonManager = this;

        // 오브젝트 매니저 찾기
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        AllPlayers = new List<GameObject>();

        MousePlayerListOneSort = new List<PhotonPlayer>();
    }

    private void Start()
    {
        
        uIManager = UIManager.GetInstance();
        
        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "InGame" } };
        PhotonNetwork.player.SetCustomProperties(ht);


        condition = new Condition(CheckGameStart);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        // 튜토리얼 아님
        if (!isTutorial)
        {
            // 캐릭터 선택 효과 발생
            uIManager.selectCharPanelScript.LockEvent();

            // 게임 시작
            IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameStart");
        }

        else
        {
            IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCTutoActionCheckGameStart");
        }
        StartCoroutine(IEnumCoro);

        


    }

    // 현재 문제점
    // CustomProperty는 각자가 가짐. 해당 플레이어를 찾아서 점수를 받아야 함.
    // 그런데, 튜토리얼은 한명밖에 없고 쥐인경우 Cat을 찾지 못해서 점수시스템에 문제가 생김.


    // TestPhoton에서 실시, 나중에 없애기.
    public void TutorialStart()
    {
        uIManager = UIManager.GetInstance();

        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "InGame" } };
        PhotonNetwork.player.SetCustomProperties(ht);


        condition = new Condition(CheckGameStart);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);
   
        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCTutoActionCheckGameStart");
        StartCoroutine(IEnumCoro);
    }


    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    private bool isCanUseCursor = false;

    private void Update()
    {

        if (!isCanUseCursor)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {

            if (CurrentPlayer != null)
            {
                SpringArmObject.GetInstance().transform.SetParent(null);

                Vector3 position = CurrentPlayer.transform.position;

                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Dead" } });

                AllPlayers.Remove(CurrentPlayer);

                PhotonNetwork.Destroy(CurrentPlayer);
                DramaticCameraObject = Instantiate(DramaticCameraPrefab, position, Quaternion.identity);

                SpringArmObject.GetInstance().transform.SetParent(DramaticCameraObject.transform);
            }   

        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (isCanUseCursor)
                isCanUseCursor = false;
            else
                isCanUseCursor = true;
        }

    }




    
    public void InitMousePlayerListOneSort()
    {

        // 1. 쥐 플레이어 저장
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Mouse")
                MousePlayerListOneSort.Add(PhotonNetwork.playerList[i]);
        }

        // 2. 소팅 진행
        MousePlayerSorting();

    }

    public void MousePlayerSorting()
    {
        MousePlayerListOneSort.Sort(
            (PhotonPlayer One, PhotonPlayer Two) =>
            {
                if (One.ID > Two.ID)
                    return 1;
                else if (One.ID < Two.ID)
                    return -1;
                return 0;

            }
           );
    }

    void CheckFastTime()
    {

        if (isUse30Second == false)
        {
            if (playTimerNumber <= 30)
            {
                StartCoroutine("LastThirtyTime");
                isUse30Second = true;
            }
        }
    }





    /***** 조건용 함수들 *****/



    // 특수 함수들
    void NoAction()
    {
    }

    void DecreateTimeCountImageAction()
    {
        TimerValue -= Time.deltaTime;


        int CountImage;
        if (TimerValue >= 2 && TimerValue < 3)
        {
            CountImage = 2;
        }

        else if (TimerValue >= 1 && TimerValue < 2)
        {
            CountImage = 1;
        }

        else
        {
            CountImage = 0;
        }


        for (int i = 0; i < UIManager.GetInstance().gameStartCountPanelScript.Count.Length; i++)
        {
            if (i == CountImage &&
                        UIManager.GetInstance().gameStartCountPanelScript.Start.activeInHierarchy == false)

                UIManager.GetInstance().gameStartCountPanelScript.Count[i].SetActive(true);
            else
                UIManager.GetInstance().gameStartCountPanelScript.Count[i].SetActive(false);
        }

    }

    int NoRPCActonCondition()
    {
        return -1;
    }

    bool CheckGameStart()
    {
        bool isInGame = true;

        // 로딩 안된 클라이언트 찾기
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["Scene"] != "InGame")
            {
                isInGame = false;
            }
        }

        return isInGame;
    }

    bool AlwaysCondition() { return true; }


    /**** 특수 코루틴****/

        // 방장이 모든걸 처리함.
    // 조건/ 반복조건 / 액션조건판단 후 액션 으로 나눔
    IEnumerator CoroTrigger(Condition condition, ConditionLoop conditionLoop, RPCActionType rPCActionType, string RPCAction)
    {
        while (true)
        {

            bool AcceptCondition = condition();

            if (AcceptCondition)
            {
                if (PhotonNetwork.isMasterClient)
                {

                    // 액션 사용, 조건 판단해서 사용.
                    int type = rPCActionType();

                    if (type >= 0)
                        photonView.RPC(RPCAction, PhotonTargets.All, type);
                    else if (type == -1)
                        photonView.RPC(RPCAction, PhotonTargets.All);
                }
                yield break;
            }
            else
                conditionLoop();

            yield return null;
        }

    }

    IEnumerator CoroTriggerMine(Condition condition, ConditionLoop conditionLoop, ActionMine_Param actionMine, params object[] obj)
    {
        while (true)
        {

            bool AcceptCondition = condition();

            if (AcceptCondition)
            {

                // 액션 사용, 조건 판단해서 사용.
                actionMine(obj);


                yield break;
            }
            else
                conditionLoop();

            yield return null;
        }
    }

    IEnumerator CoroTriggerMine(Condition condition, ConditionLoop conditionLoop, ActionMine actionMine)
    {
        while (true)
        {

            bool AcceptCondition = condition();

            if (AcceptCondition)
            {

                // 액션 사용, 조건 판단해서 사용.
                actionMine();


                yield break;
            }
            else
                conditionLoop();

            yield return null;
        }
    }




    //일반 코루틴
    IEnumerator LastThirtyTime()
    {
        SpringArmObject.GetInstance().GetSystemSoundManager().StopBGSound();
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.UI_LAST_30_SECOND);

        yield return new WaitForSeconds(1.5f);
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayBGSound(SoundManager.EnumBGSound.BG_FAST_INGAME_SOUND);
        yield break;
    }

    IEnumerator Timer()
    {

        // 마스터 인 경우에만 실시.
        if (PhotonNetwork.isMasterClient)
        {

            while (true)
            {
                
                yield return new WaitForSeconds(1.0f);

                playTimerNumber -= 1;

                UpdateTimeEvent(playTimerNumber);

                CheckFastTime();

                if (playTimerNumber <= 0)
                    yield break;


            }
        }

        else
        {

            while (true)
            {

                UpdateTimeEvent(playTimerNumber);

                CheckFastTime();
                if (playTimerNumber <= 0)
                    yield break;



                yield return null;
            }
        }


    }

    /**** 포톤 함수 ****/


    public string loadingSceneName;

    public override void OnLeftRoom()
    {   
        SceneManager.LoadScene(loadingSceneName);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {

        Debug.Log("나간 사람 : " + otherPlayer.ID);
        // 리스트 내 플레이어 삭제
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i].GetPhotonView().ownerId == otherPlayer.ID)
            {
                Destroy(AllPlayers[i]);
                break;
            }

            if ((string)otherPlayer.CustomProperties["PlayerType"] == "Cat")
            {
                isLeftCatPlayer = true;
            }
        }
    }



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playTimerNumber);
        }

        else
        {
            playTimerNumber = (int)stream.ReceiveNext();
        }
    }

   


}




