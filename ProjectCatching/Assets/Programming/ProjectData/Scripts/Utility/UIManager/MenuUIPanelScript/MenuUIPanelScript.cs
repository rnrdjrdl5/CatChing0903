using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIPanelScript {

    public GameObject MenuBookUI { get; set; }
    private void InitMenuBookUI() {

        MenuBookUI = GameObject.Find("MenuBookUI").gameObject;

    }

    public GameObject MenuBookCamera { get; set; }
    private void InitMenuBookCamera() { MenuBookCamera = MenuBookUI.transform.Find("MenuBookCamera").gameObject; }

    public GameObject MenuBookObject { get; set; }
    private void InitMenuBookObject() { MenuBookObject = MenuBookUI.transform.Find("MenuBookObject").gameObject; }


    public void InitData()
    {

        InitMenuBookUI();

        InitMenuBookCamera();
        InitMenuBookObject();
    }

    public void OffActive()
    {

        MenuBookUI.SetActive(false);
        MenuBookCamera.SetActive(false);
        MenuBookObject.SetActive(false);
    }
}
