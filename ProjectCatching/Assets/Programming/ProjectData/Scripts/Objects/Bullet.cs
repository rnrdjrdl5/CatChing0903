using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 BulletDistance = Vector3.zero;

    public float BulletSpeed = 1000.0f;

    public bool isBulletCheck = false;

    public string ShootPlayer = null;




	// Use this for initialization
	void Start () {
        StartCoroutine("TimerDestroy");
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void FixedUpdate()
    {
        if (BulletDistance != Vector3.zero)
        {
            GetComponent<Rigidbody>().velocity = BulletDistance * BulletSpeed * Time.fixedDeltaTime;
        }

    }

    // 사거리에 비례해서 시간을 설정해야 합니다.
    IEnumerator TimerDestroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
        yield return null;
    }
}
