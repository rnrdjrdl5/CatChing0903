using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPhysics : MonoBehaviour {

    /**** public ****/
    public float XZPower;                

    public float MainMass;             // 테이블 질량

    public float subObjectMass;             // 보조 오브젝트 질량

    public GameObject[] subObjects;                // 보조 오브젝트

    public float CreateOffPhysicsObjectTime;           // 오브젝트 물리제거 시작시간
    public float PhysicsOffTime;        // 물리 꺼지는 시간


    /**** private ****/
    private Rigidbody rigidBody;

    IEnumerator CoroSetOffPhysics;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = MainMass;
        CoroSetOffPhysics = SetOffPhysics();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Action(Vector3 NormalVector3)
    {
        // 물체가 날라갈 방향, XZ 축
        Vector3 NotY_NormalVector3 = NormalVector3;
        NotY_NormalVector3.y = 0;

        // 모든 회전과 이동 가능
        rigidBody.constraints = RigidbodyConstraints.None;

        // 모든 보조 오브젝트 영향 받도록 설정
        UnLockSubObjects();

        // XZ축 힘 
        rigidBody.AddForce(NotY_NormalVector3 * XZPower, ForceMode.Impulse);

        StartCoroutine(CoroSetOffPhysics);
    }

    // 보조 오브젝트 연결 해제
    private void UnLockSubObjects()
    {
        // for문
        for (int i = subObjects.Length - 1; i >= 0; i--)
        {

            // 보조 오브젝트의 부모 없음으로 처리
            subObjects[i].transform.parent = null;

            // 강체 생성
            Rigidbody subRigidbody = subObjects[i].AddComponent<Rigidbody>();

            // 보조 오브젝트 물리 설정
            subRigidbody.constraints = RigidbodyConstraints.None;
            subRigidbody.maxAngularVelocity = subObjectMass;
            subRigidbody.mass = subObjectMass;

            subRigidbody.gameObject.AddComponent<BoxCollider>();
        }
    }

    // 물리 제거용 스크립트 생성
    IEnumerator SetOffPhysics()
    {

        // 플레이어와의 충돌은 없애고, 상호작용탐지도 없앤다.
        for (int i = 0; i < subObjects.Length; i++)
        {
            subObjects[i].layer = LayerMask.NameToLayer("NoPlayerIntering");
        }

        yield return new WaitForSeconds(2.0f);




        gameObject.layer = LayerMask.NameToLayer("NoPlayerInterEnd");

        // 플레이어와의 충돌은 없애고, 상호작용탐지도 없앤다.
        for (int i = 0; i < subObjects.Length; i++)
        {
            subObjects[i].layer = LayerMask.NameToLayer("NoPlayerInterEnd");
        }

        yield return null;
    }
}
