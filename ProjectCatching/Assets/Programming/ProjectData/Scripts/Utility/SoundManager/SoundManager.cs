using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ****************주의점*********************

// 인스펙터의 사운드 순서와
// 열거형 순서와 일치해야합니다.

/*****************************************/
public class SoundManager : MonoBehaviour {

    /**** Public ****/

    public AudioClip[] EffectAudios;

    public AudioClip[] BGAudios;

    public enum EnumEffectSound
    {
        UI_BUTTONCLICK_1, UI_BUTTONCLICK_EXIT_1, UI_LOBBYENTER_1, UI_LOBBYENTER_2, UI_LOBBYLEFT_1, UI_TURNBOOK_1,

        UI_ROULETTE_1, UI_ROULETTE_2, UI_ROULETTE_3,

        EFFECT_CHAR_JUMP_1, EFFECT_CHAR_JUMP_2, EFFECT_CHAR_JUMP_3,

        EFFECT_CAT_ATTACK_1, EFFECT_CAT_ATTACK_2,
        EFFECT_CAT_ATTACK_HIT_1, EFFECT_CAT_ATTACK_HIT_2,

        EFFECT_CAT_THROWPAN_1, EFFECT_CAT_THROWPAN_2,

        EFFECT_CAT_TRAP_1,

        EFFECT_CAT_HITTRAP_1, EFFECT_CAT_HITTRAP_2, EFFECT_CAT_HITTRAP_3,

        EFFECT_CAT_BUTTONCLICK1, EFFECT_CAT_BUTTONBEEP, EFFECT_CAT_TURNOFF,

        UI_LAST_30_SECOND,
        
        UI_TIMEOVER_1,

        UI_MOUSE_WIN , UI_CAT_WIN

    }

    // 이 열거형은 PoolingManager의 EffectType과 동일하게 해야합니다.
    public enum EnumRandomEffectSound
    {
        EFFECT_ATTACK_1 = 0, EFFECT_THROWPAN_1 = 26, NONE = 29
    }

    public enum EnumBGSound
    {
        BG_LOBBY_SOUND, BG_INGAME_SOUND, BG_FAST_INGAME_SOUND
    }


    /**** Private ****/

    private AudioSource sourceComponent;

    private Dictionary<string, AudioClip> effectDic;
    private Dictionary<string, AudioClip> bgDic;

    private void Awake()
    {

            sourceComponent = GetComponent<AudioSource>();

            sourceComponent.loop = true;

        effectDic = new Dictionary<string, AudioClip>();
        bgDic = new Dictionary<string, AudioClip>();

        for (int i = 0; i < EffectAudios.Length; i++)
        {
            effectDic.Add(EffectAudios[i].name, EffectAudios[i]);
        }

        for (int i = 0; i < BGAudios.Length; i++)
        {
            bgDic.Add(BGAudios[i].name, BGAudios[i]);
        }
    }



    /**** Effect Sound ****/

