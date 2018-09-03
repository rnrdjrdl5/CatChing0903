using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSound : MonoBehaviour {

    private SoundManager trapSoundManager;

    private void Awake()
    {
        trapSoundManager = GetComponent<SoundManager>();
    }

    public void HitTrapSound()
    {

        trapSoundManager.PlayRandomEffectSound(SoundManager.EnumEffectSound.EFFECT_CAT_HITTRAP_1,
            SoundManager.EnumEffectSound.EFFECT_CAT_HITTRAP_3);
    }
}
