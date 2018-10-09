using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInteractionSkill : Photon.MonoBehaviour, IPunObservable {

    public bool IsUseAction { get; set; }


    public delegate void UseInterWeaponDele();

    private UseInterWeaponDele InterWeaponEvent;
    public void AddInterWeaponEvent(UseInterWeaponDele useInterWeaponDele)
    {
        InterWeaponEvent += useInterWeaponDele;
    }


    private GameObject inGameCanvas;                       // UI 캔버스
    private GameObject interactiveObject;                  // 상호작용 오브젝트

    private int interViewID;                    // viewID 동기화 용 , 상호작용 물체에서 물체확인용으로 사용
    private Vector3 OriginalCameraPosition;             //플레이어 첫 카메라 위치

    private Animator animator;                           // 애니메이터 컴포넌트

    private FindObject findObject;                         // 탐지 정보
    public SpringArmObject springArmObject;                       // 카메라 정보
    private PlayerState playerState;                        // 플레이어 상태 정보
    private InteractiveState interactiveState;                   // 상호작용 물체 정보
    private PlayerMove boxPlayerMove;                // 플레이어 이동 스크립트

    public ObjectManager objectManager;             // 오브젝트 매니저

    private PlayerBodyPart playerBodyPart;

    private List<GameObject> Weapons;
    public List<GameObject> GetWeapons() { return Weapons; }

    
    public Vector3 PlayerNextPosition { get; set; }     // 플레이어 상호작용 사용 시 강제이동되는 경우 사용

    /**** 접근자 ****/


    public int GetinterViewID() { return interViewID; }
    public void SetinterViewID(int IV) { interViewID = IV; }

    public GameObject GetinteractiveObject() { return interactiveObject; }
    public void SetinteractiveObject(GameObject IO) { interactiveObject = IO; }

    public InteractiveState GetinteractiveState() { return interactiveState; }
    public void SetinteractiveState(InteractiveState IS) { interactiveState = IS; }

    public ObjectManager GetobjectManager() { return objectManager; }
    public void SetobjectManager(ObjectManager om) { objectManager = om; }

    /**** 유니티 함수 ****/


    private void Awake()
    {

        inGameCanvas = GameObject.Find("InGameObject");                               // 캔버스 설정

        animator = GetComponent<Animator>();                                      // 애니메이터 설정

        findObject = GetComponent<FindObject>();                                    // 탐지 오브젝트 설정
        playerState = GetComponent<PlayerState>();                                   // 플레이어 상태 설정
        boxPlayerMove = GetComponent<PlayerMove>();                        //플레이어 이동 스크립트
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();             // 오브젝트 매니저 초기화

        playerBodyPart = GetComponent<PlayerBodyPart>();

        Weapons = new List<GameObject>();

        OriginalCameraPosition = Vector3.zero;

        // 애니메이션 포톤 뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("InteractionType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("WeaponType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    private void Start()
    {
        springArmObject = SpringArmObject.GetInstance();
    }

    // 동기화, 플레이어 물체정보 동기화
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(interViewID);
            stream.SendNext(OriginalCameraPosition);
        }

        else
        {
            interViewID = (int)stream.ReceiveNext();
            OriginalCameraPosition = (Vector3)stream.ReceiveNext();
        }
    }

    // 캐릭터 스킬 사용 여부 체크
    private void Update()
    {

        // 본인이라면
        if (photonView.isMine)
        {


            // 상호작용이 가능하다면
            if (findObject.GetIsInteraction())
            {

                // 상태를 살핀다.
                if (CheckState())
                {

                    // 키 눌렀는지 판단한다.
                    if (CheckInterKey())
                    {



                        // 1.  상호작용 물체 등록
                        interactiveObject = findObject.GetObjectTarget();
                        interactiveState = interactiveObject.GetComponent<InteractiveState>();

                        // 2. 상호작용 탐지 해제
                        findObject.SetisUseFindObject(false);

                        // 3. 상호작용 설정 디폴트로 변경
                        findObject.BackDefault();

                        // 4. 카메라 설정 변경


                        // 5. 타임 바 보이게하고, 정보 설정함
                        //                        BaseTimeBarScript();
                        UIManager.GetInstance().timerBarPanelScript.PreTimeBar(
                            interactiveState.InteractiveTime);

                        // 6. 상태 NONE 으로 변경
                        playerState.SetPlayerCondition(PlayerState.ConditionEnum.NONE);



                        // 스피드 0으로 설정
                        boxPlayerMove.SetVSpeed(0.0f);
                        boxPlayerMove.SetHSpeed(0.0f);
                        boxPlayerMove.SetMoveDir(Vector3.up * boxPlayerMove.GetMoveDir().y);


                        // 애니메이션 블렌드 위치 0 으로 설정
                        animator.SetFloat("DirectionX", 0);
                        animator.SetFloat("DirectionY", 0);



                        // 플레이어 입력 불가상태로 변경 , 한번 모두 떼야 가능하도록
                        boxPlayerMove.SetisCanKey(false);



                        // 7. 전송
                        interactiveState.CallInterMessage(photonView.viewID, this);

                        // 8. ViewID 동기화
                        // 애니메이션 동기화하고 viewID 동기화시간이 다름, 최대 0.1초

                        interViewID = interactiveObject.GetPhotonView().viewID;

                        // 9. 플레이어 카메라 위치 저장, 동기화
                        OriginalCameraPosition = springArmObject.armCamera.transform.position;
                    }
                }
            }


            // 장착형 상호작용을 가진 경우.
            if (playerState.GetWeaponType() != PlayerState.WeaponEnum.NONE && 
                playerState.GetWeaponType() != PlayerState.WeaponEnum.ROPE)
            {
                if (!CheckState())
                    return;

                // 키 눌렀는지 판단한다.
                if (!CheckInterKey())
                    return;

                UIManager.GetInstance().saucePanelScript.NowOneSauceImage.fillAmount -= 0.1f;

                InterWeaponEvent();


            }

        }
    }



    // 서버에게 함수 받음
    public void UseSkill()
    {


        float RotationY = springArmObject.springArmType.GetSpringArmRotationY();

        springArmObject.SwapSpringArm(SpringArmType.EnumSpringArm.FREE);
        springArmObject.GetfreeSpringArm().SetSpringArmRotationX(boxPlayerMove.GetPlayerRotateEuler());

        springArmObject.springArmType.SetSpringArmRotationY(RotationY);

         
        // ++1. 애니메이션이면 추가로
        if (interactiveState.ActionType == InteractiveState.EnumAction.ANIMATION)
        {

            // 애니메이션의 타입에 따라 고정위치가 달라진다.


            // 플레이어타입이 Position으로 고정형이라면.
            Vector3 v3 = Vector3.zero;
            if (interactiveState.InterPosType == InteractiveState.EnumInterPos.POSITION)
            {

                // 1. 플레이어 위치 고정
                gameObject.transform.position =
                interactiveState.PlayerInterPosition.transform.position;

                


                // 2. 플레이어 각도 고정
                v3 = new Vector3
                {
                    x = gameObject.transform.rotation.eulerAngles.x,
                    y = interactiveState.PlayerInterPosition.transform.rotation.eulerAngles.y,
                    z = gameObject.transform.rotation.eulerAngles.z
                };

                gameObject.transform.rotation = Quaternion.Euler(v3);
            }

            else
            {


              /*  // 1. 플레이어 위치 고정
                gameObject.transform.position =
                interactiveState.PlayerInterPosition.transform.position;


            



                // 2. 플레이어 각도 고정
                v3 = new Vector3
                {
                    x = gameObject.transform.rotation.eulerAngles.x,
                    y = interactiveState.PlayerInterPosition.transform.rotation.eulerAngles.y,
                    z = gameObject.transform.rotation.eulerAngles.z
                };

                gameObject.transform.rotation = Quaternion.Euler(v3);

                */



            }

            interactiveState.CallActionAnimation();



        }

        
        animator.SetInteger("InteractionType", (int)interactiveState.interactiveObjectType);
    }

    // 서버에게 함수 받음
    public void DontUseSkill()
    {

        // 1. 스킬 실패에 따른 상태값 다시 주기
        playerState.SetPlayerCondition(PlayerState.ConditionEnum.IDLE);

        // 2. 스킬 실패했다고 알려줌. 누가 사용중이니까.

        ResetSkill();


    }

    // 상태 체크
    private bool CheckState()
    {
        if (
             (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
              playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ) && 

             playerState.GetWeaponType() != PlayerState.WeaponEnum.ROPE)
        {
            return true;
        }
        else
            return false;
    }

    // 키 체크
    private bool CheckInterKey()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }

    // 스킬 탈출로 인한 첫상태로 복구 , 애니메이션 스테이트 동일내용사용, 이 함수를 쓰지는 않음
    private void ResetSkill()
    {
        // 상호작용 탐지 가능
        findObject.SetisUseFindObject(true);

        // 타임바 파괴
        //timeBar.DestroyObjects();
        UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();
        springArmObject.SwapSpringArm(SpringArmType.EnumSpringArm.FOLLOW);
    }

    // 무기 사용 끝날 때 사용, 이벤트등록해서.
    public void WeaponFinish()
    {
        playerState.SetWeaponType(PlayerState.WeaponEnum.NONE);


        for (int i = Weapons.Count - 1; i >= 0; i--)
        {
            Weapons[i].transform.parent = null;
            PoolingManager.GetInstance().PushObject(Weapons[i]);
            animator.SetInteger("WeaponType", 0);
        }

        UIManager.GetInstance().saucePanelScript.SaucePanel.SetActive(false);
    }

    /**** 애니메이션 이벤트 ****/



    // 액션 사용, 물리나 애니를 호출함
    // interactive 정보가 필요함.

    private Vector3 AddPhysics(PoolingManager.EffctType physicsEffect)
    {
        // Vector3 PhysicsPower = PhysicsPower = transform.position - OriginalCameraPosition;
        Vector3 PhysicsPower = transform.position - OriginalCameraPosition;

        GameObject go = PoolingManager.GetInstance().CreateEffectCameraShake(physicsEffect, photonView.isMine);


        Vector3 DirVector =
            (gameObject.transform.position -
            interactiveState.gameObject.transform.position).normalized;

        DirVector.y = 0;


        go.transform.position = interactiveState.transform.position +
            Vector3.up +
            DirVector * 0.5f;


        go.transform.rotation = Quaternion.identity;

        go.transform.Rotate(Vector3.right, -90.0f);

        return PhysicsPower;

    }


    public delegate void DeleInteractive(int data);
    public event DeleInteractive EventInteractive;

    private void CallAction()
    {
        
        if(EventInteractive != null) EventInteractive((int)interactiveState.interactiveObjectType);

        IsUseAction = true;

        // 물리 일 경우 날라갈 위치의 노말벡터 전달

        if (interactiveState.ActionType == InteractiveState.EnumAction.PHYSICS)
        {
            Vector3 PhysicsPower = transform.forward;                // 물체가 날라갈 XZ방향
            switch (interactiveState.interactiveObjectType)
            {
                case InteractiveState.EnumInteractiveObject.TABLE:
                    PhysicsPower = AddPhysics(interactiveState.physicsEffect);
                    break;

                case InteractiveState.EnumInteractiveObject.CHAIR:
                    PhysicsPower = AddPhysics(interactiveState.physicsEffect);
                    break;

                case InteractiveState.EnumInteractiveObject.CART:
                    PhysicsPower = transform.forward;
                    break;
            }

            interactiveState.UseAction(PhysicsPower);
        }

        else
            interactiveState.UseAction(Vector3.zero);


        // 사용했을 때 스코어 추가
        if (!photonView.isMine)
            return;

        // 장착형이면 추가로 장착한다. 점수 미추가
        if (interactiveState.ActionType == InteractiveState.EnumAction.EQUIP)
        {



            photonView.RPC("RPCEquip", PhotonTargets.All);
            return;
        }

            
        

        
    }

    private void OffInteraction()
    {
        animator.SetInteger("InteractionType", 0);

        if (photonView.isMine)
        {

            // 애니메이션 타입이고
            // 액션이 사용되지 않은 상태라면
            if (interactiveState.ActionType == InteractiveState.EnumAction.ANIMATION &&
                interactiveState.IsUseAction == false)
            {
                interactiveState.CallRPCCancelActionAnimation();
            }


            // FIndObject의 활성화 탐지 시작
            findObject.SetisUseFindObject(true);

            //timeBar.DestroyObjects();
            UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();



            float RotationY = springArmObject.springArmType.GetSpringArmRotationY();

            springArmObject.SwapSpringArm(SpringArmType.EnumSpringArm.FOLLOW);

            boxPlayerMove.SetPlayerRotateEuler(springArmObject.GetfreeSpringArm().GetSpringArmRotationX());

            springArmObject.springArmType.SetSpringArmRotationY(RotationY);

            // 모든 클라이언트에게 RPC전송 하는 함수 콜
            //interactiveState.SetisCanFirstCheck(true);
            // ** 마스터에서 처리하기에는 애니메이션 동기화 문제가 있어서 안됨

            interactiveState.CallOnCanFirstCheck();

            if (interactiveState.PlayerNextPositionType == InteractiveState.EnumPlayerNextPosition.USE)
            {
                gameObject.transform.position = interactiveState.NextPosition.transform.position;
            }

            IsUseAction = false;

        }

    }

    private void InitInterState()
    {

    }

    /***** RPC *****/

    [PunRPC]
    public void RPCEquip()
    {
        
        if (interactiveState.interactiveObjectType == InteractiveState.EnumInteractiveObject.SAUCES)
        {

            if (photonView.isMine)
            {
                UIManager.GetInstance().saucePanelScript.SaucePanel.SetActive(true);
                WeaponInteraction weaponmInteraction = gameObject.AddComponent<WeaponInteraction>();

                weaponmInteraction.InitData(interactiveState.MaxInterUseMount , this);
                weaponmInteraction.AddWeaponInterFinishEvent(WeaponFinish);
            }

            animator.SetInteger("WeaponType", 1);

            boxPlayerMove.SetPreWeaponYRotate(transform.rotation.eulerAngles.y);

            playerState.SetWeaponType(PlayerState.WeaponEnum.SAUCE);

            // 소스 생성
            GameObject RedSauce = PoolingManager.GetInstance().CreateSauce(PoolingManager.SauceType.RED_SAUCE);

            RedSauce.transform.SetParent(playerBodyPart.PlayerRightHand.transform);
            RedSauce.transform.localPosition = Vector3.zero;

            Weapons.Add(RedSauce);



            GameObject YellowSauce = PoolingManager.GetInstance().CreateSauce(PoolingManager.SauceType.YELLOW_SAUCE);

            YellowSauce.transform.SetParent(playerBodyPart.PlayerLeftHand.transform);
            YellowSauce.transform.localPosition = Vector3.zero;

            Weapons.Add(YellowSauce);
        }

        // 2. 장착 ( 부위선택 )

        


    }

    void CheckCameraShakeEffect(GameObject go)
    {
        if (!photonView.isMine) return;

        CameraShake cameraShake = go.GetComponent<CameraShake>();
        if (cameraShake == null) return;

        cameraShake.enabled = true;
        
    }

}

