using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour {

    private PointToLocation PTL;

    public Texture FindTexture;

    [Header("- 상호작용 액션 거리")]
    [Tooltip(" - 상호작용이 작동할 수 있는 최대거리입니다.")]
    public float MaxInteractionDistance = 0.0f;


    public float MaxFindObjectRayCast;

    private GameObject ObjectTarget;

    public GameObject GetObjectTarget() { return ObjectTarget; }
    public void SetObjectTarget(GameObject GO) { ObjectTarget = GO; }





    private GameObject InGameCanvas;



    // 상호작용 물체의 속성입니다. 시간을 가지고 있습니다.
    private InteractiveState ObjectState;

    public InteractiveState GetObjectState() { return ObjectState; }
    public void SetObjectState(InteractiveState IS) { ObjectState = IS; }

    // 상호작용이 가능한지 여부,

    // 사용한 곳 : 상호작용 스킬.
    private bool IsInteraction;

    public bool GetIsInteraction() { return IsInteraction; }
    public void SetIsInteraction(bool SI) { IsInteraction = SI; }

    private bool isUseFindObject = true;

    public bool GetisUseFindObject() { return isUseFindObject; }
    public void SetisUseFindObject(bool FO) { isUseFindObject = FO; }


    [Header("- 하 / 중 액션 시간 기준")]
    public float LowMiddleRank = 2.0f;

    [Header("- 중 / 상 단계 액션 시간 기준")]
    public float MiddleHighRank = 4.0f;

    public Color LowColor = Color.green;
    public Color MiddleColor = Color.yellow;
    public Color HighColor = Color.red;




    private PhotonView pv;

    private PlayerState playerState;

    GameObject Interaction;
    private GameObject PreInterObject;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        pv = gameObject.GetComponent<PhotonView>();

        IsInteraction = false;
        isUseFindObject = true;

    }

    void Start() {



        PTL = new PointToLocation();
        InGameCanvas = UIManager.GetInstance().InGameCanvas;


        
    }


    // 단, 상호작용은 다른곳에서 적용한다.





        // 상호작용과의 거리, 거리가 가까우면 true, 멀면 false
    bool IsInteractionObjectDistance(GameObject Interaction)
    {
        if((Interaction.transform.position - gameObject.transform.position).magnitude <= MaxInteractionDistance)
        {
            return true;
        }
        
        else
        {
            return false;
        }
    }

    bool IsInteractionPlace()
    {
        if (ObjectState.playerPositionScript.IsCanInter == true)
        {
            return true;
        }
        else
            return false;
    }

    private enum FindType { DISTANCE, POSITION };
    void CreateMeleeObject(GameObject Interaction , FindType ft)
    {

        bool isMelee = false;

        if (ft == FindType.DISTANCE)
        {
            // 상호작용이 가능한 범위라면, 근접 일 때. 
            if (IsInteractionObjectDistance(Interaction) == true)
            {

                isMelee = true;
            }
        }

        else if (ft == FindType.POSITION)
        {

            if (ObjectState.playerPositionScript.IsCanInter == true)
            {
                isMelee = true;
            }
        }

        if (!isMelee)
            return;


            UIManager.GetInstance().pressImagePanelScript.PressImage.SetActive(true);

    }

    void DestroyMeleeObject()
    {
           UIManager.GetInstance().pressImagePanelScript.PressImage.SetActive(false);
    }

    void CreateFarObject(GameObject Interaction , FindType ft)
    {
        bool isFar = false;

        if (ft == FindType.DISTANCE)
        {
            if (IsInteractionObjectDistance(Interaction) != true)
            {
                isFar = true;
            }
        }

        else if (ft == FindType.POSITION)
        {
            if(ObjectState.playerPositionScript.IsCanInter==false)
            isFar = true;
        }

        if (!isFar)
            return;

        

    }

    void DestroyFarObject()
    {

    }






    void ChooseIsInteraction(GameObject Interaction)
    {
        // 상호작용이 가까운지 판단합니다. 가까우면 true를 리턴합니다.
        if (IsInteractionObjectDistance(Interaction))
        {
            IsInteraction = true;
        }
        else
            IsInteraction = false;
    }

    void ChoosePlaceIsInter()
    {
        // 포지션용

        if (ObjectState.playerPositionScript.IsCanInter == true)
        {
            IsInteraction = true;
        }

        else
            IsInteraction = false;
    }

    void ChooseMaterialColor(GameObject Interaction)
    {
        // 초 노 빨

        // 시간에 따라서 색상을 변경합니다.
        if (Interaction != null)
        {


            for (int i = 0; i < ObjectState.InterMaterials.Count; i++)
            {
                ObjectState.InterMaterials[i].SetTexture("_MainTex", FindTexture);

                if (ObjectState.InteractiveTime < LowMiddleRank)
                {
                    ObjectState.InterMaterials[i].color = LowColor;
                }

                else if (ObjectState.InteractiveTime >= LowMiddleRank && ObjectState.InteractiveTime < MiddleHighRank)
                {
                    ObjectState.InterMaterials[i].color = MiddleColor;
                }

                else if (ObjectState.InteractiveTime >= MiddleHighRank)
                {
                    ObjectState.InterMaterials[i].color = HighColor;
                }

            }

        }
        else
            Debug.LogWarning("에러");

    }

    void ResetMaterialColor()
    {
        for (int i = 0; i < ObjectState.InterMaterials.Count; i++)
        {
            ObjectState.InterMaterials[i].color = Color.white;
            ObjectState.InterMaterials[i].SetTexture("_MainTex",
                ObjectState.InterTexture[i]);
        }
    }

    void ResetMaterialColor(GameObject PreInter)
    {
        InteractiveState PreinterState = PreInter.GetComponent<InteractiveState>();

        if (PreinterState == null)
            return;

        for (int i = 0; i < PreinterState.InterMaterials.Count; i++)
        {
            PreinterState.InterMaterials[i].color = Color.white;
            PreinterState.InterMaterials[i].SetTexture("_MainTex",
                PreinterState.InterTexture[i]);
        }
    }

    // Update is called once per frame
    void Update () {


        if (pv.isMine)
        {

            if (playerState.GetWeaponType() != PlayerState.WeaponEnum.NONE)
                return;


            // 상호작용이 사용 가능한가?
            // 스킬에서 해제하고 
            // 스킬을 빠져나가면 다시 true가 된다.
                if (isUseFindObject)
            {


                // 이전 오브젝트 
                if (Interaction != null)
                {
                    PreInterObject = Interaction;
                }

                // 오브젝트 없으면 null.
                else
                {
                    PreInterObject = null;
                }


                // 사용 가능하면 레이를 쏴서 물체를 찾습니다.

                Interaction = PTL.FindObject(MaxFindObjectRayCast, "MainObject", SpringArmObject.GetInstance().armCamera);
                //(gameObject, MaxFindObjectRayCast, "MainObject");



            }

            // 사용 불가능하면 무조건 못찾게 한다.
            else
                Interaction = null;

            // 물체를 발견 했을 경우
            if (Interaction != null)
            {
                if (PreInterObject != Interaction && 
                    PreInterObject != null)
                {
                    // 오브젝트가 달라질 경우
                    ResetMaterialColor(PreInterObject);

                    PoolingManager.GetInstance().PositionArea.SetActive(false);
                }




                ObjectState = Interaction.GetComponent<InteractiveState>();
                if (ObjectState == null)
                    return;

                if (!ObjectState.GetCanUseObject())
                    return;


                if (ObjectState.ActionType == InteractiveState.EnumAction.PHYSICS ||
                    ObjectState.ActionType == InteractiveState.EnumAction.EQUIP)
                    ChooseIsInteraction(Interaction);

                else if (ObjectState.ActionType == InteractiveState.EnumAction.ANIMATION)
                {
                    if(ObjectState.InterPosType == InteractiveState.EnumInterPos.POSITION)
                        ChoosePlaceIsInter();
                    else
                        ChooseIsInteraction(Interaction);

                }


                /*
                //상호작용 오브젝트에게서 시간을 받아온다.
                SetObjectState(Interaction.GetComponent<InteractiveState>());
                */

                // 거리 상황에 따라 UI를 생성한다.

                FindType findType;
                if (ObjectState.ActionType == InteractiveState.EnumAction.PHYSICS ||
                    ObjectState.ActionType == InteractiveState.EnumAction.EQUIP)
                    findType = FindType.DISTANCE;
                else 
                {
                    if (ObjectState.InterPosType == InteractiveState.EnumInterPos.POSITION)
                        findType = FindType.POSITION;

                    else
                        findType = FindType.DISTANCE;

                }

                CreateFarObject(Interaction,findType);

                CreateMeleeObject(Interaction,findType);


                //시간에 비례해서 색상을 정합니다.
                ChooseMaterialColor(Interaction);

                //포지션 위치 활성화시킨다.
                ActivePositionEffect();


                // 물체를 발견했을 때, 이미 지정했던 오브젝트가 있는 경우 ,
                // 기존에 물체를 발견한 적이 있다면.
                if (Interaction != ObjectTarget &&
                    ObjectTarget != null)
                {

                    // 기존 오브젝트 색을 돌린다.
                    ResetMaterialColor();

                }


                // 레이캐스트로 맞은 상호작용 대상을 ObjectTarget에 저장합니다.
                ObjectTarget = Interaction;

            }


            // 오브젝트를 이미 타겟한 상태
            if (ObjectTarget != null)
            {

                // 타겟한상태 + 발견하지 못했다면.
                // 사용 한 곳 : 1 갑자기 다른 곳으로 커서를 돌릴때 ( max , melee 상황에서 다른 곳으로 돌릴 때 .)
                //              2 거리가 멀어질 때  ( max 이후로 멀어지는 경우 ) 

                

                if (Interaction == null)
                {
                    // 오브젝트 색상을 원래대로 돌려준다.
                    ResetMaterialColor();

                    if(ObjectState.InterPosType == InteractiveState.EnumInterPos.POSITION)
                    PoolingManager.GetInstance().PositionArea.SetActive(false);

                    // 오브젝트를 삭제하자. 
                    // 둘다 삭제하자.
                    DestroyMeleeObject();
                    DestroyFarObject();

                    IsInteraction = false;

                }

                // 타겟한 상태 + 발견했다면.
                else if (Interaction != null)
                {
                            




                    if (ObjectState.ActionType == InteractiveState.EnumAction.PHYSICS||
                        ObjectState.ActionType == InteractiveState.EnumAction.EQUIP ||
                        ObjectState.InterPosType == InteractiveState.EnumInterPos.DISTANCE)
                    {

                        // 원거리로 멀어졌다면.
                        if (!IsInteractionObjectDistance(Interaction))
                        {
                            // 근거리 UI 삭제.
                            DestroyMeleeObject();

                        }

                        // 근거리로 가까워졌다면.
                        else if (IsInteractionObjectDistance(Interaction))
                        {
                            DestroyFarObject();

                        }
                    }

                    else if (ObjectState.ActionType == InteractiveState.EnumAction.ANIMATION)
                    {
                        if (ObjectState.playerPositionScript.IsCanInter == false)
                        {
                            DestroyMeleeObject();
                        }

                        else if (ObjectState.playerPositionScript.IsCanInter == true)
                        {
                            DestroyFarObject();
                        }

                    }


                }

            }

        }

	}

    public void BackDefault()
    {
        // 색상을 원래대로 돌립니다.
       ResetMaterialColor();

        if (ObjectState.InterPosType == InteractiveState.EnumInterPos.POSITION)
            PoolingManager.GetInstance().PositionArea.SetActive(false);


        // 근접 전용 ui를 파괴합니다.
        DestroyMeleeObject();

        // 원거리 전용 ui를 파괴합니다.
        DestroyFarObject();

        // 타겟을 초기화시킵니다.
        ObjectTarget = null;
        
        // 상호작용이
        IsInteraction = false;

    }

    public void ActivePositionEffect()
    {
        if (ObjectState.InterPosType == InteractiveState.EnumInterPos.POSITION)
        {
            GameObject PositionEffect = PoolingManager.GetInstance().PositionArea;

            PositionEffect.SetActive(true);

            PositionEffect.transform.SetParent(
                ObjectState.PlayerInterPosition.transform);

            PositionEffect.transform.localPosition = Vector3.zero;
        }

    }

    




}
