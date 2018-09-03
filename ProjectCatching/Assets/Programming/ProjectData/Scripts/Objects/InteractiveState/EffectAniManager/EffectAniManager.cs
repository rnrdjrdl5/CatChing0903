using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 애니메이션 이벤트 시 함수 사용하기 위해 
// 스크립트 작성.
public class EffectAniManager : MonoBehaviour
{

    public GameObject InterEffect;

    public List<GameObject> UseEffectList { get; set; }

    public GameObject UseEffect;

    public void Awake()
    {
        UseEffectList = new List<GameObject>();
    }

    bool SetPlayerIsMine()
    {
        InteractiveState interactiveState = GetComponent<InteractiveState>();
        if (interactiveState == null) return false;

        NewInteractionSkill newInteractionSkill = interactiveState.GetNewInteractiveSkill();
        if (newInteractionSkill == null) return false;

        return true;
    }

    public void BigFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.BIGDUST_BIG);
        go.transform.position = InterEffect.transform.position;

        go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.BIGDUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }

    public void MiddleFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_BIG);
        go.transform.position = InterEffect.transform.position;

        go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }

    public void SmallFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.SMALL_DUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }

    public void PotCrashEffect()
    {

        GameObject go = PoolingManager.GetInstance().CreateEffectCameraShake(PoolingManager.EffctType.POT_EFFECT,
            SetPlayerIsMine());

        go.transform.position = InterEffect.transform.position;
    }

    public void FrameBreakEffect()
    {

        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.FRAME_EFFECT);
        go.transform.position = InterEffect.transform.position;
    }


    public void CartChargeEffect()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.FIRST_CARTCHARGE);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;

        UseEffectList.Add(go);
    }

    public void PosSpreadEffect()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.FIRST_POS_SPREAD);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.Rotate(Vector3.up, 180.0f, Space.Self);

        UseEffectList.Add(go);
    }

    public void DrawBreak()
    {
        UseEffectList.Clear();

        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.FIRST_DRAWEBREAK);
        go.transform.position = UseEffect.transform.position;
        go.transform.Rotate(Vector3.right, -90.0f, Space.Self);

        UseEffectList.Add(go);
    }

    public void MikeSpotEffect()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIKE_SPOT_EFFECT);
        go.transform.position = transform.position;

        UseEffectList.Add(go);
    }

    public void PianoRhythmEffect()
    {

        UseEffectList.Clear();

        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.PIANO_RHYTHM_EFFECT);
        go.transform.position = transform.position;

        UseEffectList.Add(go);
    }

    public void DrawerEffect()
    {

        UseEffectList.Clear();

        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.DRAWER_EFFECT);
        go.transform.position = InterEffect.transform.position;

        Quaternion q = go.transform.rotation;

        go.transform.SetParent(InterEffect.transform);
        
        go.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        UseEffectList.Add(go);
    }

    public void DrawerEffectTrailPlayer()
    {

        GameObject go2 = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.DRAWER_TRAIL_EFFECT);
        go2.transform.SetParent(transform);
        go2.transform.localRotation = Quaternion.identity;
        go2.transform.localPosition = Vector3.zero;

        go2.transform.position = transform.position;
        go2.transform.SetParent(transform);
        go2.transform.localScale = Vector3.one;

        UseEffectList.Add(go2);

    }




    // 플레이어 해제용
    public void DestroyEffects()
    {
        for (int i = UseEffectList.Count - 1; i >= 0; i--)
        {
            if (UseEffectList[i].activeInHierarchy)
                PoolingManager.GetInstance().PushObject(UseEffectList[i]);
        }

        
    }

    public void DestroyFirstEffect()
    {
        for (int i = UseEffectList.Count - 1; i >= 0; i--)
        {
            if (UseEffectList[i].activeInHierarchy)
                PoolingManager.GetInstance().PushObject(UseEffectList[i]);
        }
    }





}
