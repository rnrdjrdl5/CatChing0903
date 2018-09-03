using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinVideoPanelScript : MonoBehaviour {


    public GameObject ResultUI { get; set; }
    public void InitResultUI() { ResultUI = UIManager.GetInstance().ResultUI.gameObject; }

}
