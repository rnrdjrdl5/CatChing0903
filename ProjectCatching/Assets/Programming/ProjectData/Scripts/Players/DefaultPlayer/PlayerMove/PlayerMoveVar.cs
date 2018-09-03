using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{

    /**** enum ****/

    public enum EnumSpeedLocation { PLUS, NONE, MINUS };                // 애니메이션 블렌딩
    public enum EnumSpeedMulti { MULTI, NONE };             // 애니메이션 블렌딩

    public delegate void DeleRescueCancel();
    public DeleRescueCancel RescueCancelEvent;

    /**** public ****/


    private SpringArmObject springArmObject;

    [Header(" 이동속도")]
    public float PlayerSpeed = 10.0f;               // 캐릭터 이동속도

    [Header("뒤 이동속도")]
    public float PlayerBackSpeed = 5.0f;                // 카메라 뒤로 이동

    [Header("밧줄 이동속도")]
    public float PlayerRopeSpeed = 2.5f;                // 카메라 뒤로 이동

    [Header("카메라 회전속도")]
    public float RotationSpeed = 100.0f;             // 캐릭터 회전속도

    [Header("점프 값 ")]
    [Tooltip(" 점프 속도에 따른 수치.")]
    public float JumpSpeed = 10.0f;

    [Header("중력값")]
    [Tooltip(" 중력수치")]
    public float gravity = 20;

    [Header("데미지 받는 최소 높이")]
    public float AtLeastFallPosition;

    [Header("추락 데미지")]
    public float FallDamage;

    [Header("추락 스턴 지속시간")]
    public float FallStunTime;

    public float GroundHieght = 0.5f;
    public float JumpHeight = 0.15f;


    /**** private ****/


    private Animator animator;
    private PlayerState ps;
    private CharacterController characterController;                // 캐릭터 컨트롤러
    private NewInteractionSkill newInteractionSkill;            // 상호작용
    private FindObject findObject;                      // 탐지
    private PlayerHealth playerHealth;
    private SoundManager playerSoundManager;            // 소리용


    public Vector3 MoveDir = Vector3.zero;             // 플레이어 이동속도

    private bool isCanKey = true;               // 키 누를 수 있는지 여부
    private bool isCanUsePlayerRotateX = true;      // 입력가능여부

    private float OriginalPlayerSpeed = 0.0f;               // SpeedRun) 일반 이동속도 저장함
    private float HSpeed = 0;               // 플레이어 가로 속도
    private float VSpeed = 0;               // 플레이어 세로 속도
    private float PlayerRotateEuler = 0;                // 플레이어 오일러 회전값 , 플레이어 회전 담당
    private float SpeedMulti = 1.5f;                  // 애니메이션 배수
    private float AniSpeedUp = 3.0f;                     // 애니메이션 속도

    private float PreFallPosition;              // 플레이어 점프 이전 위치.
    private float PreWeaponYRotate;

    


    EnumSpeedLocation SpeedLocationTypeX = EnumSpeedLocation.NONE;              // 애니메이션 블렌딩
    EnumSpeedLocation SpeedLocationTypeY = EnumSpeedLocation.NONE;              // 애니메이션 블렌딩
    EnumSpeedMulti SpeedMultiTypeX = EnumSpeedMulti.NONE;               // 애니메이션 블렌딩
    EnumSpeedMulti SpeedMultiTypeY = EnumSpeedMulti.NONE;               // 애니메이션 블렌딩


    /**** 접근자 ****/


    public float GetOriginalPlayerSpeed() { return OriginalPlayerSpeed; }
    public void SetOriginalPlayerSpeed(float SPS) { OriginalPlayerSpeed = SPS; }

    public float GetHSpeed() { return HSpeed; }
    public void SetHSpeed(float HS) { HSpeed = HS; }

    public float GetVSpeed() { return VSpeed; }
    public void SetVSpeed(float VS) { VSpeed = VS; }

    public float GetPlayerRotateEuler() { return PlayerRotateEuler; }
    public void SetPlayerRotateEuler(float RPE) { PlayerRotateEuler = RPE; }

    public Vector3 GetMoveDir() { return MoveDir; }
    public void SetMoveDir(Vector3 md) { md = MoveDir; }


    public bool GetisCanKey() { return isCanKey; }
    public void SetisCanKey(bool ck) { isCanKey = ck; }

    public void SetAniSpeedUp(float ASU) { AniSpeedUp = ASU; }
    public float GetAniSpeedUp() { return AniSpeedUp; }

    public void SetisCanUsePlayerRotateX(bool ICUPR)
    {
        isCanUsePlayerRotateX = ICUPR;
    }
    public bool GetisCanUsePlayerRotateX() { return isCanUsePlayerRotateX; }

    public float GetPreWeaponYRotate() { return PreWeaponYRotate; }
    public void SetPreWeaponYRotate(float PWYR)
    {
        PreWeaponYRotate = PWYR;
    }
}
