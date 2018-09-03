using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageScript {

    public GameObject FadeImage { get; set; }
    private void InitFadeImage()
    { FadeImage = UIManager.GetInstance().ResultUI.transform.Find("FadeImage").gameObject; }

    public Image FadeImageImage { get; set; }
    private void InitFadeImageImage()
    { FadeImageImage = FadeImage.GetComponent<Image>(); }

    public void InitData()
    {

        InitFadeImage();
        InitFadeImageImage();
    }

    public void SetAlpha(float alpha)
    {

        FadeImageImage.color = new Color(
            FadeImageImage.color.r,
            FadeImageImage.color.g,
            FadeImageImage.color.b,
            alpha);
    }


}
