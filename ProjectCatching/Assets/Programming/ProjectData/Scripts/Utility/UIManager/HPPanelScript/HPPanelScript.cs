using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPPanelScript
{

    public int ShakeFrequency;
    public int ShakePower;

    /**** HP ****/

    public GameObject HPPanel { set; get; }
    public void InitHPPanel() { HPPanel = UIManager.GetInstance().UICanvas.transform.Find("HPPanel").gameObject; }

    public GameObject NowHPImage { set; get; }
    public Image NowHPImageImage { get; set; }

    public GameObject DieHPImage { get; set; }
    public Image DieHPImageImage { get; set; }

    public GameObject MaxHPImage { get; set; }

    public GameObject NowHPText { get; set; }
    public Text NowHPTextText { get; set; }
    public GameObject HPPersentImage { get; set; }
    public GameObject HPInfinityImage { get; set; }

    private PlayerHealth playerHealth;

    private UIEffect uIEffect;

    // playerhealth에서 사용함.
    public void SetPlayerHealth(GameObject go)
    {
        playerHealth = go.GetComponent<PlayerHealth>();

        // PlayerHelath에 다시 등록함.
        playerHealth.HealthEvent += new PlayerHealth.HealthDele(HPEvent);

        playerHealth.DecreaseDeadEvent = DieHPEvent;
    }


    public void InitNowMouseHPImage()
    {
        NowHPImage = HPPanel.transform.Find("NowHPImage").gameObject;
        NowHPImageImage = NowHPImage.GetComponent<Image>();
    }

    public void InitDieMouseHPImage()
    {
        DieHPImage = HPPanel.transform.Find("DieHPImage").gameObject;
        DieHPImageImage = DieHPImage.GetComponent<Image>();
    }

    public void InitMaxMouseHPImage()
    {
        MaxHPImage = HPPanel.transform.Find("MaxHPImage").gameObject;
    }

    public void InitNowHPText()
    {
        NowHPText = HPPanel.transform.Find("NowHPText").gameObject;
        NowHPTextText = NowHPText.GetComponent<Text>();

        HPPersentImage = HPPanel.transform.Find("HPPersentImage").gameObject;
        HPInfinityImage = HPPanel.transform.Find("HPInfinityImage").gameObject;

    }

    public void SetHealthPoint(bool isActive)
    {
        HPPanel.SetActive(isActive);
        NowHPImage.SetActive(isActive);
        MaxHPImage.SetActive(isActive);

        HPPersentImage.SetActive(isActive);

        string playerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        if (playerType == "Mouse" || 
            playerType == "Rope" ||
            playerType == "Dead")
        {
            NowHPText.SetActive(isActive);
            HPInfinityImage.SetActive(false);
        }

        else if (playerType == "Cat")
        {
            NowHPText.SetActive(false);
            HPInfinityImage.SetActive(isActive);
        }
    }

    public void InitData()
    {
        InitHPPanel();
        InitNowMouseHPImage();
        InitDieMouseHPImage();
        InitMaxMouseHPImage();

        InitNowHPText();

        ShakePower = UIManager.GetInstance().HPPanel_ShakePower;
        ShakeFrequency = UIManager.GetInstance().HPPanel_ShakeFrequency;

        uIEffect = new UIEffect();

        
    }

    public void HPEvent()
    {
        if (HPPanel.activeInHierarchy == false)
            return;

        NowHPImageImage.fillAmount =
                playerHealth.GetNowHealth() / playerHealth.GetMaxHealth();

        int Persent = (int)(NowHPImageImage.fillAmount * 100);

            NowHPTextText.text = Persent.ToString();

        // 첫 등록했을때는? 어떻게? check

        uIEffect.CheckResetUIEffect(HPPanel);
        uIEffect.originalLocalPosition = HPPanel.transform.localPosition;
        uIEffect.AddShakeEffectNode(HPPanel, ShakeFrequency, ShakePower , UIEffectNode.ShakeType.UPDOWN);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;



    }

    public void DieHPEvent(float nowData, float maxData)
    {

        if (HPPanel.activeInHierarchy == false)
            return;

        DieHPImageImage.fillAmount =
            nowData / maxData;


    }


    public void SetAliveActive(bool active)
    {
        NowHPImage.SetActive(active);
        DieHPImage.SetActive(!active);
    }

}
