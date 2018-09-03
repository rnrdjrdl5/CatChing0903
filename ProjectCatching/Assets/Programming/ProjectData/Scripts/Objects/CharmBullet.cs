using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmBullet : MonoBehaviour {



    public Vector3 BulletDistance = Vector3.zero;

    public float BulletSpeed = 1000.0f;

    public bool isBulletCheck = false;

    public string ShootPlayer = null;


    private float NotMoveTime = 0.0f;

    public float GetNotMoveTime() { return NotMoveTime; }

    public void SetNotMoveTime(float NMT) { NotMoveTime = NMT; }



    // Use this for initialization
    void Start () {
        StartCoroutine("TimerDestroy");
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
