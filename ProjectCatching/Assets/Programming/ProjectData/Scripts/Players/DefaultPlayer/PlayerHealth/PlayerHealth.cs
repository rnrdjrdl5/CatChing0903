using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth : Photon.PunBehaviour, IPunObservable
{
    
    private void Awake()
    {
        SetAwakeAll();

        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isRopeDead", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        if (gameObject.GetPhotonView().isMine)
            SetAwake();

        
    }

    public void Update()
    {        
        if (isHiting)
        {
            if (NowHiting + Time.deltaTime >= MaxHiting)
            {
                isHiting = false;
                NowHiting = 0.0f;

                for (int i = 0; i < skinnedMeshRenderer.Length; i++)
                {
                    skinnedMeshRenderer[i].material.color = Color.white;
                }
            }
            else
                NowHiting += Time.deltaTime;
        }

        CheckRopeHelp();

        CheckRopeDead();

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {


        if (stream.isWriting)
        {
            stream.SendNext(NowHealth);
            stream.SendNext(NowRopeDeadTime);
        }

        else
        {
            NowHealth = (float)stream.ReceiveNext();
            NowRopeDeadTime = (float)stream.ReceiveNext();
        }

    }



    


}
