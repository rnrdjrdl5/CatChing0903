using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState
{


    // 디버프를 겁니다.
    public void AddDebuffState(DefaultPlayerSkillDebuff.EnumSkillDebuff ESD, float MaxTime)
    {
        gameObject.GetPhotonView().RPC("RPCAddDebuffState", PhotonTargets.All, (int)ESD, MaxTime);
    }

    [PunRPC]
    void RPCAddDebuffState(int ESD, float MaxTime)
    {
        if ((DefaultPlayerSkillDebuff.EnumSkillDebuff)ESD == DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN)
        {

            // 스턴 애니메이션 재생
            animator.SetBool("StunOnOff", true);

            PlayerStunDebuff playerStunDebuff = gameObject.GetComponent<PlayerStunDebuff>();

            if (playerStunDebuff == null)
            {
                playerStunDebuff = gameObject.AddComponent<PlayerStunDebuff>();
            }



            playerStunDebuff.SetMaxDebuffTime(MaxTime);
            playerStunDebuff.SetNowDebuffTime(0);
        }

        else if ((DefaultPlayerSkillDebuff.EnumSkillDebuff)ESD == DefaultPlayerSkillDebuff.EnumSkillDebuff.GROGGY)
        {
            Debug.Log("그로기");
            animator.SetBool("isGroggy", true);

            // 경직 디버프 받아오기
            PlayerGroggyDebuff playerGroggyDebuff = gameObject.GetComponent<PlayerGroggyDebuff>();

            // 경직 없으면 새로 추가
            if (playerGroggyDebuff == null)
            {
                playerGroggyDebuff = gameObject.AddComponent<PlayerGroggyDebuff>();
            }

            // 경직 속성 설정
            playerGroggyDebuff.SetMaxDebuffTime(MaxTime);
            playerGroggyDebuff.SetNowDebuffTime(0);
        }
    }





}
