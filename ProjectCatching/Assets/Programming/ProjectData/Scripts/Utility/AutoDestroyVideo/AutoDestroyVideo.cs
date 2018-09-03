using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AutoDestroyVideo : MonoBehaviour {

    VideoPlayer videoPlayer;

    public delegate void videoFinishDele();

    private videoFinishDele videoFinishEvent;

    // Update is called once per frame
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    void Update () {

        if ((videoPlayer.isPrepared &&
            !videoPlayer.isPlaying) || 
            Input.GetKeyDown(KeyCode.Escape))
        {
            SetEndVideo();
        }





    }
    

    private void SetEndVideo()
    {


        if (videoFinishEvent != null)
            videoFinishEvent();

        gameObject.SetActive(false);
    }

    public void AttachEvent(videoFinishDele finishDele)
    {
        videoFinishEvent = finishDele;
    }
}
