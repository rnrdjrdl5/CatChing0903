using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffPanel : MonoBehaviour
{

    private GameObject CartoonLine;

    private RectTransform RTF;

    private GameObject TurnOffLighting;
    private GameObject TurnOffBipIcon;

    private bool isUseTurnOff = false;
    public bool GetisUseTurnOff() { return isUseTurnOff; }
    public void SetisUseTurnOff(bool UT)
    {
        isUseTurnOff = UT;
    }


    private float TurnOffTime = 3;

    private float ImageFadeInTime = 0.2f;

    private Vector3 cartoonPosition;

    IEnumerator CoroCutScene;


    public float GetTurnOffTime() { return TurnOffTime; }
    public void SetTurnOffTime(float tot) { TurnOffTime = tot; }

    public float GetImageFadeInTime() { return ImageFadeInTime; }
    public void SetImageFadeInTime(float IFIT) { ImageFadeInTime = IFIT; }




    private void Start()
    {
        
       // StartCutScene();
    }


    private void Update()
    {
        if (isUseTurnOff)
        {
            TurnOffTime -= Time.deltaTime;
            if (TurnOffTime <= 0)
            {
                gameObject.SetActive(false);
                isUseTurnOff = false;
            }
        }
    }


    public void StartCutScene()
    {

        TurnOffPanelScript turnOffPanelScript = UIManager.GetInstance().turnOffPanelScript;

        CartoonLine = turnOffPanelScript.CartoonLine;

        RTF = CartoonLine.GetComponent<RectTransform>();

        TurnOffLighting = turnOffPanelScript.TurnOffLighting;
        TurnOffBipIcon = turnOffPanelScript.TurnOffBipIcon;

        CoroCutScene = CutScene();
        StartCoroutine(CoroCutScene);
    }


    public enum EnumCutScene { MOVE, WAIT, BACKMOVE, SMALLBACKMOVE };

    

    IEnumerator CutScene()
    {



        float Width = RTF.rect.width;

        EnumCutScene CutSceneType = EnumCutScene.MOVE;

        cartoonPosition = new Vector3(
            -Width,
            0,
            0);

        float NowTime = 0.0f;

        while (true)
        {
            Debug.Log("돌아간다");

            if (CutSceneType == EnumCutScene.MOVE)
            {


                // 1. 이미지의 다음값을 선정
                NowTime += 1 / ImageFadeInTime * Time.deltaTime;

                float NowWidth = Mathf.Lerp(Width, 0, NowTime);

                cartoonPosition.x = -NowWidth;

                CartoonLine.transform.localPosition = cartoonPosition;

                if (NowTime >= 1.0f)
                {
                    CoroCutScene = SmallBackMove();
                    StartCoroutine(CoroCutScene);
                    yield break;
                }

                else
                    yield return null;
            }
            yield return null;
        }
    }

    IEnumerator SmallBackMove()
    {

        // 2. 거리도 기존의 1/10만 이동.
        float Width = RTF.rect.width;

        // 3. 벡터 설정
        cartoonPosition = new Vector3(0,0, 0);

        // 4. 현재시간
        float NowTime = 0.0f;

        while (true)
        {
            // 다음 시간
            NowTime += 1 / ImageFadeInTime * Time.deltaTime;

            // 다음위치
            float NowWidth = Mathf.Lerp(0, Width, NowTime);

            cartoonPosition.x = -NowWidth;

            CartoonLine.transform.localPosition = cartoonPosition;

            if (NowTime >= 0.1f)
            {
                CoroCutScene = WaitTime();
                StartCoroutine(CoroCutScene);
                yield break;
            }

            yield return null;
        }




    }

    IEnumerator WaitTime()
    {
        SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.EFFECT_CAT_BUTTONCLICK1);
        TurnOffBipIcon.SetActive(true);

        int i = 0;
        while (true)
        {
            if (TurnOffLighting.GetActive() == true)
            {
                TurnOffLighting.SetActive(false);
            }
            else if(TurnOffLighting.GetActive() == false)
            {
                TurnOffLighting.SetActive(true);

                SpringArmObject.GetInstance().GetSystemSoundManager().PlayEffectSound(SoundManager.EnumEffectSound.EFFECT_CAT_BUTTONBEEP);
            }


            if (i < 4)
            {

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                TurnOffBipIcon.SetActive(false);
                CoroCutScene = BackMove();
                StartCoroutine(CoroCutScene);
                yield break;
            }

            i++;
        }

    }

    IEnumerator BackMove()
    {

        float Width = RTF.rect.width;

        float NowTime = 0.0f;

        float NowPosition = CartoonLine.transform.localPosition.x;

        cartoonPosition = new Vector3(
                  -NowPosition,
                  0,
                0);


        
        while (true)
        {


            // 1. 이미지의 다음값을 선정
            NowTime += 1 / ImageFadeInTime * Time.deltaTime;



            float NowWidth = Mathf.Lerp(NowPosition, -Width, NowTime);

            cartoonPosition.x = NowWidth;

            CartoonLine.transform.localPosition = cartoonPosition;

            if (NowTime >= 1.0f)
            {
                yield break;
            }

            else
                yield return null;
        }
    }

}
