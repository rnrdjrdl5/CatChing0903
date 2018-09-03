using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleTime : MonoBehaviour {

    private ParticleSystem ps;
    public float MaxTime;
    private float NowTime;
    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        NowTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ps)
        {
            NowTime += Time.deltaTime;

            if (NowTime < MaxTime)
                return;

            NowTime = 0.0f;
            PoolingManager.GetInstance().PushObject(gameObject);
        }
    }
}
