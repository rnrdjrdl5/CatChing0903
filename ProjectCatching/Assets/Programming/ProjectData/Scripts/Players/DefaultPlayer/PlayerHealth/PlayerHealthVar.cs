using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public partial class PlayerHealth
{
    /**** public ****/
    public SkinnedMeshRenderer[] skinnedMeshRenderer;
    public float MaxHiting = 0.3f;

    public float RopeDeadAnimation = 2.0f;      // 구조 첫 죽음 시간

    public float MaxRopeDeadTime = 10.0f;       // 구조시간

    public float PlayerFinalDeadtime = 3.0f;        // 죽을 때 애니메이션 후 얼마 뒤에 죽을 지

    public delegate void DeleDecreaseDead(float nowData, float maxData);
    /**** private ****/

    private PlayerState playerState;

    private Animator animator;

    private GameObject UICanvas;            // UI 캔버스
    private GameObject HPPanel;            // HP 오브젝트
    private Image NowHPImage;               // HP 이미지

    private float MaxHealth = 100.0f;   // 최대체력
    private float NowHealth = 100.0f;   // 현재체력

    private bool isHiting;
    private float NowHiting;

    private PhotonManager photonManager;

    private float NowRopeDeadTime;
    private bool isUseRopeDead = false;

    private PlayerBodyPart playerBodyPart;

    public DeleDecreaseDead DecreaseDeadEvent;

    public void CallDecreaseDeadEvent()
    {
        DecreaseDeadEvent(NowRopeDeadTime, MaxRopeDeadTime);
    }





    /**** 접근자 ****/

    public void SetMaxHealth(float MH) { MaxHealth = MH; }
    public void SetNowHealth(float NH) { NowHealth = NH; }
    public void SetNowRopeDeadTime(float NRDT)
    {
        NowRopeDeadTime = NRDT;
    }
    public void SetisUseRopeDead(bool URD) { isUseRopeDead = URD; }

    public float GetMaxHealth() { return MaxHealth; }
    public float GetNowHealth() { return NowHealth; }
    public float GetNowRopeDeadTime() { return NowRopeDeadTime; }
    public bool GetisUseRopeDead() { return isUseRopeDead; }

    public void ResetNowRopeDeadTime()
    {
        NowRopeDeadTime = MaxRopeDeadTime;
    }





}
