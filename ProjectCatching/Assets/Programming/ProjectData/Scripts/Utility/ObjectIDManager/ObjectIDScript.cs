using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 보완 필요
 *  문제점 : static 을 사용해서 판마다 새로 선언이 되지 않은 점.
 * 
 * 
 * 
 * 
 */
public class ObjectIDScript : MonoBehaviour {

    const int MAXCOUNT = 100000;
    public static int MaxID = 0;

    public int ID;

    public void SetID()
    {
        
        ID = MaxID;
        IncreaseMaxID();
    }

    public void DeleteID()
    {
        ID = -1;
    }

    public void IncreaseMaxID()
    {
        if (MaxID > MAXCOUNT) MaxID = 0;

        else MaxID++;
    }

    private void OnDestroy()
    {
        MaxID = 0;
    }
}
