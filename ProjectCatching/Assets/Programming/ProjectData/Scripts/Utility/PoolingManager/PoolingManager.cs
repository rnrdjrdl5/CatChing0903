using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

    

    public enum EffctType
    {
        ATTACK,

        ATTACKLINE1 , ATTACKLINE2, MIKE_SPOT_EFFECT, PIANO_RHYTHM_EFFECT, 

        BALL_DROP, NINJA_HIDE,

        BIGDUST_BIG, BIGDUST_SMALL, MIDDLEDUST_BIG , MIDDLEDUST_SMALL , SMALL_DUST_SMALL,

        POT_EFFECT , TABLE_EFFECT, FRAME_EFFECT, DIE_EFFECT,

        FIRST_DRAWEBREAK , FIRST_CARTCHARGE, FIRST_POS_SPREAD,

        MOUSE_START_DASH , MOUSE_DASH,

        TRAP_EFFECT , CHEESE_HEALING_EFFECT,

        DRAWER_EFFECT, HELP_BUBBLE, REVIVE_EFFECT,FRYINGPAN_HIT_EFFECT,DRAWER_TRAIL_EFFECT,

        ATTACK_FINISH, CHAIR_EFFECT,

        NONE

    }

    public GameObject CreateEffectCameraShake(EffctType effectType, bool isMine)
    {
        GameObject effect = null;
        switch (effectType)
        {
            case EffctType.TABLE_EFFECT:
                effect = PopObject("FX_KDH_HitTable_Charge");
                break;
            case EffctType.POT_EFFECT:
                effect = PopObject("FX_KDH_HitPlate_wave");
                break;
            case EffctType.CHAIR_EFFECT:
                effect = PopObject("FX_KDH_Prefab_chair");
                break;

        }

        if(isMine)
            CheckCameraShakeEffect(effect);

        return effect;
    }

    public GameObject CreateEffect(EffctType effectType)
    {
        GameObject effect = null;
        switch(effectType)
        {
            case EffctType.ATTACK:
                effect = PopObject("YSK_FX_Hit_Cat");
                break;

            case EffctType.ATTACKLINE1:
                effect = PopObject("YSK_Cat_Hit_Line_Attack_01_RE");
                break;
            case EffctType.ATTACKLINE2:
                effect = PopObject("YSK_Cat_Hit_Line_Attack_02_RE");
                break;

            case EffctType.BIGDUST_BIG:
                effect = PopObject("FX_YSK_Prefab_Dust_Big_Big");
                break;
            case EffctType.BIGDUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Big_Small");
                break;
            case EffctType.MIDDLEDUST_BIG:
                effect = PopObject("FX_YSK_Prefab_Dust_Middle_Big");
                break;
            case EffctType.MIDDLEDUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Middle_Small");
                break;
            case EffctType.SMALL_DUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Small_Small");
                break;
            case EffctType.MOUSE_START_DASH:
                effect = PopObject("FX_KDH_Prefab_MouseStartDash");
                break;
            case EffctType.MOUSE_DASH:
                effect = PopObject("FX_KDH_Prefab_MouseDash");
                break;
            case EffctType.TRAP_EFFECT:
                effect = PopObject("FX_YSK_Trap_Effect");
                break;
            case EffctType.FIRST_DRAWEBREAK:
                effect = PopObject("FX_KDH_DrawBreak_Mesh");
                break;
            case EffctType.FIRST_CARTCHARGE:
                effect = PopObject("FX_KDH_Prefab_HitCart");
                break;
            case EffctType.FIRST_POS_SPREAD:
                effect = PopObject("FX_KDH_Prefab_HitPos");
                break;
            case EffctType.MIKE_SPOT_EFFECT:
                effect = PopObject("YSK_FX_Mike_Effect_0528");
                break;
            case EffctType.PIANO_RHYTHM_EFFECT:
                effect = PopObject("YSK_FX_Piano_Effect_0528");
                break;
            case EffctType.CHEESE_HEALING_EFFECT:
                effect = PopObject("YSK_FX_Healing_Effect_0528");
                break;
            case EffctType.NINJA_HIDE:
                effect = PopObject("FX_KDH_Prefab_NinjaHide");
                break;
            case EffctType.BALL_DROP:
                effect = PopObject("FX_KDH_Prefab_BallDrop");
                break;
            case EffctType.FRAME_EFFECT:
                effect = PopObject("FX_KDH_Prefab_Frame_Test01");
                break;
            case EffctType.DIE_EFFECT:
                effect = PopObject("FX_KDH_Prefab_Die_Test01");
                break;
            case EffctType.DRAWER_EFFECT:
                effect = PopObject("YSK_FX_Cabinet");
                break;
            case EffctType.HELP_BUBBLE:
                effect = PopObject("YSK_FX_Help_Bubble");
                break;
            case EffctType.REVIVE_EFFECT:
                effect = PopObject("YSK_Revive_Effecet");
                break;
            case EffctType.FRYINGPAN_HIT_EFFECT:
                effect = PopObject("FX_YSK_Hit_Sequence_Effect");
                break;
            case EffctType.DRAWER_TRAIL_EFFECT:
                effect = PopObject("FX_Trail_Effcet");
                break;
            case EffctType.ATTACK_FINISH:
                effect = PopObject("YSK_FX_Hit_Cat_Finsh");
                break;

            case EffctType.NONE:
                effect = null;
                break;



        }

        return effect;
    }

    public float DecideEffectPosition(EffctType effectType)
    {
        if (effectType == EffctType.CHEESE_HEALING_EFFECT)
            return 0.0f;

        else
            return 0.5f;
    }

    public bool CheckAttachEffect(EffctType effectType)
    {
        if (effectType == EffctType.CHEESE_HEALING_EFFECT)
        {
            return true;
        }

        return false;
    }






    public enum SauceType { RED_SAUCE, YELLOW_SAUCE };

    public GameObject CreateSauce(SauceType sauceType)
    {
        GameObject sauce = null;

        switch (sauceType)
        {
            case SauceType.RED_SAUCE:
                sauce = PopObject("RedSauce");
                break;

            case SauceType.YELLOW_SAUCE:
                sauce = PopObject("YellowSauce");
                break;
        }

        if (sauce == null)
            Debug.LogWarning("에러");

        return sauce;
    }

    public enum EnumEmoticon{ ANGRY, HAPPY, HI, MERONG, SAD };

    public GameObject CreateEmoticon(EnumEmoticon enumEmoticon)
    {

        GameObject go = null;
        switch (enumEmoticon)
        {
            case EnumEmoticon.ANGRY:
                go = PopObject("Emoticon_Angry");
                break;

            case EnumEmoticon.HAPPY:
                go = PopObject("Emoticon_Happy");
                break;

            case EnumEmoticon.HI:
                go = PopObject("Emoticon_Hi");
                break;

            case EnumEmoticon.MERONG:
                go = PopObject("Emoticon_Merong");
                break;

            case EnumEmoticon.SAD:
                go = PopObject("Emoticon_Sad");
                break;


        }

        if (go == null)
            Debug.LogWarning("에러");

        return go;
    }



    static private PoolingManager poolingManager;

    static public PoolingManager GetInstance()
    {
        return poolingManager;
    }


    public Dictionary<string, PoolingObject> Poolings;

    public GameObject[] Prefabs;

    [Header(" Only One Object")]
    public GameObject PositionArea;

    private void Awake()
    {
        Poolings = new Dictionary<string, PoolingObject>();

        poolingManager = this;
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < Prefabs.Length; i++)
        {
            // 오브젝트 풀링 인스턴스화
            PoolingObject pm = new PoolingObject();

            pm.Objects = new List<GameObject>();        // 오브젝트 
            pm.ActiveObjects = new List<GameObject>();
           // ID지정, 프리팹위치 파악용
            pm.ID = i;

            // 상위 접근용
            pm.poolingManager = this;



            // 오브젝트 풀링 최소 오브젝트 생성
            for (int j = 0; j < 3; j++)
            {
                // 1. 게임 오브젝트 생성
                GameObject go = Instantiate(Prefabs[i]);
                
                go.name = Prefabs[i].name;

                // 2. 기본 속성 정의
                pm.InitProp(go, transform);

                // 3. 오브젝트 풀링
                pm.Objects.Add(go);
            }

            // 등록
            Poolings.Add(Prefabs[i].name, pm);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    public GameObject PopObject(string prefabName)
    {
        GameObject go;
        if (Poolings[prefabName].Objects.Count == 0)
        {
            go = CreateObject(prefabName);
            Poolings[prefabName].ActiveObjects.Add(go);
        }

        else
            go = Poolings[prefabName].PopObject();


        ObjectIDScript objectIDScript = go.GetComponent<ObjectIDScript>();

        // objectID가 존재하면 지급, 
        if (objectIDScript != null)
        {
            // 고유 값 적용
            objectIDScript.SetID();
        }

        
        

        return go;


    }

    public void PushObject(GameObject go)
    {
        Poolings[go.name].PushObject(go,transform);

        ObjectIDScript objectIDScript = go.GetComponent<ObjectIDScript>();

        if(objectIDScript != null)
            objectIDScript.DeleteID();

    }

    public GameObject CreateObject(string prefabName)
    {
        GameObject go = Instantiate(Prefabs[Poolings[prefabName].ID]);
        go.name = prefabName;

        return go;
    }


    // 맞았을 때 RPC 이용해서 다른 곳의 ID 검색
    public GameObject FindObjectUseObjectID(int ObjectID)
    {

        for (int i = 0; i < GetInstance().Prefabs.Length; i++)
        {

            string PoolingObjName = GetInstance().Prefabs[i].name;

            // 활성화된 오브젝트들
            List<GameObject> gos = GetInstance().Poolings[PoolingObjName].ActiveObjects;


            // 오브젝트 카운터 for문
            for (int j = 0; j < gos.Count; j++)
            {
                if (gos[j] != null)
                {

                    ObjectIDScript objectIDScript = gos[j].GetComponent<ObjectIDScript>();

                    if (objectIDScript != null)
                    {
                        
                        if (objectIDScript.ID == ObjectID)
                        {
                            
                            return gos[j].gameObject;
                        }
                    }
                }
            }

            
        }
        return null;

    }

    public void CheckCameraShakeEffect(GameObject effect)
    {

        CameraShake cameraShake = effect.GetComponent<CameraShake>();
        if (cameraShake == null) return;
            cameraShake.enabled = true;
    }






}
