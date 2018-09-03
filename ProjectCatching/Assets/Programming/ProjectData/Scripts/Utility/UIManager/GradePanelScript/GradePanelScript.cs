using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradePanelScript{

    public int ShakeFrequency;
    public int ShakePower;

    public GameObject GradePanel { set; get; }
    public void InitGradePanel() { GradePanel = UIManager.GetInstance().UICanvas.transform.Find("GradePanel").gameObject; }


    public GameObject GradeBarImage { get; set; }
    public void InitGradeBarImage() { GradeBarImage = GradePanel.transform.Find("GradeBarImage").gameObject; }

    public Image GradeBarImageImage { get; set; }
    public void InitGradeBarImageImage(){GradeBarImageImage = GradeBarImage.GetComponent<Image>();}

    
    public RectTransform GradeBarImageRect { get; set; }
    public void InitGradeBarImageRect() { GradeBarImageRect = GradeBarImage.GetComponent<RectTransform>(); }


    public GameObject GradeBarBackImage { get; set; }
    public void InitGradeBarBackImage() { GradeBarBackImage = GradePanel.transform.Find("GradeBarBackImage").gameObject; }


    public GameObject GradeHouseImage { get; set; }
    public void InitGradeHouseImage() { GradeHouseImage = GradePanel.transform.Find("GradeHouseImage").gameObject; }


    public GameObject GradeArrow { get; set; }
    public void InitGradeArrow() { GradeArrow = GradePanel.transform.Find("GradeArrow").gameObject; }

    private UIEffect uIEffect;


    public void InitData()
    {
        InitGradePanel();

        InitGradeBarImage();
        InitGradeBarImageImage();
        InitGradeBarImageRect();

        InitGradeBarBackImage();

        InitGradeHouseImage();

        InitGradeArrow();


        UIManager uIManager = UIManager.GetInstance();

        uIEffect = new UIEffect();
        ShakeFrequency = uIManager.HPPanel_ShakeFrequency;
        ShakePower = uIManager.HPPanel_ShakePower;

        uIManager.UpdateEvent += UpdateGrade;

    }

    public void InitEvent()
    {
        ObjectManager.GetInstance().RemoveEvent += ShakeRestaurantEvent;
    }

    public void SetActiveObjects(bool isActive)
    {
        GradeBarImage.SetActive(isActive);

        GradeBarBackImage.SetActive(isActive);

        GradeHouseImage.SetActive(isActive);

        GradeArrow.SetActive(isActive);
    }


    void UpdateGrade()
    {
        int CatGradeScore = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Cat")
            {
                CatGradeScore = (int)PhotonNetwork.playerList[i].CustomProperties["CatScore"];
                break;
            }
        }

        float GradePersent = (float)CatGradeScore / (float)PhotonManager.GetInstance().MaxCatScore;

        GradeBarImageImage.fillAmount = GradePersent;


        float GradeRootYPos = GradeBarImage.transform.localPosition.y -
            GradeBarImageRect.rect.height / 2;


        float GradeArrowYPos = GradeRootYPos + GradePersent * GradeBarImageRect.rect.height;

        GradeArrow.transform.localPosition = new Vector3(
            GradeArrow.transform.localPosition.x,
            GradeArrowYPos,
            GradeArrow.transform.localPosition.z);


       



    }

    private void ShakeRestaurantEvent()
    {
        Debug.Log("이벤트작동");
        uIEffect.CheckResetUIEffect(GradePanel);
        uIEffect.originalLocalPosition = GradePanel.transform.localPosition;
        uIEffect.AddShakeEffectNode(GradePanel, ShakeFrequency, ShakePower, UIEffectNode.ShakeType.RIGHTLEFT);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;
    }
    
}
