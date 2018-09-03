using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuePlayer : Photon.MonoBehaviour, IPunObservable
{
    public float RescueDistance = 10.0f;        // 레이 거리
    public float MaxRescueTime = 3.0f;
    public float RescueHP = 10.0f;

    public float MinPlayerDis = 0.7f;         //플레이어 사이 거리

    public DefaultInput defaultInput;

    public PointToLocation pointToLocation;




    private PhotonView photonView;
    private GameObject tempTargetObject;
    private GameObject targetObject;
    private PlayerState playerState;
    private Animator animator;
    private PlayerMove playerMove;
    private PlayerHealth playerHealth;
    private UIManager uIManager;
    private PlayerBodyPart playerBodyPart;

    private bool isUsedRescue = false;      // 동기화, 살려지는지 판단
    private bool isUpdateRescueTime = false;        // 살리고 있는지 판단. 


    private float NowRescueTime = 0.0f;

    public delegate void deleUpdateRescue(float now, float max);

    public deleUpdateRescue UpdateRescueEvent;

    IEnumerator CoroRescueTime;


    GameObject helpEffect;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        playerState = GetComponent<PlayerState>();
        playerMove = GetComponent<PlayerMove>();
        playerHealth = GetComponent<PlayerHealth>();
        playerBodyPart = GetComponent<PlayerBodyPart>();

        uIManager = UIManager.GetInstance();

        pointToLocation = new PointToLocation();

        CoroRescueTime = UpdateResqueTime();


        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("RescueType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isRevive", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);

    }

    private void Start()
    {
        RegisterEvent();
    }

    private bool CheckMinDistance(float playerDis)
    {

        if (MinPlayerDis >= playerDis) return true;

        else return false;
    }

    public void Update()
    {


        if (!CheckPhotonMine())
            return;



        if (!CheckState())
            return;


        SpringArmObject springArmObject = SpringArmObject.GetInstance();

        tempTargetObject = pointToLocation.FindObject(RescueDistance, "OtherPlayer", SpringArmObject.GetInstance().armCamera);

        if (tempTargetObject == null)
        {
            UIManager.GetInstance().pressImagePanelScript.RescueImage.SetActive(false);
            return;
        }

        float PlayerDistance = (tempTargetObject.transform.position - gameObject.transform.position).magnitude;

        if (!CheckMinDistance(PlayerDistance))
            UIManager.GetInstance().pressImagePanelScript.RescueImage.SetActive(false);



        if (!CheckCanUseResque(PlayerDistance))
            return;

        if (!CheckOtherPlayerState())
            return;

        if (tempTargetObject != null)
            UIManager.GetInstance().pressImagePanelScript.RescueImage.SetActive(true);

        if (!defaultInput.IsUseKey())
            return;



        targetObject = tempTargetObject;

        targetObject.GetComponent<RescuePlayer>().CallCheckUseRescue(PhotonNetwork.player.ID);



    }


    private bool CheckPhotonMine()
    {
        if (photonView.isMine)
            return true;
        else
            return false;
    }


    private bool CheckCanUseResque(float playerDis)
    {


        if (tempTargetObject == null)
            return false;

        if (CheckMinDistance(playerDis))
            return true;

        else
            return false;




    }

    private bool CheckState()
    {
        if (
             (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)) &&
             playerState.GetWeaponType() == PlayerState.WeaponEnum.NONE)
        {
            return true;
        }

        else
            return false;

    }

    private void CheckMinRescue()
    {

        if (playerState.GetPlayerCondition() != PlayerState.ConditionEnum.RESCUE) return;


        if (tempTargetObject == null)
        {
            CancelEvent();
            return;
        }

        float PlayerDistance = (tempTargetObject.transform.position - gameObject.transform.position).magnitude;

        if (!CheckMinDistance(PlayerDistance))
            CancelEvent();
    }


    private bool CheckOtherPlayerState()
    {
        Animator targetAnimator = tempTargetObject.GetComponent<Animator>();

        if (targetAnimator == null)
            return false;


        if (
            (tempTargetObject.GetComponent<PlayerHealth>().GetNowRopeDeadTime() > 0.0f) &&
            (targetAnimator.GetInteger("WeaponType") == 2))
            return true;

        else
            return false;

    }

    private GameObject FindPlayerObject(int vID)
    {
        for (int i = 0; i < PhotonManager.GetInstance().AllPlayers.Count; i++)
        {

            if (PhotonManager.GetInstance().AllPlayers[i].GetPhotonView().ownerId == vID)
                return PhotonManager.GetInstance().AllPlayers[i];
        }

        Debug.LogWarning("에러");
        return null;
    }

    private void UseRescue()
    {

        animator.SetInteger("RescueType", 1);
        playerState.SetPlayerCondition(PlayerState.ConditionEnum.RESCUE);

        playerMove.ResetMoveSpeed();


        CoroRescueTime = UpdateResqueTime();
        StartCoroutine(CoroRescueTime);

    }


    private IEnumerator UpdateResqueTime()
    {
        uIManager.rescueBarPanelScript.SetActive(true);


        while (true)
        {
            CheckMinRescue();

            NowRescueTime += Time.deltaTime;


            UpdateRescueEvent(NowRescueTime, MaxRescueTime);


            if (NowRescueTime >= MaxRescueTime)// 살려짐
            {
                NowRescueTime = 0.0f;

                uIManager.rescueBarPanelScript.SetActive(false);

                animator.SetInteger("RescueType", 2);

                targetObject.GetComponent<RescuePlayer>().CallSuccessRescue();

                StopCoroutine(CoroRescueTime);
                // 코루틴 종료

                // 상태변경하고

                // 체력체우고

                yield break;

            }




            yield return null;
        }
    }

    private void RegisterEvent()
    {
        UIManager.GetInstance().rescueBarPanelScript.SetEvent(gameObject);
        playerMove.RescueCancelEvent = CancelEvent;
    }

    public void CancelEvent()
    {
        // 이미 살리는게 완료되었으면. finish 상태때.
        if (animator.GetInteger("RescueType") == 0)
            return;

        animator.SetInteger("RescueType", 0);
        UIManager.GetInstance().rescueBarPanelScript.SetActive(false);
        NowRescueTime = 0.0f;
        StopCoroutine(CoroRescueTime);

        if (targetObject == null)
            return;

        targetObject.GetComponent<RescuePlayer>().CallOtherCancelEvent();


    }

    public void CreateHelpEffect()
    {

        helpEffect = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.HELP_BUBBLE);
        helpEffect.transform.position = transform.position;
    }

    public void DeleteHelpEffect()
    {

        if (helpEffect == null) return;

        PoolingManager.GetInstance().PushObject(helpEffect);

    }


    // 애니메이션 이벤트

    public void OffRevive()
    {
        animator.SetBool("isRevive", false);
        animator.SetInteger("WeaponType", 0);
    }
    public void ReviveEffect()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.REVIVE_EFFECT);
        go.transform.position = transform.position;
    }


    /***** Call RPC 함수 *****/

    public void CallCheckUseRescue(int vID)
    {
        photonView.RPC("RPCCheckUseRescue", PhotonTargets.All, vID);
    }

    public void CallOKSign()
    {
        photonView.RPC("RPCOKSign", PhotonTargets.All);
    }

    public void CallNoSign()
    {
        photonView.RPC("RPCNoSign", PhotonTargets.All);
    }

    public void CallOtherCancelEvent()
    {
        photonView.RPC("RPCOtherCancelEvent", PhotonTargets.All);
    }

    public void CallSuccessRescue()
    {
        photonView.RPC("RPCSuccessRescue", PhotonTargets.All);
    }
    /***** RPC *****/

    [PunRPC]
    public void RPCCheckUseRescue(int vID)
    {
        if (!CheckPhotonMine())
            return;

        GameObject go = FindPlayerObject(vID);
        RescuePlayer rescuePlayer = go.GetComponent<RescuePlayer>();
        if (!isUsedRescue)
        {

            rescuePlayer.CallOKSign();
            isUsedRescue = true;

            playerHealth.SetisUseRopeDead(false);


        }

        else
            rescuePlayer.CallNoSign();
    }

    [PunRPC]
    public void RPCOKSign()
    {
        if (photonView.isMine)
        {

            UseRescue();
        }


        
        
    }

    [PunRPC]
    public void RPCNoSign()
    {
        if (photonView.isMine)
        {
            Debug.Log("실패함.");
        }


    }

    [PunRPC]
    public void RPCOtherCancelEvent()
    {
        if (!CheckPhotonMine())
            return;

        isUsedRescue = false;

        playerHealth.SetisUseRopeDead(true);
    }

    [PunRPC]
    public void RPCSuccessRescue()
    {
        playerBodyPart.LegRope.SetActive(false);
        playerBodyPart.HandRope.SetActive(false);


        UIManager.GetInstance().rescueIconPanelScript.RescueIconsImage[
            playerHealth.GetPlayerNumber()].fillAmount = 1.0f;
        UIManager.GetInstance().rescueIconPanelScript.RescueSet[
            playerHealth.GetPlayerNumber()].SetActive(false);
        
        playerHealth.SetPlayerNumber(-1);

        

        if (!CheckPhotonMine())
            return;




        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Mouse" } });

        

        playerHealth.ResetNowRopeDeadTime();
        playerHealth.CallDecreaseDeadEvent();

        UIManager.GetInstance().hpPanelScript.SetAliveActive(true);
        isUsedRescue = false;

        playerState.SetWeaponType(PlayerState.WeaponEnum.NONE);
        animator.SetInteger("DamagedType", 0);
        animator.SetBool("isRevive", true);



        playerHealth.CallApplyDamage(-RescueHP);



    }






    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
        
}
