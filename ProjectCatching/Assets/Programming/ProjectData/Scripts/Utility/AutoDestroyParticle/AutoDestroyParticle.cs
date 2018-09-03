using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour {

    private ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ps)
        {
            if (!ps.IsAlive())
            {
                CameraShake cameraShake = GetComponent<CameraShake>();

                if (cameraShake != null)
                {
                    cameraShake.ResetCameraShake();
                    cameraShake.enabled = false;
                }

                PoolingManager.GetInstance().PushObject(gameObject);


            }
        }
	}
}
