using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboarding : MonoBehaviour {
    private GameObject m_Camera;

    public Vector3 RotationOffset = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start () {
        m_Camera = SpringArmObject.GetInstance().armCamera;
	}


    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
        transform.Rotate(RotationOffset);
    }
}
