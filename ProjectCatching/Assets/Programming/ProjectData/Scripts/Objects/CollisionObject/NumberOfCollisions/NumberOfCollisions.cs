using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfCollisions : MonoBehaviour
{

    // 충돌물체 물질 몇번까지 맞을수있는지?
    // 유지, 한번맞으면 사라진다던지 몇명까지 유지한다던지.

    // 이즈리얼 궁 : 여러번
    // 바드 q : 2번.

    // -1 : 쭉 유지.
    public float numberOfCollisions;

    public void SetNumberOfCollisions(float NOC)
    {
        numberOfCollisions = NOC;
    }
    public float GetNumberOfCollisions()
    { return numberOfCollisions; }

    public void DeCreaseNumberOfCollisions()
    {
        if (numberOfCollisions >= 0)
            numberOfCollisions -= 1;
    }

    public void ResetObject()
    {
        numberOfCollisions = 0;

    }

}
