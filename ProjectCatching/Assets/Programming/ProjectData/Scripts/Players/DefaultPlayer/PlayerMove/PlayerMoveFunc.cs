using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{

    void SetStart()
    {
        springArmObject = SpringArmObject.GetInstance();
    }

    // 초기 설정
    void SetAwake()
    {
        PreWeaponYRotate = 0.0f;

        characterController = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        ps = gameObject.GetComponent<PlayerState>();

        newInteractionSkill = GetComponent<NewInteractionSkill>();
        findObject = GetComponent<FindObject>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSoundManager = GetComponent<SoundManager>();

        OriginalPlayerSpeed = PlayerSpeed;

        // 애니메이션 뷰 설정
        PhotonAnimatorView pav = gameObject.GetComponent<PhotonAnimatorView>();

        if (pav == null) return;

        pav.SetParameterSynchronized("DirectionX", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
        pav.SetParameterSynchronized("DirectionY", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
        pav.SetParameterSynchronized("JumpType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
        pav.SetParameterSynchronized("StunOnOff", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
    }

    bool JumpCondition()
    {
        if ((ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN)) &&
             ps.isCanActive == true &&

             Input.GetKeyDown(KeyCode.Space) &&

             ps.GetWeaponType() != PlayerState.WeaponEnum.ROPE &&
             IsJumpGround())
            return true;

        else
            return false;

    }

    bool MoveCondition()
    {
        if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.EMOTICON) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.RESCUE) ||
             ps.EqualPlayerCondition(PlayerState.ConditionEnum.HIDE )
             )
            return true;

        else
            return false;
    }



    void PlayerTransform()
    {

        if (photonView.isMine)
        {

            if (MoveCondition())
            {

                //if (characterController.isGrounded)
                if(IsLandGround())
                {
                    float tempDirY = MoveDir.y;
                    // 1. 플레이어 이동방향 설정
                    MoveDir = new Vector3(HSpeed,0, VSpeed);

                    // 2. 노말처리
                    float NormalsqrMag = MoveDir.normalized.sqrMagnitude;

                    // 3. 방향전환 여부 선택
                    MoveDir = CheckMoveDash();

                    // 노말벡터 길이 vs 일반벡터 길이
                    if (MoveDir.sqrMagnitude > NormalsqrMag &&
                        NormalsqrMag > 0)
                    {
                        MoveDir = MoveDir.normalized;
                    }



                    CheckResetInteraction();
                    CheckResetRescue();

                    // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
                    MoveDir = transform.TransformDirection(MoveDir);

                    // 앞으로 이동 시
                    if (ps.GetWeaponType() != PlayerState.WeaponEnum.ROPE)
                    {
                        if (Input.GetAxisRaw("Vertical") >= 0 && VSpeed >= 0)
                        {
                            MoveDir *= PlayerSpeed;
                        }

                        // 뒤로 이동 시
                        else
                        {
                            MoveDir *= PlayerBackSpeed;
                        }
                    }

                    else
                        MoveDir *= PlayerRopeSpeed * PlayerRopeCurve;

                    MoveDir.y = tempDirY;
                }


            }


            // 캐릭터 조건부 x방향 회전
            if (!ps.EqualPlayerCondition(PlayerState.ConditionEnum.STUN) &&
                !ps.EqualPlayerCondition(PlayerState.ConditionEnum.GROGGY) &&
                !ps.EqualPlayerCondition(PlayerState.ConditionEnum.NOTCOLLISION))
                SetPlayerRotateX();



            // 점프했으면
            if (JumpCondition())
            {
                // 1. 점프 준비자세 시작

                // - 원래 이동속도로 돌립니다.
                if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN))
                {
                    MoveDir /= PlayerSpeed;

                    PlayerSpeed = OriginalPlayerSpeed;

                    MoveDir *= PlayerSpeed;
                }

                // - 점프 시작
                animator.SetInteger("JumpType", 1);

                


               // - 점프 추가
                MoveDir.y = JumpSpeed;
                


            }

            // 점프 아니면 중력.
            else
            {


                if (MoveDir.y > -9.8f) MoveDir.y -= gravity * Time.deltaTime;
                else MoveDir.y = -9.8f;
                


                    
            }

            // 캐릭터 움직임.
            characterController.Move(MoveDir * Time.deltaTime);

            RaycastHit hit;

            if (MoveDir.y < 0 &&
                animator.GetInteger("JumpType") == 1)
            {
                Debug.Log("점프 최대 도달 후 MoveDir.y : " + MoveDir.y);
                animator.SetInteger("JumpType", 2);

                PreFallPosition = transform.position.y;

            }





            else if (!IsLandGround() &&

            //else if (!characterController.isGrounded &&
                 animator.GetInteger("JumpType") == 0)
            {
                animator.SetInteger("JumpType", 2);

                // 플레이어 최대 위치등록
                PreFallPosition = transform.position.y;

            }


            // 지상일때, 착지 애니메이션 사용중일때 
            //else if (characterController.isGrounded &&
            else if(IsLandGround() && 
                MoveDir.y < 0 &&
                animator.GetInteger("JumpType") == 2)
            {

                animator.SetInteger("JumpType", 0);

                //착지 시 높이 판단.
                if ((string)PhotonNetwork.player.CustomProperties["PlayerType"] == "Mouse")
                {
                    if (PreFallPosition - transform.position.y >= AtLeastFallPosition)
                    {
                        // 데미지주기
                        playerHealth.CallFallDamage(FallDamage);

                        // 스턴 주기
                        ps.AddDebuffState(DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN, FallStunTime);

                        // 속도값 없애기
                        MoveDir = Vector3.up * MoveDir.y;
                    }
                }

            }









        }
    }

    Vector3 CheckMoveDash()
    {

        // 방향전환 선택
        if (!ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN) &&
            ps.GetWeaponType() == PlayerState.WeaponEnum.NONE)
        {
            return new Vector3(HSpeed, 0, VSpeed);
        }

        // 대쉬인 경우 직진(회전에서 방향처리)
        else
        {
            return Vector3.forward * MoveDir.magnitude;
        }
    }

    void PlayerMoveAnimation()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {

            // 키를 누를 수 있는 상태라면.
            if (isCanKey)
            {

                if (ps.isCanActive == true)
                {
                    //0. 입력에 따라서 Speed를 결정합니다.

                    if (HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal")) > -(AniSpeedUp) * Time.deltaTime &&
                     HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal")) < (AniSpeedUp) * Time.deltaTime)
                    {
                        HSpeed = 0.0f;
                    }

                    else
                    {
                        HSpeed = HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal"));
                    }

                    if (VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical")) > -(AniSpeedUp) * Time.deltaTime &&
                         VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical")) < (AniSpeedUp) * Time.deltaTime)
                    {
                        VSpeed = 0.0f;
                    }

                    else
                    {
                        VSpeed = VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical"));
                    }

                    // 1. 키 입력을 받습니다.
                    float HKey = Input.GetAxisRaw("Horizontal");
                    float VKey = Input.GetAxisRaw("Vertical");

                    // 2. 이동에 따라서 열거형을 설정합니다.
                    CalcKey_X(HSpeed, HKey);
                    CalcKey_Y(VSpeed, VKey);

                    //3. 배수가 있으면 열거형을 수정합니다.
                    HSpeed = CalcPlayerMoveMulti(SpeedMultiTypeX, SpeedLocationTypeX, HSpeed);
                    VSpeed = CalcPlayerMoveMulti(SpeedMultiTypeY, SpeedLocationTypeY, VSpeed);

                    //4. 특정 값 (1)이 넘어가지 못하도록 고정합니다.
                    HSpeed = CalcPlusMinus(HSpeed);
                    VSpeed = CalcPlusMinus(VSpeed);

                    animator.SetFloat("DirectionX", HSpeed);
                    animator.SetFloat("DirectionY", VSpeed);

                }
                else
                {
                    HSpeed = CalcPlusMinus(0);
                    VSpeed = CalcPlusMinus(0);

                    animator.SetFloat("DirectionX", 0);
                    animator.SetFloat("DirectionY", 0);
                }

            }
        }
    }


    void CheckResetInteraction()
    {
        if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION) &&
                        MoveDir != Vector3.zero)
        {

            // 플레이어 다시 이동으로 변경
            animator.SetInteger("InteractionType", 0);

            ResetSkill();


        }
    }

    void CheckResetRescue()
    {
        if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RESCUE) &&
            MoveDir != Vector3.zero)
        {

            RescueCancelEvent();


        }
    }

    // 플레이어가 마우스 x축으로 이동하는지에 대한 여부.`
    void SetPlayerRotateX()
    {




        if (!isCanUsePlayerRotateX)
            return;



        if (springArmObject.springArmType !=
           springArmObject.GetfreeSpringArm())
        {
            Vector3 v3 = transform.rotation.eulerAngles;

            // 캐릭터 회전값 받기
            PlayerRotateEuler += Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
            if (PlayerRotateEuler >= 360)
                PlayerRotateEuler -= 360;

            // 전력질주 상태
            if (ps.GetPlayerCondition() == PlayerState.ConditionEnum.SPEEDRUN ||
                ps.GetWeaponType() != PlayerState.WeaponEnum.NONE)
            {

                // 변수 초기화
                float atan2 = 0.0f;
                float YRotate = 0.0f;

                // 움직임 여부
                if (!(animator.GetFloat("DirectionX") == 0 && animator.GetFloat("DirectionY") == 0))
                {


                    // 애니메이션 float으로 각도 구하기
                    atan2 = Mathf.Atan2(animator.GetFloat("DirectionX"), animator.GetFloat("DirectionY")) * Mathf.Rad2Deg;

                    // 각도 적용한 YROTATE 생성
                    YRotate = PlayerRotateEuler + atan2;
                    PreWeaponYRotate = YRotate;
                }

                else if (animator.GetFloat("DirectionX") == 0 && animator.GetFloat("DirectionY") == 0)
                {
                    YRotate = PreWeaponYRotate;
                }

                    // 갱신값으로 회전
                    transform.rotation = Quaternion.Euler(v3.x, YRotate, v3.z);

            }

            // 전력질주 상태가 아닌 경우
            else
                transform.rotation = Quaternion.Euler(v3.x, PlayerRotateEuler, v3.z);


        }


    }


    void CalcKey_X(float aniKey, float key)
    {
        if (aniKey > 0)
        {
            if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
                SpeedMultiTypeX = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
                SpeedMultiTypeX = EnumSpeedMulti.NONE;
            }

            else if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
            }
        }

        else if (aniKey < 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
                SpeedMultiTypeX = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
                SpeedMultiTypeX = EnumSpeedMulti.NONE;
            }

            else if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
            }
        }
        else if (aniKey == 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
            }
            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
            }
            else if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
            }
        }
    }

    void CalcKey_Y(float aniKey, float key)
    {
        if (aniKey > 0)
        {
            if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
                SpeedMultiTypeY = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
                SpeedMultiTypeY = EnumSpeedMulti.NONE;
            }

            else if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
            }
        }

        else if (aniKey < 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
                SpeedMultiTypeY = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
                SpeedMultiTypeY = EnumSpeedMulti.NONE;
            }

            else if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
            }
        }
        else if (aniKey == 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
            }
            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
            }
            else if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
            }
        }
    }

    float SetMoveAnimation(float NowSpeed, float aniKey)
    {

        if (NowSpeed > 0)
        {
            if (aniKey < 0)
            {

                return -(AniSpeedUp) * Time.deltaTime;
            }

            else if (aniKey == 0)
            {

                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }

        else if (NowSpeed == 0)
        {
            if (aniKey < 0)
            {
                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey == 0)
            {
                return 0;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }

        else if (NowSpeed < 0)
        {
            if (aniKey < 0)
            {
                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey == 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }
        return 0.0f;
    }


    float CalcPlayerMoveMulti(EnumSpeedMulti esm, EnumSpeedLocation esl, float Speed)
    {
        if (esm == EnumSpeedMulti.MULTI)
        {
            if (esl == EnumSpeedLocation.PLUS)
            {
                Speed += AniSpeedUp * SpeedMulti * Time.deltaTime;
            }
            else if (esl == EnumSpeedLocation.MINUS)
            {
                Speed += -AniSpeedUp * SpeedMulti * Time.deltaTime;
            }
        }
        return Speed;
    }

    float CalcPlusMinus(float Speed)
    {
        if (Speed > 1.0f)
        {
            Speed = 1.0f;
        }

        else if (Speed < -1.0f)
        {
            Speed = -1.0f;
        }

        return Speed;
    }


    private void CheckResetCanKey()
    {

        // 입력 받을 수 없는상태
        // + 키입력이 없는 상태
        if ((Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) &&
            isCanKey == false)
        {
            isCanKey = true;
        }

    }

    // 애니메이션 스테이트 exit역할을 하던 것. 
    // exit 는 늦기 때문에 직접적으로 호출
    // interaction에서.
    public void ResetSkill()
    {
        if (photonView.isMine)
        {

            InteractiveState interactiveState = newInteractionSkill.GetinteractiveState();

            // 애니메이션 타입이고
            // 액션이 사용되지 않은 상태라면
            if (interactiveState.ActionType == InteractiveState.EnumAction.ANIMATION &&
                interactiveState.IsUseAction == false)
            {
                interactiveState.CallRPCCancelActionAnimation();
            }

            // FIndObject의 활성화 탐지 시작
            findObject.SetisUseFindObject(true);


            UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();


            float RotationY = springArmObject.springArmType.GetSpringArmRotationY();

            // 1. 카메라를 따라가는 상태로 변경.
            springArmObject.SwapSpringArm(SpringArmType.EnumSpringArm.FOLLOW);

            springArmObject.springArmType.SetSpringArmRotationY(RotationY);

            // 2. 플레이어의 회전값을 free값으로 변경
            SetPlayerRotateEuler(SpringArmObject.GetInstance().GetfreeSpringArm().GetSpringArmRotationX());

            // 추가. 0615
            springArmObject.springArmType.SetSpringArmRotationY(RotationY);



            // 모든 클라이언트에게 RPC전송 하는 함수 콜
            //interactiveState.SetisCanFirstCheck(true);
            // ** 마스터에서 처리하기에는 애니메이션 동기화 문제가 있어서 안됨
            interactiveState.CallOnCanFirstCheck();





            // 플레이어 강제 위치이동 여부 ex) 냉장고
            if (interactiveState.PlayerNextPositionType == InteractiveState.EnumPlayerNextPosition.USE &&
               interactiveState.IsUseAction == true)
            {
                gameObject.transform.position = interactiveState.NextPosition.transform.position;
            }

            newInteractionSkill.IsUseAction = false;

        }
    }

    public void ResetMoveSpeed()
    {
        Debug.Log("리셋사용");
        MoveDir = new Vector3 { x = 0, y = MoveDir.y, z = 0 };

    }


    public void IgnoreRotateX(bool CanUseRotate)
    {
        isCanUsePlayerRotateX = CanUseRotate;
    }

    public bool IsLandGround()
    {

        // 여기서 판단.

        RaycastHit hit;

        int LayerType = (LayerMask.NameToLayer("NoPlayerIntering")) | (LayerMask.NameToLayer("NoPlayerInterEnd"));
        LayerType = ~LayerType;




        if (Physics.SphereCast(gameObject.transform.position + Vector3.up * characterController.radius,
            characterController.radius,
            Vector3.down, 
            out hit, 
            GroundHieght))
        {

            Debug.DrawRay(gameObject.transform.position, hit.distance * Vector3.down, Color.green);
            return true;
        }

        else
        {
            Debug.DrawRay(gameObject.transform.position, Vector3.down * GroundHieght, Color.red);
            return false;
        }

    }


    // 최적화 필요, 하나의 스피어 캐스트사용해서 거리 이용해 판단하는 방법.
    public bool IsJumpGround()
    {

        

        RaycastHit hit;

        int LayerType = (LayerMask.NameToLayer("NoPlayerIntering")) | (LayerMask.NameToLayer("NoPlayerInterEnd"));
        LayerType = ~LayerType;




        if (Physics.SphereCast(gameObject.transform.position + Vector3.up * characterController.radius,
    characterController.radius,
    Vector3.down,
    out hit,
    JumpHeight))
        {

            Debug.DrawRay(gameObject.transform.position, hit.distance * Vector3.down, Color.green);
            return true;
        }

        else
        {
            Debug.DrawRay(gameObject.transform.position, Vector3.down * GroundHieght, Color.red);
            return false;
        }

    }

    public void CallRandomJumpSound()
    {

        playerSoundManager.PlayRandomEffectSound(
            SoundManager.EnumEffectSound.EFFECT_CHAR_JUMP_1,
            SoundManager.EnumEffectSound.EFFECT_CHAR_JUMP_3);
    }
}
