using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtility {

    public bool IsIncludeRad(Vector3 DirVector, Vector3 DesVector, float Rad)
    {
        
        Vector2 DirVectorTo2 = new Vector2(DirVector.x, DirVector.z);
        Vector2 DesVectorTo2 = new Vector2(DesVector.x, DesVector.z);


        
        // 1. 두 벡터를 내적한다.
        float VectorRad = Vector2.Dot(DirVectorTo2.normalized, DesVectorTo2.normalized);
        // 2. 각도 비교
        if (VectorRad >= Rad * Mathf.Deg2Rad * 0.5f)
        {
            return true;
        }
        else
            return false;
    }




    public enum EnumDirVector { RIGHT, LEFT, UP, DOWN , NONE};
    public EnumDirVector VectorDirType(GameObject go, Vector3 DesVector)
    {

        if (IsIncludeRad(go.transform.right, DesVector - go.transform.position, 90))
        {
            Debug.Log("오른쪽");
            return EnumDirVector.RIGHT;
        }

        else if (IsIncludeRad(-go.transform.right, DesVector - go.transform.position, 90))
        {
            Debug.Log("왼쪽");
            return EnumDirVector.LEFT;
        }

        else if (IsIncludeRad(-go.transform.forward, DesVector - go.transform.position, 90))
        {
            Debug.Log("아래");
            return EnumDirVector.DOWN;
        }

        else if (IsIncludeRad(go.transform.forward, DesVector - go.transform.position, 90))
        {
            Debug.Log("위");
            return EnumDirVector.UP;

        }

        return EnumDirVector.NONE;
    }
}
