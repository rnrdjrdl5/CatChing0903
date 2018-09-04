using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class TitleLogoManager : MonoBehaviour {

    private GameObject UICanvas;

    private GameObject CKLogoPanel;
    private GameObject FadePanel;

    private GameObject FadeImage;
    private Image FadeImageImage;

    public float FadeTime = 1.0f;
    public float FadeWait = 1.5f;

    public string LobbyName;

    private void Awake()
    {

        UICanvas = GameObject.Find("UICanvas").gameObject;

        CKLogoPanel = UICanvas.transform.Find("CKLogoPanel").gameObject;
        FadePanel = UICanvas.transform.Find("FadePanel").gameObject;

        FadeImage = FadePanel.transform.Find("FadeImage").gameObject;
        FadeImageImage = FadeImage.GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine("StartLogo");
    }

    IEnumerator StartLogo()
    {
        Debug.Log("실행");
        StartCoroutine("FadeOutEffect", FadeTime);
        yield return new WaitForSeconds(FadeTime + FadeWait);

        StartCoroutine("FadeInEffect", FadeTime);
        yield return new WaitForSeconds(FadeTime + FadeWait);

        SceneManager.LoadScene(LobbyName);
    }

    IEnumerator FadeInEffect(float time)
    {
        float alpha = 0;

        while (true)
        {

            if (alpha > 1)
            {
                SetColor(FadeImageImage,
                FadeImageImage.color.r,
                FadeImageImage.color.g,
                FadeImageImage.color.b,
                1);

                yield break;
            }

            float nowTime = Time.deltaTime / time;

            alpha += nowTime;

            SetColor(FadeImageImage,
            FadeImageImage.color.r,
            FadeImageImage.color.g,
            FadeImageImage.color.b,
            alpha);

            yield return new WaitForSeconds(nowTime);
        }
    }

    IEnumerator FadeOutEffect(float time)
    {
        float alpha = 1;

        while (true)
        {

            if (alpha < 0)
            {
                SetColor(FadeImageImage,
                FadeImageImage.color.r,
                FadeImageImage.color.g,
                FadeImageImage.color.b,
                0);

                yield break;
            }

            float nowTime = Time.deltaTime * time;

            alpha -= nowTime;

            SetColor(FadeImageImage,
            FadeImageImage.color.r,
            FadeImageImage.color.g,
            FadeImageImage.color.b,
            alpha);

            yield return new WaitForSeconds(nowTime);
        }
    }


    void SetColor(Image image, float r, float b, float g, float a)
    {
        image.color = new Color(r, g, b, a);
    }
}
