using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBookScript : MonoBehaviour
{

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = GameObject.Find("Main Camera").GetComponent<SoundManager>();
    }

    public void CallPageSound()
    {

        soundManager.PlayEffectSound(SoundManager.EnumEffectSound.UI_TURNBOOK_1);
    }
}
