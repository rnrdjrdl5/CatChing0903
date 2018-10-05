using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArmObject : MonoBehaviour
{

    // 카메라 쉐이크용 대리자
    public delegate Vector3 OtherCameraMoveDele();
    public event OtherCameraMoveDele OtherCameraEvent;

    // 시스템 소리 사용용도
    private SoundManager SystemSoundManager;
    public SoundManager GetSystemSoundManager() { return SystemSoundManager; }

    public static SpringArmObject GetInstance()
    {
        if (springArmObject == null)
        {
            return null;
        }

        return springArmObject;
    }

    private static SpringArmObject springArmObject;



    public SpringArmType springArmType;

    private FollowSpringArm followSpringArm;
    private FollowSpringArm GetfollowSpringArm() { return followSpringArm; }

    private FreeSpringArm freeSpringArm;
    public FreeSpringArm GetfreeSpringArm() { return freeSpringArm; }

    private DramaticSpringArm DramaticSpringArm;
    public DramaticSpringArm GetDramaticSpringArm() { return DramaticSpringArm; }

    public void SwapSpringArm(SpringArmType.EnumSpringArm _enumSpringArm)
    {
        switch (_enumSpringArm)
        {

            case SpringArmType.EnumSpringArm.FOLLOW:
                springArmType = followSpringArm;
                break;

            case SpringArmType.EnumSpringArm.FREE:
                springArmType = freeSpringArm;
                break;

            case SpringArmType.EnumSpringArm.DRAMATIC:
                springArmType = DramaticSpringArm;
                break;

        }
    }


    public PlayerMove PlayerMove;


    public GameObject PlayerObject;// { get; set; }
    public GameObject armCamera { get; set; }

    public Vector3 SpringArmPosition;

    public int SeePlayerNumber = 0;

    public float MinRotationY = -45;
    public float MaxRotationY = 45;

    public bool IsUseObserver { get; set; }


    private void Awake()
    {
        IsUseObserver = false;
        springArmObject = this;

        SystemSoundManager = GetComponent<SoundManager>();



        InitArmTypes();
    }

    private void Start()
    {
        armCamera = transform.Find("NewPlayerCamera").gameObject;

        armCamera.transform.localPosition = Vector3.zero;


    }

    private void Update()
    {
        ChangeObserverTarget();

    }



    private void LateUpdate()
    {

        if (!CheckHavingPlayerObject()) return;

        springArmType.SetSpringArm();

        // 카메라 이벤트 있으면
        if (OtherCameraEvent != null)
        {

            // 카메라 이동
            armCamera.transform.Translate(OtherCameraEvent(), Space.Self);
        }

    }

    private void InitArmTypes()
    {
        followSpringArm = new FollowSpringArm();
        freeSpringArm = new FreeSpringArm();
        DramaticSpringArm = new DramaticSpringArm();

        followSpringArm.AwakeInit(this);
        freeSpringArm.AwakeInit(this);
        DramaticSpringArm.AwakeInit(this);

        springArmType = followSpringArm;

    }




    public bool CheckHavingPlayerObject()
    {
        if (PlayerObject == null) return false;

        else return true;
    }

    public void ChangeObserverTarget()
    {
        if (!IsUseObserver) return;

        if (!Input.GetMouseButtonDown(0)) return;

        FindSeePlayer();
    }

    public void FindSeePlayer()
    {
        if (springArmType == DramaticSpringArm) return;

        PhotonManager photonManager = PhotonManager.GetInstance();
        if (photonManager == null) return;

        SeePlayerNumber++;
        if (SeePlayerNumber >= PhotonManager.GetInstance().AllPlayers.Count)
            SeePlayerNumber = 0;


        PlayerObject = PhotonManager.GetInstance().AllPlayers[SeePlayerNumber];
        springArmObject.transform.SetParent(PlayerObject.transform);

        SpringArmPosition = PlayerObject.GetComponent<PlayerManager>().PlayerViewPosition;


    }
}
