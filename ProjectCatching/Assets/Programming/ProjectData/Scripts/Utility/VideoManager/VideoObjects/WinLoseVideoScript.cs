using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseVideoScript
{
    public GameObject WinLoseVideo { get; set; }
    private void InitWinLoseVideo()
    {
        WinLoseVideo = VideoManager.GetInstance().transform.Find("WinLoseVideo").gameObject;
    }


    public GameObject CatWinVideo { get; set; }
    private void InitCatWinVideo()
    {
        CatWinVideo = WinLoseVideo.transform.Find("CatWinVideo").gameObject;
    }


    public GameObject MouseWinVideo { get; set; }
    private void InitMouseWinVideo()
    {
        MouseWinVideo = WinLoseVideo.transform.Find("MouseWinVideo").gameObject;
    }

    public void InitData()
    {
        InitWinLoseVideo();

        InitCatWinVideo();
        InitMouseWinVideo();

    }



}
