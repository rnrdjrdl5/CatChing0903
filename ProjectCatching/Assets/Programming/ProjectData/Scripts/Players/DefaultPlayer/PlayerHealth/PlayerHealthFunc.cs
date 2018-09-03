using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth
{


    IEnumerator EnumCoro;
    public delegate void HealthDele();

    public event HealthDele HealthEvent;

    private int playerNumber = -1;
    public int GetPlayerNumber() { return playerNumber; }
    public void SetPlayerNumber(int pn) { playerNumber = pn; }

    private void SetAwake()
    {


        photonManager = PhotonManager.GetInstance();

        playerState = GetComponent<PlayerState>();

        animator = GetComponent<Animator>();

        isHiting = false;
        NowHiting = 0.0f;


        UIManager.GetInstance().hpPanelScript.SetPlayerHealth(gameObject);




    }
    public void SetAwakeAll()
    {
        playerBodyPart = GetComponent<PlayerBodyPart>();
    }


    private void CheckRopeDead()
    {
        if (!isUseRopeDead)
            return;


        NowRopeDeadTime -= Time.deltaTime;

        if (NowRopeDeadTime <= 0)
        {

            isUseRopeDead = false;

            EnumCoro = PlayerDead();
            StartCoroutine(EnumCoro);
        }

        // 이벤트 발생
        DecreaseDeadEvent(NowRopeDeadTime, MaxRopeDeadTime);


    }

    private void CheckRopeHelp()
    {

        if (playerNumber == -1) return;

        Vector3 RescueIconPosition = 
            SpringArmObject.GetInstance().armCamera.GetComponent<Camera>().WorldToScreenPoint(GetComponent<PlayerBodyPart>().UpHeadPosition.transform.position);

        GameObject RescutIconObject =
            UIManager.GetInstance().rescueIconPanelScript.RescueSet[playerNumber];

        if (RescueIconPosition.z < 0)
            RescutIconObject.SetActive(false);

        else
            RescutIconObject.SetActive(true);


        RescutIconObject.transform.position = RescueIconPosition;

        UIManager.GetInstance().rescueIconPanelScript.RescueIconsImage[playerNumber].fillAmount =
            NowRopeDeadTime / MaxRopeDeadTime;

        
        

    }

    
    


    // 일반 데미지
    public void CallApplyDamage(float _damage)
    {
        photonView.RPC("ApplyDamage", PhotonTargets.All, _damage);
    }

    // 낙하될 떄 데미지
    public void CallFallDamage(float _damage)
    {
        photonView.RPC("FallDamage", PhotonTargets.All, _damage);
    }

    public void BindRope()
    {
        // 1. 상태 변경
        playerState.SetWeaponType(PlayerState.WeaponEnum.ROPE);
        animator.SetInteger("WeaponType", 2);
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Rope" } });
        // 1. 죽는 애니메이션 재생

        EnumCoro = CoroRopeDeadAnimation();

        StartCoroutine(EnumCoro);






    }

    private void PostDeadAction()
    {
        Debug.Log("플레이어사망");


        // 플레이어 타입 죽은 상태로 전환
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Dead" } });

        // 옵저버 모드 온

        SpringArmObject.GetInstance().IsUseObserver = true;

        SpringArmObject.GetInstance().SwapSpringArm(SpringArmType.EnumSpringArm.FREE);

        UIManager.GetInstance().SetDeadUI();
        PoolingManager.GetInstance().PositionArea.SetActive(false);

        // 파괴대신 비활성화 시키고, 
        CallPostDeadAction();

    }



    IEnumerator CoroRopeDeadAnimation()
    {
        animator.SetBool("isRopeDead", true);

        
        yield return new WaitForSeconds(RopeDeadAnimation);
        photonView.RPC("OnRope", PhotonTargets.All);
        animator.SetBool("isRopeDead", false);

        


        UIManager.GetInstance().hpPanelScript.DieHPImage.SetActive(true);
        NowRopeDeadTime = MaxRopeDeadTime;
        isUseRopeDead = true;





        yield return null;
    }

    IEnumerator PlayerDead()
    {
        animator.SetBool("isRopeDead", true);

        

        yield return new WaitForSeconds(1.5f);

        // 개선필요 
        photonView.RPC("RPCDeadEffect", PhotonTargets.All);
        

        yield return new WaitForSeconds(PlayerFinalDeadtime - 1.5f);

        PostDeadAction();
        yield break;
    }


    public void CallPostDeadAction()
    {
        photonView.RPC("RPCPostDeadAction", PhotonTargets.All);
    }

    /**** RPC ****/

    [PunRPC]
    private void ApplyDamage(float _damage)
    {


            // 본인 인 경우에만
            if (!gameObject.GetComponent<PhotonView>().isMine)
                return;

        if (playerState.GetWeaponType() != PlayerState.WeaponEnum.ROPE)
        {
            // 데미지 입음
            NowHealth -= _damage;


            // 등록한 스크립트들에게 이벤트 실행
            HealthEvent();


            // 체력 0이하면 밧줄로 변경
            if (NowHealth <= 0)
            {
                NowHealth = 0;
                BindRope();
            }

            else if (NowHealth >= 100)
                NowHealth = 100.0f;
        }




    }

    [PunRPC]
    private void FallDamage(float _damage)
    {
        // 본인 인 경우에만
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (NowHealth - _damage <= 0)
                NowHealth = 1;
            else
                NowHealth -= _damage;
        }

        // 등록한 스크립트들에게 이벤트 실행
        if (HealthEvent == null)
            return;

        HealthEvent();
    }

    [PunRPC]
    private void RPCPostDeadAction()
    {
        UIManager.GetInstance().rescueIconPanelScript.RescueSet[playerNumber].SetActive(false);
        playerNumber = -1;

        PhotonManager.GetInstance().AllPlayers.Remove(gameObject);

        if (SpringArmObject.GetInstance().PlayerObject == gameObject)
        {
            SpringArmObject.GetInstance().transform.SetParent(null);
            SpringArmObject.GetInstance().FindSeePlayer();
        }

       

        gameObject.SetActive(false);
    }

    public void FlushEffect()
    {
       /* // 그 외에도 이펙트는 준다.
        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material.color = Color.red;
        }

        isHiting = true;
        NowHiting = 0.0f;*/

    }

    [PunRPC]
    public void OnRope()
    {

        playerBodyPart.HandRope.SetActive(true);
        playerBodyPart.LegRope.SetActive(true);

        for (int i = 0; i < PhotonManager.GetInstance().AllPlayers.Count; i++)
        {

            if (PhotonManager.GetInstance().AllPlayers[i].GetPhotonView().ownerId ==
                gameObject.GetPhotonView().ownerId)
            {
                UIManager.GetInstance().rescueIconPanelScript.RescueIconPanel.SetActive(true);
                UIManager.GetInstance().rescueIconPanelScript.RescueSet[i].SetActive(true);

                playerNumber = i;
            }
                
        }
    }

    [PunRPC]
    public void OffRope()
    {

        playerBodyPart.HandRope.SetActive(true);
        playerBodyPart.LegRope.SetActive(true);
    }

    [PunRPC]
    public void RPCDeadEffect()
    {
        Debug.Log("액션");

        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.DIE_EFFECT);

        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(0.0f, 0.3f, 1.0f);
    }






}
