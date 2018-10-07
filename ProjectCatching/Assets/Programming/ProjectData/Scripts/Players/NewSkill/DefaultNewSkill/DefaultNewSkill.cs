using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2018. 03. 22 반재억
 *  스킬 개편 스크립트
 *  모든 조건들을 따로 관리합니다.
 *  컴포넌트 화 시킵니다. */

/* 2018. 04.02 반재억
 *  조건문의 복잡함, 이양화가 힘들기 때문에
 *  조건 통째로 빼버립니다. */

public class DefaultNewSkill : MonoBehaviour {


    

    /**** public ****/

    public CoolDown coolDown;               // 스킬 쿨타임
    public DefaultInput InputKey;               // 키 입력 조건
    public DefaultInput ExitInputKey;           // 키 탈출 조건

    protected DefaultConditionAction defaultCdtAct;               //조건과 스킬을 가지는 스크립트

    public List<DefaultPlayerSkillDebuff> PlayerSkillDebuffs;               // 디버프 종류

    public Sprite SkillImage;               // 스킬이미지
    public Sprite SkillCoolTimeImage;       // 스킬 쿨타임 이미지

    public float MaxCtnCoolTime;               // 달리기 등 쿨타임을 모두 사용하지 않는 이상 사용가능한 것들.
    public float IncreaseCoolTimeTick;              // 프레임 당 올라가는 수치
    public float DecreaseCoolTimeTick;              // 프레임 당 내려가는 수치

    /**** protected ****/

    protected PhotonView photonView;
    public PhotonView GetphotonView() { return photonView; }

    protected PlayerState playerState;
    public PlayerState GetplayerState() { return playerState; }

    protected Animator animator;

    protected PlayerManager playerManager;
    public PlayerManager GetPlayerManager() { return playerManager; }




    protected int PlayerSkillNumber = -1;      // 플레이어가 스킬 몇번째인지.

    protected float NowCtnCoolTime;         // 현재 달리기 쿨타임
    public float GetNowCtnCoolTime() { return NowCtnCoolTime; }
    public void SetNowCtnCoolTime(float NCCT)
    {
        NowCtnCoolTime = NCCT;
    }

    public bool isUseCtnCoolTime { get; set; }      // 쿨타임 사용하고 있는지, 다 사용중에는 true로 변해서 회복불가.

    public delegate void DeleUpdateCtnCoolTime(float now, float max, int number);
    public DeleUpdateCtnCoolTime UpdateCtnCoolTimeEvent;        //쿨타임 적용하는 이벤트.



    public delegate void DeleSkillEvent_No();

    virtual protected void Awake()
    {
        
        // 기본설정
        SettingBaseOption();

        SettingUIEvent();

        coolDown.DecreaseCoolDown();

    }



    virtual protected void Update()
    {
        defaultCdtAct.ConditionAction();

    }

    // 기본 설정을 합니다.
    void SettingBaseOption()
    {
        // 사용에 필요한 변수들 게임오브젝트로부터 받아옵니다.
        photonView = gameObject.GetComponent<PhotonView>();
        playerState = gameObject.GetComponent<PlayerState>();
        animator = gameObject.GetComponent<Animator>();
        playerManager = gameObject.GetComponent<PlayerManager>();

        isUseCtnCoolTime = false;

    }