    public void PlayEffectSound(EnumEffectSound enumEffectSound)
    {
        switch (enumEffectSound)
        {

            case EnumEffectSound.UI_BUTTONCLICK_1:
                sourceComponent.PlayOneShot(effectDic["Sound_UI_ButtonClick1"]);
                break;
            case EnumEffectSound.UI_BUTTONCLICK_EXIT_1:
                sourceComponent.PlayOneShot(effectDic["Sound_UI_ButtonClick2"]);
                break;


            case EnumEffectSound.UI_LOBBYENTER_1:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Meow_1"]);
                break;
            case EnumEffectSound.UI_LOBBYENTER_2:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Meow_2"]);
                break;
            case EnumEffectSound.UI_LOBBYLEFT_1:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Meow_3"]);
                break;


            case EnumEffectSound.UI_TURNBOOK_1:
                sourceComponent.PlayOneShot(effectDic["Effect_Book_Page_1"]);
                break;


            case EnumEffectSound.UI_ROULETTE_1:
                sourceComponent.PlayOneShot(effectDic["Sound_Roulette_01"]);
                break;
            case EnumEffectSound.UI_ROULETTE_2:
                sourceComponent.PlayOneShot(effectDic["Sound_Roulette_02"]);
                break;
            case EnumEffectSound.UI_ROULETTE_3:
                sourceComponent.PlayOneShot(effectDic["Sound_Roulette_03"]);
                break;

            case EnumEffectSound.EFFECT_CHAR_JUMP_1:
                sourceComponent.PlayOneShot(effectDic["Effect_Char_Jump_1"]);
                break;
            case EnumEffectSound.EFFECT_CHAR_JUMP_2:
                sourceComponent.PlayOneShot(effectDic["Effect_Char_Jump_2"]);
                break;
            case EnumEffectSound.EFFECT_CHAR_JUMP_3:
                sourceComponent.PlayOneShot(effectDic["Effect_Char_Jump_3"]);
                break;

            case EnumEffectSound.EFFECT_CAT_ATTACK_1:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Attack_1"]);
                break;
            case EnumEffectSound.EFFECT_CAT_ATTACK_2:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Attack_2"]);
                break;
            case EnumEffectSound.EFFECT_CAT_ATTACK_HIT_1:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Attack_Hit_1"]);
                break;
            case EnumEffectSound.EFFECT_CAT_ATTACK_HIT_2:
                sourceComponent.PlayOneShot(effectDic["Sound_Cat_Attack_Hit_2"]);
                break;

            case EnumEffectSound.EFFECT_CAT_THROWPAN_1:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_ThrowPan_1"]);
                break;
            case EnumEffectSound.EFFECT_CAT_THROWPAN_2:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_ThrowPan_2"]);
                break;

            case EnumEffectSound.EFFECT_CAT_TRAP_1:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_Trap_1"]);
                break;

            case EnumEffectSound.EFFECT_CAT_HITTRAP_1:
                sourceComponent.PlayOneShot(effectDic["Effect_Hit_Trap_1"]);
                break;
            case EnumEffectSound.EFFECT_CAT_HITTRAP_2:
                sourceComponent.PlayOneShot(effectDic["Effect_Hit_Trap_2"]);
                break;
            case EnumEffectSound.EFFECT_CAT_HITTRAP_3:
                sourceComponent.PlayOneShot(effectDic["Effect_Hit_Trap_3"]);
                break;

            case EnumEffectSound.EFFECT_CAT_BUTTONCLICK1:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_ButtonClick_1"]);
                break;
            case EnumEffectSound.EFFECT_CAT_BUTTONBEEP:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_ButtonBeep"]);
                break;
            case EnumEffectSound.EFFECT_CAT_TURNOFF:
                sourceComponent.PlayOneShot(effectDic["Effect_Cat_TurnOff"]);
                break;

            case EnumEffectSound.UI_LAST_30_SECOND:
                sourceComponent.PlayOneShot(effectDic["UI_Last_30Second"]);
                break;

            case EnumEffectSound.UI_TIMEOVER_1:
                sourceComponent.PlayOneShot(effectDic["UI_TimeOver_1"]);
                break;

            case EnumEffectSound.UI_MOUSE_WIN:
                sourceComponent.PlayOneShot(effectDic["UI_Mouse_Win"]);
                break;
            case EnumEffectSound.UI_CAT_WIN:
                sourceComponent.PlayOneShot(effectDic["UI_Cat_Win"]);
                break;






        }

    }

    public void PlayRandomEffectSound(EnumEffectSound startEffect, EnumEffectSound endEffect)
    {

        int random = Random.Range((int)startEffect, (int)endEffect + 1);

        PlayEffectSound((EnumEffectSound)random);
    }

    public void PlayRandomEffectSound(EnumRandomEffectSound enumRandomEffectSound)
    {
        switch (enumRandomEffectSound)
        {
            case EnumRandomEffectSound.EFFECT_ATTACK_1:
                PlayRandomEffectSound(EnumEffectSound.EFFECT_CAT_ATTACK_HIT_1, EnumEffectSound.EFFECT_CAT_ATTACK_HIT_2);
                break;

            case EnumRandomEffectSound.EFFECT_THROWPAN_1:
                PlayRandomEffectSound(EnumEffectSound.EFFECT_CAT_ATTACK_HIT_1, EnumEffectSound.EFFECT_CAT_ATTACK_HIT_2);
                break;

            case EnumRandomEffectSound.NONE:
                break;

        }

    }



    /**** BackGround Sound ****/

    public void PlayBGSound(EnumBGSound enumBGSound)
    {
        switch (enumBGSound)
        {

            case EnumBGSound.BG_LOBBY_SOUND:
                sourceComponent.clip = bgDic["Sound_Lobby_BackGround_1"];
                break;

            case EnumBGSound.BG_INGAME_SOUND:
                sourceComponent.clip = bgDic["BG_InGame_Normal_1"];
                break;
            case EnumBGSound.BG_FAST_INGAME_SOUND:
                sourceComponent.clip = bgDic["BG_InGame_Fast_1"];
                break;

        }
        sourceComponent.Play();

    }

    public void StopBGSound()
    {

        //sourceComponent.Stop();
        sourceComponent.clip = null;
    }

    public void FadeOutSound()
    {

        StartCoroutine("IEnumFadeOutSound");
    }

    IEnumerator IEnumFadeOutSound()
    {
        while (true)
        {
            sourceComponent.volume -= 0.05f;
            if (sourceComponent.volume <= 0)
            {
                sourceComponent.volume = 1.0f;
                sourceComponent.Stop();
                StopCoroutine("IEnumFadeOutSound");
                yield break;
            }

            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
