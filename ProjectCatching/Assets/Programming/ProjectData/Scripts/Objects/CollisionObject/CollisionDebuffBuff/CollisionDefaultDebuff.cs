using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDefaultDebuff : MonoBehaviour
{

    private float MaxTime;

    public void SetMaxTime(float MT) { MaxTime = MT; }
    public float GetMaxTime() { return MaxTime; }

    public void ResetObject()
    {
        MaxTime = 0;

    }

	
}
