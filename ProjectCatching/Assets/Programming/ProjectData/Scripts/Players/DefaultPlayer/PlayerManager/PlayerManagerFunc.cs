using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class PlayerManager
{



    void SetFallowCamera()
    {
        // 연속적인 코드보다, if의 효율성을 생각
        // 모든 조건을 판단해서 if로 넣는게 아닌 느낌
        // 조건문 간결화신경쓰고
        // if else가 많아지면 팩토리로 빼버리기
        
        PhotonView pv = gameObject.GetComponent<PhotonView>();

        if (pv == null)
            return;

        if (!gameObject.GetComponent<PhotonView>().isMine)
            return;



        
    }

    void SetSpringArm()
    {

        PhotonView pv = gameObject.GetPhotonView();

        if (pv == null) return;
        if (!pv.isMine) return;



        SpringArmObject springArmObject = SpringArmObject.GetInstance();
        if (springArmObject == null) return;

        springArmObject.PlayerObject = gameObject;


        springArmObject.transform.SetParent(transform);


        PlayerMove pm = GetComponent<PlayerMove>();
        if (pm == null) return;

        springArmObject.PlayerMove = pm;

        springArmObject.SpringArmPosition = PlayerViewPosition;
        

    }

    // Damage 들어갈 떄, 관련 데미지 모두삭제.






}
