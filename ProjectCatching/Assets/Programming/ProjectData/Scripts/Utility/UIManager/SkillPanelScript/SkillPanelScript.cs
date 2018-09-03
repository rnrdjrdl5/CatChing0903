using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelScript {

    private int MaxSkillMount = 4;

    public GameObject SkillPanel { get; set; }
    public void InitSkillPanel()
    {
        SkillPanel = UIManager.GetInstance().UICanvas.transform.Find("SkillPanel").gameObject;
    }

    public GameObject[] SkillImage { get; set; }
    public Image[] SkillImageImage { get; set; }
    public void InitSkillImage()
    {
        SkillImage = new GameObject[MaxSkillMount];
        SkillImageImage = new Image[MaxSkillMount];


        for (int i = 0; i < SkillImage.Length; i++)
        {
            SkillImage[i] = SkillPanel.transform.Find("Skill" + (i + 1).ToString() + "Image").gameObject;
            SkillImageImage[i] = SkillImage[i].GetComponent<Image>();
        }
    }



    public GameObject[] SkillCoolTime { get; set; }
    public Image[] SkillCoolTimeImage { get; set; }
    public void InitSkillCoolTime()
    {

        SkillCoolTime = new GameObject[MaxSkillMount];
        SkillCoolTimeImage = new Image[MaxSkillMount];

        for (int i = 0; i < SkillImage.Length; i++)
        {
            SkillCoolTime[i] = SkillPanel.transform.Find("Skill" + (i + 1).ToString() + "CoolTime").gameObject;
            SkillCoolTimeImage[i] = SkillCoolTime[i].GetComponent<Image>();
        }
    }

    public GameObject[] SkillUseImage { get; set; }
    public Image[] SkillUseImageImage { get; set; }
    public void InitSkillUseImage()
    {

        SkillUseImage = new GameObject[MaxSkillMount];
        SkillUseImageImage = new Image[MaxSkillMount];

        for (int i = 0; i < SkillImage.Length; i++)
        {
            SkillUseImage[i] = SkillPanel.transform.Find("Skill" + (i + 1).ToString() + "UseImage").gameObject;
            SkillUseImageImage[i] = SkillUseImage[i].GetComponent<Image>();
        }
    }

    public GameObject[] SkillKeyIcon { get; set; }
    public Image[] SkilKeyIconImage { get; set; }
    public void InitSkillKeyIcon()
    {
        SkillKeyIcon = new GameObject[MaxSkillMount];
        SkilKeyIconImage = new Image[MaxSkillMount];

        for (int i = 0; i < SkillImage.Length; i++)
        {
            SkillKeyIcon[i] = SkillPanel.transform.Find("Skill" + (i + 1).ToString() + "KeyIcon").gameObject;
            SkilKeyIconImage[i] = SkillKeyIcon[i].GetComponent<Image>();
        }
    }

    public void InitData()
    {
        InitSkillPanel();
        InitSkillImage();
        InitSkillCoolTime();
        InitSkillUseImage();
        InitSkillKeyIcon();
    }

    public void ChangeImage(Sprite image , int number)
    {
        SkillImageImage[number].sprite = image;
    }

    public void ChangeCooltimeImage(Sprite image, int number)
    {
        SkillCoolTimeImage[number].sprite = image;
    }

    // 콜백사용
    public void UpdateSkillCoolTimeImage(float NowTime, float MaxTime, int SkillNumber)
    {
          SkillCoolTimeImage[SkillNumber].fillAmount = NowTime / MaxTime;
    }

    public void UpdateNotUseSkillImage(int SkillNumber)
    {
        SkillUseImage[SkillNumber].SetActive(false);
    }

    public void UpdateUseSkillImage(int SkillNumber)
    {
        SkillUseImage[SkillNumber].SetActive(true);
    }



    // 1. 이미지 변경

    // 2. 스킬 쿨타임 보여주기?
}
