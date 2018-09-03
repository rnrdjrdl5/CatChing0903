using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코더 : 반재억
// 제작일 : 2018. 02. 22
// 사용목적 : 플레이어 충돌체크를 담당.
// 사용하는 곳 : 플레이어 컴포넌트.
public class PlayerCheckCollision : Photon.PunBehaviour , IPunObservable {

    private BaseCollision baseCollision;


	// Use this for initialization
	void Start () {
        baseCollision = gameObject.AddComponent<BaseCollision>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnTriggerEnter(Collider other)
    {
        //CheckCollision(gameObject, other);
    }

    private void OnTriggerStay(Collider other)
    {

        CheckCollision(gameObject, other);
       
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌체크테스트");
    }

    


    // 클라이언트, 서버 모두 실시합니다.
    void CheckCollision(GameObject go , Collider other)
    {
        //Debug.Log("충돌체크`" + go.name);
        if (other.tag == "CheckCollision")
        {
            baseCollision.UseCollision(other);

        }

        else if (other.tag == "ItemCollision")
        {

            baseCollision.UseItemCollision(other);
        }
          
    }
}