    void SettingUIEvent()
    {
        if (photonView == null) return;
        if (!photonView.isMine) return;

            // 이미지 설정
            switch (InputKey.SkillKeyType)
            {
                case DefaultInput.EnumSkillKey.RIGHTMOUSE:
                    PlayerSkillNumber = 0;
                    break;

                case DefaultInput.EnumSkillKey.Q:
                    PlayerSkillNumber = 1;
                    break;

                case DefaultInput.EnumSkillKey.E:
                    PlayerSkillNumber = 2;
                    break;

                case DefaultInput.EnumSkillKey.LEFTSHIFT:
                    PlayerSkillNumber = 3;
                    break;
            }


        if (PlayerSkillNumber == -1)
        {
            Debug.LogWarning("에러");
            return;
        }

        UIManager.GetInstance().skillPanelScript.SkillImage[PlayerSkillNumber].SetActive(true);
        UIManager.GetInstance().skillPanelScript.SkillCoolTime[PlayerSkillNumber].SetActive(true);
        UIManager.GetInstance().skillPanelScript.SkillUseImage[PlayerSkillNumber].SetActive(true);
        UIManager.GetInstance().skillPanelScript.SkillKeyIcon[PlayerSkillNumber].SetActive(true);
        

        UIManager.GetInstance().skillPanelScript.ChangeImage(SkillImage, PlayerSkillNumber);
        UIManager.GetInstance().skillPanelScript.ChangeCooltimeImage(SkillCoolTimeImage, PlayerSkillNumber);
        coolDown.SkillNumber = PlayerSkillNumber;


        SetDrawCoolTimeUIEvent();

        coolDown.CanUseSkillEvent = UIManager.GetInstance().skillPanelScript.UpdateUseSkillImage;
        coolDown.CanNotUseSkillEvent = UIManager.GetInstance().skillPanelScript.UpdateNotUseSkillImage;
    }

    virtual protected void SetDrawCoolTimeUIEvent()
    {
        coolDown.DecreaseEvent = UIManager.GetInstance().skillPanelScript.UpdateSkillCoolTimeImage;
    }

    public void UpdateIncreaseCtnCoolTime()
    {
        NowCtnCoolTime += IncreaseCoolTimeTick * Time.deltaTime;

        

        if (NowCtnCoolTime >= MaxCtnCoolTime)
            NowCtnCoolTime = MaxCtnCoolTime;

            UpdateCtnCoolTimeEvent(NowCtnCoolTime, MaxCtnCoolTime, PlayerSkillNumber);
    }

    public void UpdateDecreaseCtnCoolTime()
    {

        if (isUseCtnCoolTime == false)
        {
            NowCtnCoolTime -= DecreaseCoolTimeTick * Time.deltaTime;

            if (NowCtnCoolTime <= 0)
                NowCtnCoolTime = 0;

                UpdateCtnCoolTimeEvent(NowCtnCoolTime, MaxCtnCoolTime, PlayerSkillNumber);
        }


    }

    /************************  아래부터는 재정의 해야 할 목록 *******************/

    // 현재 상태를 체크합니다.
    public virtual bool CheckState()
    {
        return false;
    }

    // 현재 지속형 스킬을 위한 상태를 체크합니다.
    public virtual bool CheckCtnState()
    {

        return false;
    }


    // 스킬 사용입니다.
    public virtual void UseSkill()
    {
    }

    // 지속형 스킬 사용입니다.
    public virtual void UseCtnSkill()
    {

    }

    // 지속형 스킬 사용의 해제입니다.
    public virtual void ExitCtnSkill()
    {

    }




    // 디버프를 해당 오브젝트에 추가한다.
    protected void AddDebuffComponent(GameObject CollisionObject)
    {
        for(int i = 0; i < PlayerSkillDebuffs.Count; i++)
        {
            if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN))
            {
                CollisionStunDebuff CSD = CollisionObject.AddComponent<CollisionStunDebuff>();
                CSD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }

            else if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.DAMAGED))
            {
                CollisionDamagedDebuff CDD = CollisionObject.AddComponent<CollisionDamagedDebuff>();
                CDD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }

            else if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.NOTMOVE))
            {
                CollisionNotMoveDebuff CNMD = CollisionObject.AddComponent<CollisionNotMoveDebuff>();
                CNMD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }

            else if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.GROGGY))
            {
                CollisionGroggyDebuff CGD = CollisionObject.AddComponent<CollisionGroggyDebuff>();
                CGD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }

            else if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.SLIDE))
            {
                CollisionSlideDebuff CSD = CollisionObject.AddComponent<CollisionSlideDebuff>();
                CSD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }


        }
    }



}
