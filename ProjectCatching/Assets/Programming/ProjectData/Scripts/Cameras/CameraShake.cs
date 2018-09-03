using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShakeProperties
{
    public Vector3 ShakePosition;               // 현재 위치
    public float ShakeSpeed;                // 현재 스피드
}


public class CameraShake : MonoBehaviour {


    private ShakeManager shakeManager;

    private bool isUseShake = false;


    private bool isCoroRunning = false; // 코루틴 실행중 판단 여부

    IEnumerator CoroShake;       // 코루틴

    // public Vector3 OnlyShakePosition;        // 코루틴으로 이동하는 거리


    public List<ShakeProperties> ShakeOptions;


    public float StartTime;

    public float LastSpeed;

    // 파티클 시스템 받아오기
    new private ParticleSystem particleSystem;
    bool isUseFirstUpdate = false;


    // 현재 쉐이크 위치
    int NowShakeCount;
    int MaxShakeCount;


    // 각 포지션 정의
    Vector3 NowPosition = Vector3.zero;
    Vector3 NextPosition = Vector3.zero;
    Vector3 DestPosition = Vector3.zero;

    // 카메라쉐이크 고유 값
    Vector3 OnlyShakePosition = Vector3.zero;
    Vector3 MovePosition = Vector3.zero;

    // 거리
    float MaxDis;
    float NowDis;


    private void Awake()
    {
        // 파티클, 쉐이더 관리자 받아옴
        particleSystem = GetComponent<ParticleSystem>();
        

        InitData();
    }

    private void Start()
    {

        SetCameraShake();

        ShakeOptions.Insert(0, new ShakeProperties
        {
            ShakePosition = Vector3.zero,
            ShakeSpeed = 0.0f
        });

        if (ShakeOptions[ShakeOptions.Count - 1].ShakePosition == Vector3.zero)
        {
            ShakeOptions.RemoveAt(ShakeOptions.Count-1);
        }

        ShakeOptions.Add(new ShakeProperties
        {
            ShakePosition = Vector3.zero,
            ShakeSpeed = LastSpeed 
        });
    }

    
    private void Update()
    {

        // 파티클 사용하지 않은 경우
        if (!isUseShake)
        {

            // 사용 시간이 되었다면
            if (particleSystem.time > StartTime)
            {
                // 새 스크립트로 설정, 이전 스크립트 데이터 설정 ,보간 설정
                shakeManager.InitCameraScript(this);


                // 대리자 설정
                ShakeManager.CameraShakeDele sm = new ShakeManager.CameraShakeDele(CameraShakeUpdate);
                shakeManager.csd = sm;

                //파티클 사용했다. 알림
                isUseShake = true;

                // 첫 업데이트 가능하도록 설정
                isUseFirstUpdate = false;
            }
        }

    }

    
    public Vector3 CameraShakeUpdate()
    {

        // 첫 FirstUpdate 사용했을 때
        if (!isUseFirstUpdate)
        {

            // 다음부터는 사용불가
            isUseFirstUpdate = true;

            // 현재 쉐이크 위치
            NowShakeCount = 0;
            MaxShakeCount = ShakeOptions.Count;


            // 각 포지션 정의
            NowPosition = ShakeOptions[NowShakeCount].ShakePosition;
            NextPosition = ShakeOptions[NowShakeCount + 1].ShakePosition;

            // 초기화
            DestPosition = Vector3.zero;
            OnlyShakePosition = Vector3.zero;

            // 이동 설정
            MovePosition = (NextPosition - NowPosition).normalized * ShakeOptions[NowShakeCount + 1].ShakeSpeed;

            // 최대 거리 설정
            MaxDis = Mathf.Abs((NextPosition - NowPosition).sqrMagnitude);
        }


        // 쉐이크 고유 값 갱신
        OnlyShakePosition += MovePosition;
        // 플레이어 이동상황 갱신
        DestPosition += MovePosition;


        

        //이동에 따른 거리 갱신
        NowDis = Mathf.Abs(DestPosition.sqrMagnitude);




        // 이동 완료 시 
        if (MaxDis <= NowDis)
        {


            // 카운트 증가
            NowShakeCount++;

            // 고유 쉐이크 위치 갱신
            OnlyShakePosition = ShakeOptions[NowShakeCount].ShakePosition;

            // 모두 이동 시 
            if (NowShakeCount + 1 >= ShakeOptions.Count)
            {

                // 다음 업데이트에서는 업데이트의 Start 사용가능
                isUseFirstUpdate = false;

                // 더이상 업데이트도 실행하지 않음 , 이번 쉐이크에서는 더이상 없음
                if(shakeManager.GetCameraShake() == this)
                    shakeManager.csd = null;
                
                // 원점으로 돌아가는 쉐이크 실행 (안함, 원점은 반드시 0,0,0에서 끝남 ) 
                //shakeManager.SetisUseShakelerp(true);
                
                // 마지막으로 전달
                return OnlyShakePosition;
            }

            // 위치들 갱신
            NowPosition = ShakeOptions[NowShakeCount].ShakePosition;
            NextPosition = ShakeOptions[NowShakeCount + 1].ShakePosition;

            DestPosition = Vector3.zero;

            MovePosition = (NextPosition - NowPosition).normalized * ShakeOptions[NowShakeCount + 1].ShakeSpeed;

            MaxDis = Mathf.Abs((NextPosition - NowPosition).sqrMagnitude);
        }
        return OnlyShakePosition;
    }


    public void FinishCameraShake()
    {

        isUseShake = true;
        isUseFirstUpdate = false;
    }

    private void InitData()
    {
        isUseFirstUpdate = false;


        NowShakeCount = 0;
        MaxShakeCount = 0;


        NowPosition = Vector3.zero;
        NextPosition = Vector3.zero;
        DestPosition = Vector3.zero;

        OnlyShakePosition = Vector3.zero;
        MovePosition = Vector3.zero;


        MaxDis = 0.0f;
        NowDis = 0.0f;
    }

    public void ResetCameraShake()
    {

        // 파티클  사용가능, 파티클 업데이트 사용가능
        isUseShake = false;
        isUseFirstUpdate = false;

        // 카메라쉐이크 본인이면 삭제

        if (shakeManager == null)
            SetCameraShake();

        if (shakeManager.GetCameraShake() == null)
            return;

        if (shakeManager.GetCameraShake() == this)
        {



            //( 업데이트 되고 있던 쉐이크가 본인 쉐이크였다면.)
            // 쉐이크 도중에 종료되버린다면 보간시켜야한다.
            if (shakeManager.csd == CameraShakeUpdate)
            {
                shakeManager.ChangeisUseShakelerp();
            }


            shakeManager.csd = null;        // 대리자 삭제
            shakeManager.SetCameraShake(null);    // 스크립트 삭제

        }
    }

    void SetCameraShake()
    {
        shakeManager = SpringArmObject.GetInstance().armCamera.GetComponent<ShakeManager>();
    }

}
