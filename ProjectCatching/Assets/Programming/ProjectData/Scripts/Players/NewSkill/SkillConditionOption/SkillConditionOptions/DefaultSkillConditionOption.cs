using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkillConditionOption{
    protected DefaultNewSkill defaultSkill;


    virtual public bool CheckCondition()
    {
        return false;
    }

    public void SettingDefaultNewSkill(DefaultNewSkill dns)
    {
        defaultSkill = dns;
    }



}
