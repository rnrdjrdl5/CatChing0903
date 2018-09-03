using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadOutLinePanelScript{

    public GameObject DeadOutLinePanel { get; set; }
    public void InitDeadOutLinePanel(){DeadOutLinePanel = UIManager.GetInstance().UICanvas.transform.Find("DeadOutLinePanel").gameObject; }

    public void InitData()
    {

        InitDeadOutLinePanel();
    }

}
