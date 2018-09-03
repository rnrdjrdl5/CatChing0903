using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectNode {

    public enum EnumUIEffectType { STANDARD, WAIT, FADEIN, FADEOUT, CUSTOM_EVENT, SCALE, SHAKEUPDOWN , SHAKERIGHTLEFT }
    private EnumUIEffectType UIEffectType;
    public void SetEnumUIEffectType(EnumUIEffectType et) { UIEffectType = et; }

    private GameObject TargetObject;

    private Vector2 FirstPosition;
    private Vector2 LastPosition;
    private Vector2 NowPosition;

    private float NowTime;
    private float MaxTime;
    public void SetMaxTime(float maxTime) { MaxTime = maxTime; }

    private float MinScale;
    private float MaxScale;
    public void SetMaxScale(float maxScale) { MaxScale = maxScale; }

    // Fade 효과 담당
    private Image FadeImage;

    // custom event 담당
    public delegate void DeleCustom();
    private DeleCustom CustomEvent;
    public void SetCustomEvent(DeleCustom deleCustom) { CustomEvent = deleCustom; }

    // Shake 효과 담당
    private int nowEffectCount;
    private int maxEffectCount;
    private int Weight;


    // 팩토리로 전환 요구됨
    public bool NodeUpdate()
    {
        bool isFinish = true;

        switch (UIEffectType)
        {
            case EnumUIEffectType.STANDARD:
                isFinish = StandardMoveEffect();
                break;

            case EnumUIEffectType.WAIT:
                isFinish = WaitEffect();
                break;

            case EnumUIEffectType.FADEOUT:
                isFinish = FadeOutEffect();
                break;

            case EnumUIEffectType.FADEIN:
                isFinish = FadeInEffect();
                break;

            case EnumUIEffectType.CUSTOM_EVENT:
                isFinish = CallCustomEvent();
                break;

            case EnumUIEffectType.SCALE:
                isFinish = ScaleEffect();
                break;

            case EnumUIEffectType.SHAKEUPDOWN:
                isFinish = ShakeEffect(ShakeType.UPDOWN);
                break;

            case EnumUIEffectType.SHAKERIGHTLEFT:
                isFinish = ShakeEffect(ShakeType.RIGHTLEFT);
                break;



        }


        return isFinish;
    }

    public void SetMoveData(GameObject gameObject, Vector2 firstPosition, Vector2 lastPosition, float maxTime)
    {
        TargetObject = gameObject;
        TargetObject.transform.position = new Vector3(firstPosition.x, firstPosition.y, 0);

        FirstPosition = firstPosition;
        LastPosition = lastPosition;
        NowPosition = FirstPosition;

        NowTime = 0;
        MaxTime = maxTime;

        UIEffectType = EnumUIEffectType.STANDARD;
    }

    public enum EnumFade { IN, OUT }
    
    public void SetFadeData(GameObject gameObject, float maxTime, EnumFade enumFade) 
    {

        TargetObject = gameObject;

        MaxTime = maxTime;
        NowTime = 0;

        if (enumFade == EnumFade.IN)
            UIEffectType = EnumUIEffectType.FADEIN;

        else
            UIEffectType = EnumUIEffectType.FADEOUT;
    }


    public void SetScaleData(GameObject gameobject, float _minScale, float _maxScale, float _maxTime)
    {

        if (gameobject == null)
        {
            Debug.LogWarning("에러");
            return;
        }

        MinScale = _minScale;
        MaxScale = _maxScale;

        TargetObject = gameobject;

        NowTime = 0.0f;
        MaxTime = _maxTime;



        UIEffectType = EnumUIEffectType.SCALE;


    }
    public void SetWaitData(float maxTime)
    {
        MaxTime = maxTime;
        NowTime = 0.0f;

        UIEffectType = EnumUIEffectType.WAIT;
    }

    public void SetShakeData(GameObject tObject, int _effectCount, int _weight, ShakeType st)
    {
        TargetObject = tObject;

        maxEffectCount = _effectCount;
        Weight = _weight;
        nowEffectCount = 0;
        FirstPosition = new Vector2(tObject.transform.localPosition.x, tObject.transform.localPosition.y);

        if (st == ShakeType.UPDOWN)
            UIEffectType = EnumUIEffectType.SHAKEUPDOWN;

        else if (st == ShakeType.RIGHTLEFT)
            UIEffectType = EnumUIEffectType.SHAKERIGHTLEFT;

    }


private bool StandardMoveEffect()
    {
        NowTime += Time.deltaTime;



        float persentTime = CalcTimePersent();

        if (persentTime >= 1)
        {
            NowPosition = Vector2.Lerp(FirstPosition, LastPosition, 1);
            return false;
        }


        NowPosition = Vector2.Lerp(FirstPosition, LastPosition, persentTime);


        TargetObject.transform.localPosition =
            new Vector3(NowPosition.x, NowPosition.y, 0);

        return true;

    }

    private bool WaitEffect()
    {
        NowTime += Time.deltaTime;

        if (NowTime >= MaxTime)
            return false;

        return true;
    }



    private bool FadeOutEffect()
    {
        
        InitFadeImage();


        NowTime += Time.deltaTime;
        if (NowTime >= MaxTime) {

            FadeImage.color = new Color(
                FadeImage.color.r,
                FadeImage.color.g,
                FadeImage.color.b,
                0);
            return false;
        }

        float AlphaColor = (1 - CalcTimePersent());

        FadeImage.color = new Color(
            FadeImage.color.r,
            FadeImage.color.g,
            FadeImage.color.b,
            AlphaColor);


        return true;
    }

    private bool FadeInEffect()
    {

        InitFadeImage();


        NowTime += Time.deltaTime;
        if (NowTime >= MaxTime)
        {
            FadeImage.color = new Color(
                  FadeImage.color.r,
                  FadeImage.color.g,
                  FadeImage.color.b,
                  1);
            return false;
        }

        float AlphaColor = (CalcTimePersent());


        FadeImage.color = new Color(
            FadeImage.color.r,
            FadeImage.color.g,
            FadeImage.color.b,
            AlphaColor);

        Debug.Log(FadeImage.color.a);
        Debug.Log(" Target : " + FadeImage.name);

        return true;
    }


    private bool CallCustomEvent()
    {
        CustomEvent();

        return false;
    }




    private float CalcTimePersent()
    {
        if (NowTime == 0)
            return 0;

        return NowTime / MaxTime;
    }

    private void InitFadeImage()
    {
        if (FadeImage == null)
            FadeImage = TargetObject.GetComponent<Image>();
    }

    private bool ScaleEffect()
    {

        float TimePer = CalcTimePersent();
        if (TimePer >= 1)
            return false;

        TargetObject.transform.localScale =
            Mathf.Lerp(MinScale, MaxScale, TimePer) * Vector3.one / 100;

        NowTime += Time.deltaTime;

        return true;

            
    }

    public enum ShakeType { UPDOWN, RIGHTLEFT };

    private bool ShakeEffect(ShakeType st)
    {

        if (nowEffectCount >= maxEffectCount)
        {
            TargetObject.transform.localPosition = new Vector3(FirstPosition.x, FirstPosition.y, 0);
            return false;
        }

        int tempCount = nowEffectCount % 2;

        Vector2 tempVector = Vector2.zero;
        tempVector = FirstPosition;

        Vector2 DirVector;


        if (st == ShakeType.UPDOWN)  DirVector = Vector2.up;
        else DirVector = Vector2.right;
       

        if (tempCount == 0)tempVector = FirstPosition + DirVector * Weight;
        else tempVector = FirstPosition + -DirVector * Weight;


        TargetObject.transform.localPosition = new Vector3(tempVector.x, tempVector.y, 0);

        nowEffectCount++;

        return true;
    }


}
