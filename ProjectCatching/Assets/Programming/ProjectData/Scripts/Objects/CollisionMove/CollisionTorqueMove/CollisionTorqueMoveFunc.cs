using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionTorqueMove
{

    private void Awake()
    {
        frypanSoundManager = GetComponent<SoundManager>();
    }

    


    public void UseTorque()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = TorqueRad;
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.up * TorqueRad, ForceMode.Impulse);
        StartCoroutine("TorqueSound");
    }

    public void ResetObject()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 0;

        SetCollisionMoveDirect(Vector3.zero);
        SetCollisionMoveSpeed(0);
        SetTorqueRad(0);
        StopCoroutine("TorqueSound");
    }

    IEnumerator TorqueSound()
    {
        while (true)
        {
            frypanSoundManager.PlayRandomEffectSound(
                SoundManager.EnumEffectSound.EFFECT_CAT_THROWPAN_1,
                SoundManager.EnumEffectSound.EFFECT_CAT_THROWPAN_2);
            yield return new WaitForSeconds(0.25f);
        }

        
    }

}