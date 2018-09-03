using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour {

    private CameraShake cameraShake;
    public CameraShake GetCameraShake() { return cameraShake; }
    public void SetCameraShake(CameraShake _cameraShake) { cameraShake = _cameraShake; }


    public delegate Vector3 CameraShakeDele();
    public CameraShakeDele csd;

    private Vector3 CameraShakeMove;        // 쉐이크 이동값


    // 보간 사용 조건
    // 1. 쉐이크 사용도중 강제로 변경이 들어왔을 때
    // 2. 쉐이크 가 끝났을 때
    private bool isUseShakelerp;               // 쉐이크 사이 보간용

    public bool GetisUseShakelerp() { return isUseShakelerp; }
    public void SetisUseShakelerp(bool iusl) {
        isUseShakelerp = iusl;
    }

    public void ChangeisUseShakelerp()
    {
        if (CameraShakeMove == Vector3.zero)
        {
            isUseShakelerp = false;
        }

        else
        {
            if (isUseShakelerp != true)
            {
                Debug.Log("보간사용");
                isUseShakelerp = true;      // 보간시작

                float fpsMove = ShakeLerpTime / Time.deltaTime;     // 프레임

                ShakeLerpMove = (CameraShakeMove - Vector3.zero) / fpsMove; // 프레임 당 이동량
            }
        }
    }


    private Vector3 ShakeLerpMove;   //쉐이크 보간 이동 값
    private float ShakeLerpTime = 0.05f;        // 시간.

    // Use this for initialization
        private void Awake()
    {
        isUseShakelerp = false;
        CameraShakeMove = Vector3.zero;
    }

    void Start () {
        SpringArmObject.GetInstance().OtherCameraEvent += GetCameraShakeMove;
    }

    // Update is called once per frame
    void Update()
    {
        

        // 보간이 끝났을 경우.
        if(!isUseShakelerp)
        {
            if (csd != null)
            {
                CameraShakeMove = csd();
            }
            else
            {
                CameraShakeMove = Vector3.zero;
            }
        }

        // 보간중인경우
        // 보간조건 : 쉐이크 진행도중 다른 쉐이크 들어올 시 1)
        //            쉐이크 진행도중 파티클이 끝나버렸을때 2)
        else
        {


            if (CameraShakeMove != Vector3.zero)
            {

                if (ShakeLerpMove.sqrMagnitude >= CameraShakeMove.sqrMagnitude)
                {
                    CameraShakeMove = Vector3.zero;
                }

                else
                {
                    CameraShakeMove -= ShakeLerpMove;
                }
            }

            // 제자리인 경우
            else
            {
                isUseShakelerp = false;
                ShakeLerpMove = Vector3.zero;
            }
        }

        
    }

    //  이전 쉐이크에 데이터 설정
    public void InitCameraScript(CameraShake m_cameraShake)
    {
       
        // 이전 쉐이크가 끝나지않았을 때만 사용
        // (이전 파티클이 종료되거나  쉐이크 시간이 끝나면 알아서보간됨.)
        if (csd != null)
        {
            // shakemove에 따라서 보간여부 결정
            ChangeisUseShakelerp();
        }

        // 이전에 등록한 적이 있다면.
        if (cameraShake != null)
        {
            // 이전 끝냄.
            cameraShake.FinishCameraShake();
            
        }

        cameraShake = m_cameraShake;



    }

    public Vector3 GetCameraShakeMove()
    {
        return CameraShakeMove;
    }


}
