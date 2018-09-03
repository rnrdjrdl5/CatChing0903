using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObjectTime : MonoBehaviour
{


    // 충돌체의 유지시간 , 0 = 유지시간 무한.
    public float collisionObjectTime;

    public void SetObjectTime(float OT) { collisionObjectTime = OT; }
    public float GetObjectTime() { return collisionObjectTime; }

    public float NowTimer = 0.0f;

    private CollisionObject collisionObject;
    

    // Use this for initialization
    void Start () {

        collisionObject = GetComponent<CollisionObject>();
	}
	
	// Update is called once per frame
	void Update () {
        DestroyTimer();
	}

    void DestroyTimer()
    {

         NowTimer += Time.deltaTime;
        if (collisionObjectTime <= NowTimer)
        {
              Debug.Log("삭제 , 시간 : " + NowTimer);
            NowTimer = 0.0f;
            collisionObjectTime = 0.0f;

            collisionObject.ResetSkillOption();

            PoolingManager.GetInstance().PushObject(gameObject);
        }

    }

    public void ResetObject()
    {
        Debug.Log("시간리셋돌아간다");
        NowTimer = 0.0f;
        collisionObjectTime = 0.0f;
    }
}
