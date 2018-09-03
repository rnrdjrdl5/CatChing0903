using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharPanelScript{

    public float MaxRouletteTick { get; set; }              // 틱

    public float DelayRouletteTick { get; set; }            // 틱에 딜레이를 추가함
    public float ExitRouletteTick { get; set; }       // 틱 일정 이상이면 이벤트 취소함
    public float DelayRouletteTickWeight { get; set; }      // 틱 딜레이 가중치 , 추가 틱 상승률

    public bool isUseDelay { get; set; }        // 딜레이 사용 여부
    
    public string PlayerType { get; set; }

    

    public bool isRouletteFinish { get; set; }


    public GameObject SelectCharPanel { get; set; }

    public GameObject CatCharPanel { get; set; }
    public GameObject MouseCharPanel { get; set; }

    public GameObject CatSelectShadow { get; set; }
    public GameObject CatSelectImage { get; set; }
    public GameObject CatSelectOutLineImage { get; set; }
    public GameObject CatBubbleSpeech { get; set; }

    public GameObject MouseSelectShadow { get; set; }
    public GameObject MouseSelectImage { get; set; }
    public GameObject MouseSelectOutLineImage { get; set; }
    public GameObject MouseBubbleSpeech { get; set; }





    private float ChangeDelayTimeCount;          // 오프셋의 추가값이 바뀌는 시점

    private float NowRouletteTick;
    private bool isCat;

    private float maxFakeLoading = 1.0f;
    private float nowFakeLoading;

    UIEffect uIEffect;


    public void InitData()
    {
        uIEffect = new UIEffect();

        SelectCharPanel = UIManager.GetInstance().UICanvas.transform.Find("SelectCharPanel").gameObject;

        CatCharPanel = SelectCharPanel.transform.Find("CatCharPanel").gameObject;
        MouseCharPanel = SelectCharPanel.transform.Find("MouseCharPanel").gameObject;

        CatSelectShadow = CatCharPanel.transform.Find("CatSelectShadow").gameObject;
        CatSelectImage = CatCharPanel.transform.Find("CatSelectImage").gameObject;
        CatSelectOutLineImage = CatCharPanel.transform.Find("CatSelectOutLineImage").gameObject;
        CatBubbleSpeech = CatCharPanel.transform.Find("CatBubbleSpeech").gameObject;

        MouseSelectShadow = MouseCharPanel.transform.Find("MouseSelectShadow").gameObject;
        MouseSelectImage = MouseCharPanel.transform.Find("MouseSelectImage").gameObject;
        MouseSelectOutLineImage = MouseCharPanel.transform.Find("MouseSelectOutLineImage").gameObject;
        MouseBubbleSpeech = MouseCharPanel.transform.Find("MouseBubbleSpeech").gameObject;

        NowRouletteTick = 0.0f;
        ChangeDelayTimeCount = 0.0f;
        nowFakeLoading = 0.0f;

        isCat = false;
        isUseDelay = false;

        UIManager uIManager = UIManager.GetInstance();



        MaxRouletteTick = uIManager.SelectChar_MaxRouletteTick;
        DelayRouletteTick = uIManager.SelectChar_DelayRouletteTick;
        ExitRouletteTick = uIManager.SelectChar_ExitRouletteTick;
        DelayRouletteTickWeight = uIManager.SelectChar_DelayRouletteTickWeight;


    }

    public void RouletteUpdate()
    {

        if (CheckDestroyUpdate()) return;
        if (MaxRouletteTick == 0.0f) { Debug.Log("룰렛 틱 설정 안함."); return; }


        NowRouletteTick += Time.deltaTime;
        if (NowRouletteTick < MaxRouletteTick) return;


        NowRouletteTick = 0;

        CheckIncreaseTickDelay();

        SwapPlayerType();
        SelectRolette();


        SpringArmObject.GetInstance().GetSystemSoundManager().PlayRandomEffectSound(
            SoundManager.EnumEffectSound.UI_ROULETTE_1,
            SoundManager.EnumEffectSound.UI_ROULETTE_3);
        
    }

    public void LockEvent() { UIManager.GetInstance().UpdateEvent += RouletteUpdate; }

    public void UnlockEvent() { UIManager.GetInstance().UpdateEvent -= RouletteUpdate; }



    public void SelectRolette()
    {

        Vector3 ScaleUp = new Vector3(1.05f, 1.05f, 1.0f);

        if (isCat)
        {

            CatSelectImage.transform.localScale = ScaleUp;

            MouseSelectImage.transform.localScale = Vector3.one;
        }

        else
        {

            CatSelectImage.transform.localScale = Vector3.one;

            MouseSelectImage.transform.localScale = ScaleUp;
        }
    }

    public void SwapPlayerType()
    {
        if (isCat == true) isCat = false;

        else isCat = true;
    }

    public void StopRoulette()
    {

        if (PlayerType == "Cat") isCat = true;

        else isCat = false;        

        UnlockEvent();
        SelectRolette();
    }

    private void CheckIncreaseTickDelay()
    {

        if (!isUseDelay) return;

        MaxRouletteTick += DelayRouletteTick;
        DelayRouletteTick += DelayRouletteTickWeight;
        ChangeDelayTimeCount++;

        // 추가로 가중치 이상이면 .
        if (ChangeDelayTimeCount >= 40)
            DelayRouletteTick += DelayRouletteTickWeight;
    }



    private bool CheckDestroyUpdate()
    {


        if (MaxRouletteTick >= ExitRouletteTick)
        {

            isUseDelay = false;
            StopRoulette();

            UIManager.GetInstance().UpdateEvent += FakeLoading;


            


            GameObject targetObject = ChangeSelectCharImage();

            
            uIEffect.AddScaleEffectNode(targetObject,100, 105, 0.1f);
            uIEffect.AddScaleEffectNode(targetObject,110, 100, 0.1f);
            

            
            UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;


            FakeLoading();

            return true;
        }

        else
            return false;

    }

    GameObject ChangeSelectCharImage()
    {

        GameObject go;
        if (isCat == true)
        {
            CatSelectImage.SetActive(false);
            CatSelectOutLineImage.SetActive(true);
            go = CatSelectOutLineImage;
        }

        else
        {
            MouseSelectImage.SetActive(false);
            MouseSelectOutLineImage.SetActive(true);
            go = MouseSelectOutLineImage;
        }

        return go;

    }

    void FakeLoading()
    {
        nowFakeLoading += Time.deltaTime;

        if (nowFakeLoading >= maxFakeLoading)
        {
            UIManager.GetInstance().UpdateEvent -= FakeLoading;
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Offset", "LoadingComplete" } });
            Debug.Log("가로딩끝");
        }
            
    }

    public void OffActive()
    {
        SelectCharPanel.SetActive(false);
    }
}
