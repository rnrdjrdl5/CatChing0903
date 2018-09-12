using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************
*
* 코더 : 반재억
* 일자 : 2018-02-21    
**********************************************/
public class PointToLocation{





    


    public GameObject FindObject(float MaxLocationDistance, string LayerName , GameObject cameraObject)
    {
        Vector3 MouseVector3 = FindMouseCursorPosition(cameraObject);

        RaycastHit hit;

        Vector3 playerForward = SpringArmObject.GetInstance().PlayerObject.transform.forward;
        playerForward.y = cameraObject.transform.forward.y;


        if (Physics.Raycast(cameraObject.transform.position,
            MouseVector3,
            out hit,
            MaxLocationDistance,
            1<< LayerMask.NameToLayer(LayerName)))

        {
            return hit.collider.gameObject;
            

        }
        else
            return null;
    }





    public Vector3 FindWall(GameObject UseObject, GameObject SpringArmObject , float Distance)
    {
        // 카메라의 마지막 위치
        Vector3 FindPostCameraPosition = SpringArmObject.transform.position;

        // 2. 레이를 쏴버립니다.
        RaycastHit hit;

        //Debug.DrawRay(UseObject.transform.position, (SpringArmObject.transform.position - UseObject.transform.position).normalized * Distance, Color.red, Time.deltaTime);

        //바닥, 겉면만 레이 사용
        int layerMask = 1 << LayerMask.NameToLayer("Floor");

        if (Physics.Raycast(

            UseObject.transform.position,

            (SpringArmObject.transform.position - UseObject.transform.position).normalized,
            out hit,

          Distance,

            layerMask))

        {
            Debug.Log("찾음");

            // 3-2 : 카메라를 레이가 맞춘 자리로 적용
            FindPostCameraPosition = hit.point;

            FindPostCameraPosition -= (SpringArmObject.transform.position - UseObject.transform.position).normalized * 0.2f;
         // 레이에서 사용한 노말벡터 이용해서 벽과 충돌했을떄 거리보다 좀 당기기



        }


        // 4. 맞지않는다면 원래 값을 리턴합니다.
        // 맞았으면 맞은 값을 리턴합니다.
        return FindPostCameraPosition;
    }


   
    public Vector3 FindMouseCursorPosition(GameObject cameraObject)
    {

        return cameraObject.transform.forward;
    }
}
