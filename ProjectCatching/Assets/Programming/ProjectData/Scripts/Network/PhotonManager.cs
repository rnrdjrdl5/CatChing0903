using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/********
 *  구현방법
 *  CustomProperty의 해쉬값을 이용해서 각종 로딩 여부 파악 ( 코루틴 사용 ) 
 *  
 * 
 * ******/
public class PhotonManager : Photon.PunBehaviour , IPunObservable
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
    
    delegate void ActionMine(params object[] obj);

    Condition condition;
    ConditionLoop conditionLoop;
    RPCActionType rPCActionType;

    ActionMine actionMine;

    /**** public ****/
    public int playTimerNumber;              // 플레이 타이머
    public int MaxCatScore;                     // 최대 스코어



    public float FinishGame_Between_WinLoseUI = 2.0f;           // 승리 패배 후 날아오는 이펙트 사이 대기시간
    public float WinLoseUI_Between_FinishFadeOut = 2.0f;        // 승리패배 날아오는 이펙트 , FadeOut 사이 대기시간
    public float FinishFadeOut_Between_ExitGame = 5.0f;        // 승리패배 날아오는 이펙트 , FadeOut 사이 대기시간

    public float StartImage_WaitTime = 1.5f;

    public List<GameObject> AllPlayers;            // 플레이어들(오브젝트)을 가리키는 변수 , 플레이어에게 무슨 효과를 주려고 할 때.
    public List<PhotonPlayer> MousePlayerListOneSort { get; set; }         // 쥐를 담는 리스트, 1회 정렬 이외에 하지 않음.

    public PhotonPlayer CatPhotonPlayer;


    public GameObject[] MouseLocation;
    public GameObject CatLocation;

    public float GameTimeOutCondition;      // 타임아웃 시 판단게이지
    public float GameBreakCondition;        // 전부 브레이크판단

    public float MenuUIFadeInFadeOut = 1.0f;

    /**** Private ****/
    private UIManager uIManager;                // UI 매니저

    private float TimerValue;               // 대기시간 기다리는 용도


    IEnumerator IEnumCoro;

    private GameObject CurrentPlayer;               // 사용자 플레이어 포톤매니저에서 등록
    private ObjectManager objectManager;            // 오브젝트 매니저

    public enum EnumGameFinish { CATWIN, MOUSEWIN };
    private EnumGameFinish GameFinishType;


    private bool isUse30Second = false;
    

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


        // 캐릭터 선택 효과 발생
        uIManager.selectCharPanelScript.LockEvent();





        // 게임 시작

        condition = new Condition(CheckGameStart);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameStart");
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

    /**** 함수 ****/


    public void AddPlayerScore(string HashName, int PlusScore)
    {
        float NowScore = (float)PhotonNetwork.player.CustomProperties[HashName];

        ExitGames.Client.Photon.Hashtable NextScore = new ExitGames.Client.Photon.Hashtable { { HashName, NowScore + PlusScore } };
        PhotonNetwork.player.SetCustomProperties(NextScore);
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

    // 이겼는지 졌는지 판단함.
    public EnumGameFinish SetGameWinLoseResult(int ResultType)
    {
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];
        EnumGameFinish GameResult = EnumGameFinish.MOUSEWIN;

        GameResult = PlayerGameResult(ResultType);

        return GameResult;

    }

    public EnumGameFinish PlayerGameResult(int type)
    {
        EnumGameFinish enumGameFinish = EnumGameFinish.CATWIN;

        switch (type)
        {
            case 0:
                enumGameFinish = EnumGameFinish.MOUSEWIN;
                break;

            case 1:
                enumGameFinish = EnumGameFinish.CATWIN;
                break;

            case 2:
                enumGameFinish = TimeOutGameResult();
                break;
                
        }

        return enumGameFinish;
        
    }


    public EnumGameFinish TimeOutGameResult()
    {
        float CatGradeScore = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Cat")
            {
                CatGradeScore = (float)PhotonNetwork.playerList[i].CustomProperties["CatScore"];
                break;
            }
        }

        float CatGradePersent = (float)CatGradeScore / (float)PhotonManager.GetInstance().MaxCatScore * 100;

        EnumGameFinish enumGameFinish = EnumGameFinish.CATWIN;

        if (CatGradePersent > GameTimeOutCondition)
            enumGameFinish = EnumGameFinish.CATWIN;

        else
            enumGameFinish = EnumGameFinish.MOUSEWIN;

        return enumGameFinish;
    }


    bool CheckEndTimer()
    {
        if (playTimerNumber <= 0)
            return true;
        else
            return false;
    }

    // 모든 쥐가죽었는지 판단
    bool CheckMouseAllDead()
    {
        bool isFinish = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

            if (PlayerType == "Mouse")
            {
                isFinish = false;
            }
        }

        return isFinish;
    }

    // 일정 게이지 이하일 때 
    bool CheckAllBreak()
    {
        if (CatPhotonPlayer == null)
        {
            Debug.LogWarning("에러");
            return false;
        }

        float CatGradeScore = (float)CatPhotonPlayer.CustomProperties["CatScore"];


        float CatGradePersent;

        if (CatGradeScore <= 0)
            CatGradePersent = 0;

        else
            CatGradePersent = CatGradeScore / PhotonManager.GetInstance().MaxCatScore * 100;


        if(CatGradePersent <= GameBreakCondition)
            return true;

        else
            return false;

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

    // 고양이가 나갔을 때
   /* bool CheckLeftCat()
    {
        }*/



    // 플레이어 삭제
    void DeleteResult(int i)
    {



        uIManager.hpPanelScript.SetHealthPoint(false);



        uIManager.limitTimePanelScript.SetLimitTime(false);
        uIManager.SetAim(false);
        uIManager.gradePanelScript.GradePanel.SetActive(false);
        uIManager.skillPanelScript.SkillPanel.SetActive(false);

        uIManager.pressImagePanelScript.PressImagePanel.SetActive(false);

        
        // 쥐 남은 수 끄기
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(false);

        uIManager.deadOutLinePanelScript.DeadOutLinePanel.SetActive(false);




        // 플레이어 Result UI 설정
        uIManager.endStatePanelScript.SetEndState(true, (EndStatePanelScript.ResultType)i);

        uIManager.OverlayCanvas.SetActive(false);
        

    }

    // 모든 플레이어가 한번씩 고양이를 했는지 여부 = 게임이 끝났는지.
    bool AllPlayCat()
    {
        bool isCheck = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["UseBoss"] == false)
            {
                isCheck = false;
            }
        }

        return isCheck;

    }

    void OffMenuUIActive()
    {
        Debug.Log("asdf");
        uIManager.menuUIPanelScript.OffActive();
        uIManager.selectCharPanelScript.OffActive();

        uIManager.hpPanelScript.SetHealthPoint(true);



        uIManager.limitTimePanelScript.SetLimitTime(true);
        UpdateTimeEvent = uIManager.limitTimePanelScript.TimeTickUpdateEvent;

        uIManager.SetAim(true);
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(true);

        uIManager.gradePanelScript.GradePanel.SetActive(true);
        uIManager.gradePanelScript.SetActiveObjects(true);

        uIManager.skillPanelScript.SkillPanel.SetActive(true);

        uIManager.pressImagePanelScript.PressImagePanel.SetActive(true);
    }

    void StartGamePlayCount()
    {

        // 대기시간 필요성. 즉 가로딩?        
        // uIManager.selectCharPanelScript.SelectCharPanel.SetActive(false);

        UIManager.GetInstance().gameStartCountPanelScript.GameStartCountPanel.SetActive(true);
        TimerValue = 3.0f;

        condition = new Condition(CheckTimeWait);
        conditionLoop = new ConditionLoop(DecreateTimeCountImageAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckCreatePlayer");
        StartCoroutine(IEnumCoro);
    }

    /***** 조건용 함수들 *****/

    // 액션들 
    void NoAction()
    {
    }

    void DecreateTimeAction()
    {
        TimerValue -= Time.deltaTime;
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

    // RPC Condition 들
    int NoRPCActonCondition()
    {
        return -1;
    }

    int MasterResultCheck()
    {
        int Type = -1;

        if (CheckAllBreak())
            Type = 0;

        else if (CheckMouseAllDead())
            Type = 1;

        else if (CheckEndTimer())
            Type = 2;

        if (Type == -1)
        {
            Debug.LogWarning("에러발생");
            Type = 0;
        }

        return Type;
    }

    

    // 개인 사용자용 액션들
    
    void ExitGameRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    // Condition 들

    // 로딩 끝났는지 파악
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

    // 게임 끝났는지 파악,
    bool CheckGameFinish()
    {
        if (CheckMouseAllDead() || CheckEndTimer() || CheckAllBreak())
            return true;

        else
            return false;
    }

    // 대기시간 기다릴 때 이 모두 흘렀는지 파악
    bool CheckTimeWait()
    {
        if (TimerValue <= 0)
        {
            TimerValue = 0;
            return true;
        }
        else
            return false;
    }



    /**** 코루틴 Invoke****/

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

    IEnumerator CoroTriggerMine(Condition condition, ConditionLoop conditionLoop, ActionMine actionMine, params object[] obj)
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

    IEnumerator LastThirtyTime()
    {
        SpringArmObject.GetInstance().GetSystemSoundManager().StopBGSound();
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.UI_LAST_30_SECOND);

        yield return new WaitForSeconds(1.5f);
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayBGSound(SoundManager.EnumBGSound.BG_FAST_INGAME_SOUND);
        yield break;
    }

    // 일반 코루틴

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

    IEnumerator GameResultUI(int Type)
    {
        DeleteResult(Type);


        GameFinishType = SetGameWinLoseResult(Type);

        SpringArmObject.GetInstance().GetSystemSoundManager().FadeOutSound();
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.UI_TIMEOVER_1);


        yield return new WaitForSeconds(FinishGame_Between_WinLoseUI);


        // 1. 패널 보여주기
        uIManager.gameResultPanelScript.GameResultPanel.SetActive(true);

        // 2. 게임종료 이벤트 발생
        GameFinishEvent((int)GameFinishType);
        
        yield return new WaitForSeconds(WinLoseUI_Between_FinishFadeOut);

        uIManager.gameResultPanelScript.GameResultPanel.SetActive(false);
        uIManager.endStatePanelScript.SetEndState(false, EndStatePanelScript.ResultType.BREAK);
        uIManager.fadeImageScript.SetAlpha(1.0f);
        uIManager.fadeImageScript.FadeImage.SetActive(true);




        PlayVideoEndScene((int)GameFinishType);
        // 영상재생
    }

    IEnumerator WaitStartImage()
    {
        UIManager.GetInstance().gameStartCountPanelScript.Start.SetActive(true);

        yield return new WaitForSeconds(StartImage_WaitTime);

        UIManager.GetInstance().gameStartCountPanelScript.Start.SetActive(false);
        UIManager.GetInstance().gameStartCountPanelScript.GameStartCountPanel.SetActive(false);

        yield break;
    }

    IEnumerator ChaneBookUIUsedFade()
    {

        uIManager.fadeImageScript.FadeImage.SetActive(true);
        uIManager.fadeImageScript.SetAlpha(0);


        UIEffect uIEffect = new UIEffect();
        uIEffect.AddFadeEffectNode(uIManager.fadeImageScript.FadeImage, MenuUIFadeInFadeOut, UIEffectNode.EnumFade.IN);
        uIEffect.AddUIEffectCustom(OffMenuUIActive);
        uIEffect.AddFadeEffectNode(uIManager.fadeImageScript.FadeImage, MenuUIFadeInFadeOut, UIEffectNode.EnumFade.OUT);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;

        yield return new WaitForSeconds(MenuUIFadeInFadeOut*2);

        uIManager.fadeImageScript.SetAlpha(1.0f);
        uIManager.fadeImageScript.FadeImage.SetActive(false);

        SpringArmObject.GetInstance().GetSystemSoundManager().PlayBGSound(SoundManager.EnumBGSound.BG_INGAME_SOUND);
        StartGamePlayCount();
    }

    /**** 서버전용) RPC 액션 함수 ****/

    // 고양이 생성 후 캐릭생성 판단하는 트리거 사용
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

    [PunRPC]
    void RPCPreStartCount()
    {

        StartCoroutine("ChaneBookUIUsedFade");
    }

    // UI 보여주고 게임 종료 조건 파악하는 트리거 사용
    [PunRPC]
    void RPCActionCheckCreatePlayer()
    {
        StopCoroutine(IEnumCoro);

        
        GameStartCountPanelScript countScript = UIManager.GetInstance().gameStartCountPanelScript;

        for (int i = 0; i < countScript.Count.Length; i++)
        {
            countScript.Count[i].SetActive(false);
        }

        //스타트 이미지.
        StartCoroutine(WaitStartImage());

        CurrentPlayer.GetComponent<PlayerState>().isCanActive = true;
        

        // 고양이 플레이어 정하기
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            
            string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

            if (PlayerType == "Cat") {
                CatPhotonPlayer = PhotonNetwork.playerList[i];
                break;
            }
                
        }



        // 게임 종료 조건 시작
        condition = new Condition(CheckGameFinish);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(MasterResultCheck);
        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameFinish");
        StartCoroutine(IEnumCoro);


        StartCoroutine(Timer());


    }

    // UI삭제하고 일정 시간 대기하는 트리거 사용
    [PunRPC]
    void RPCActionCheckGameFinish(int Type)
    {

        StartCoroutine("GameResultUI",Type);

    }


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

        
        
    }


    void PlayVideoEndScene(int winType)
    {
        GameObject go = null;


        if (winType == 0)
        {
            go = VideoManager.GetInstance().winLoseVideoScript.CatWinVideo;

            SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound
                (SoundManager.EnumEffectSound.UI_CAT_WIN);

        }

        else if (winType == 1)
        {
            go = VideoManager.GetInstance().winLoseVideoScript.MouseWinVideo;

            SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound
                (SoundManager.EnumEffectSound.UI_MOUSE_WIN);
        }



        go.SetActive(true);

        AutoDestroyVideo autoDestroyVideo = go.GetComponent<AutoDestroyVideo>();
        autoDestroyVideo.AttachEvent(ExitGameRoom);



    }















    /**** 포톤 함수 ****/

    public override void OnLeftRoom()
    {   
        SceneManager.LoadScene(0);
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




