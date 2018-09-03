using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour {

    private static VideoManager videoManager;
    public static VideoManager GetInstance() { return videoManager; }

    public WinLoseVideoScript winLoseVideoScript { get; set; }

    private void Awake()
    {
        videoManager = this;

        winLoseVideoScript = new WinLoseVideoScript();
        winLoseVideoScript.InitData();
    }


}
