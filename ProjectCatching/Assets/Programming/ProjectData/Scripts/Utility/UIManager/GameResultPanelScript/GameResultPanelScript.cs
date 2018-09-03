using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameResultPanelScript {

    private const float STANDARD_MOVE = 0.5f;








    public UIEffect uIEffect;


    public GameObject ResultUI { get; set; }
    public void InitResultUI() { ResultUI = UIManager.GetInstance().ResultUI.gameObject; }

    public GameObject GameResultPanel { get; set; }
    public void InitGameResultPanel()
    {
        GameResultPanel = ResultUI.transform.Find("GameResultPanel").gameObject;
    }

    public GameObject CatWinImage { get; set; }
    public void InitCatWinImage()
    {
        CatWinImage = GameResultPanel.transform.Find("CatWinImage").gameObject;
    }

    public GameObject MouseWinImage { get; set; }
    public void InitMouseWinImage()
    {
        MouseWinImage = GameResultPanel.transform.Find("MouseWinImage").gameObject;
    }

    public void InitData()
    {
        uIEffect = new UIEffect();

        InitResultUI();

        InitGameResultPanel();

        InitCatWinImage();
        InitMouseWinImage();

    }

    // 스타트에서 실행
    public void InitEvent()
    {
        PhotonManager.GetInstance().GameFinishEvent = SetResult;
    }

    private void SetResult(int type)
    {

        CatWinImage.SetActive(false);
        MouseWinImage.SetActive(false);


        Vector2 MoveDir = Vector2.zero;
        if (type == 0)
        {
            CatWinImage.SetActive(true);
            MoveDir = Vector2.right * -GameResultPanel.GetComponent<RectTransform>().rect.width;
        }

        else if (type == 1)
        {
            MouseWinImage.SetActive(true);
            MoveDir = Vector2.right * GameResultPanel.GetComponent<RectTransform>().rect.width;
        }






        uIEffect.AddMoveEffectNode(GameResultPanel,
            MoveDir,
            Vector2.zero, STANDARD_MOVE);

        UIManager.GetInstance().UpdateEvent += uIEffect.EffectEvent;



    }

}
