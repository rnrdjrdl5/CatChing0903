using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState
{


    private Animator animator;

    public GameObject PlayerFrontCameraPosition;


    private string PlayerType;

    public string GetPlayerType() { return PlayerType; }
    public void SetPlayerType(string pt)
    {
        PlayerType = pt;
    }
    /********* 플레이어의 현재 상태를 가지는 열거형입니다. ******/
    public enum ConditionEnum
    { NONE, NOTUSINGSKILL , NOTCOLLISION , 
        IDLE, RUN, SPEEDRUN, JUMP,
        DAMAGE, STUN, GROGGY, NOTMOVE , SLIDE 
     , THROW_FRYING_PAN, TRAP, EMOTICON , HIDE
      , INTERACTION, RESCUE ,
    };

    public ConditionEnum PlayerCondition = ConditionEnum.IDLE;

    public ConditionEnum GetPlayerCondition() { return PlayerCondition; }

    public void SetPlayerCondition(ConditionEnum CE) { PlayerCondition = CE; }

    public bool EqualPlayerCondition(ConditionEnum CE)
    {
        if (CE == PlayerCondition) return true;
        else return false;
    }

    public enum WeaponEnum { NONE, SAUCE , ROPE};

    private WeaponEnum WeaponType = WeaponEnum.NONE;
    public WeaponEnum GetWeaponType() { return WeaponType; }
    public void SetWeaponType(WeaponEnum weapon)
    {
        WeaponType = weapon;
    }




    public bool isUseAttack = false;          // 공격 사용중인가


    public void SetisUseAttack(bool UA)
    {
        isUseAttack = UA;
    }

    public bool GetisUseAttack() { return isUseAttack; }


    public bool isUseEmoticon = false;

    public bool GetisUseEmoticon() { return isUseEmoticon; }
    public void SetisUseEmoticon(bool _ise) { isUseEmoticon = _ise; }


    public bool isCanActive = false;        // 행동 가능한지.




}
