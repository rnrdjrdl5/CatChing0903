using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour {

    public float NowInterUseMount;
    public float MaxInterUseMount;

    public NewInteractionSkill newInteractionSkill;




    public delegate void DeleWeaponInter(float _var);
    private DeleWeaponInter WeaponInterUI;

    public void AddWeaponInterUI(DeleWeaponInter deleWeaponInter)
    {
        WeaponInterUI += deleWeaponInter;
    }




    public delegate void DeleWeaponInterFinish();
    private DeleWeaponInterFinish WeaponInterFinishEvent;

    public void AddWeaponInterFinishEvent(DeleWeaponInterFinish deleWeaponInterFinish)
    {
        WeaponInterFinishEvent += deleWeaponInterFinish;
    }




    public void InitData(int _maxInterUseMount, NewInteractionSkill _newInteractionSkill)
    {
        NowInterUseMount = _maxInterUseMount;
        MaxInterUseMount = _maxInterUseMount;

        newInteractionSkill = _newInteractionSkill;


        _newInteractionSkill.AddInterWeaponEvent(UseInterWeapon);
        WeaponInterUI += UIManager.GetInstance().saucePanelScript.DecreaseSauceUI;
        
    }

    void UseInterWeapon()
    {
        NowInterUseMount -= 1;
        float fillMount = NowInterUseMount / MaxInterUseMount;
        Debug.Log(fillMount);
        WeaponInterUI(fillMount);

        if (NowInterUseMount > 0)
            return;

        WeaponInterFinishEvent();

        Destroy(this);



    }
}
