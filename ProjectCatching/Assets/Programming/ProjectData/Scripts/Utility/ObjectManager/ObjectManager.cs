using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//다른 클라이언트에서 오브젝트를 찾기 위해서 사용 , 
// 점수를 확인하기 위해서도 사용
public class ObjectManager : MonoBehaviour {

    private const int MAX_OBJECT_TYPE = 11;

    private static ObjectManager objectManager;
    public static ObjectManager GetInstance() { return objectManager; }

    public int MaxInterObj { get; set; }
    public List<GameObject> InterObj;
    public void AddInterObj(GameObject go) { InterObj.Add(go); }
    public PhotonManager photonManager;

    public delegate void RemoveDele();
    public event RemoveDele RemoveEvent;

    // 아래부터는 오브젝트 카운팅 용 변수 입니다.

    private int[] mountObjects;

    private void Awake()
    {
        objectManager = this;
        mountObjects = new int[MAX_OBJECT_TYPE + 1];        // Enum 시작점이 1이라서.
    }

    private void Start()
    {
        MaxInterObj = InterObj.Count;
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    public GameObject FindObject(int vID)
    {
        for (int i = 0; i < InterObj.Count; i++)
        {


            if (InterObj[i].GetPhotonView().viewID == vID)
            {
                return InterObj[i];
            }
        }
        return null;
    }

    public void RemoveObject(int vID)
    {

        for (int i = 0; i < InterObj.Count; i++)
        {

            if (InterObj[i].GetPhotonView().viewID == vID)
            {

                // 쥐 플레이어 인 경우
                if ((string)PhotonNetwork.player.CustomProperties["PlayerType"] == "Cat")
                {

                    int NowCatScore = (int)PhotonNetwork.player.CustomProperties["CatScore"];

                    int NextCatScore = NowCatScore - InterObj[i].GetComponent<InteractiveState>().InterObjectScore;
                    if (NextCatScore <= 0)
                        NextCatScore = 0;


                    PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CatScore", NextCatScore } });

                    

                }
                // 오브젝트에서 삭제
                InterObj.Remove(InterObj[i]);

                if (RemoveEvent == null)
                {
                    Debug.LogWarning("비었음. 에러");
                    return;
                }
                    RemoveEvent();


            }
        }

    }

    public void IncObjectCount(InteractiveState.EnumInteractiveObject enumInteractiveObject , 
        int objectHeight)
    {

        mountObjects[(int)enumInteractiveObject] += objectHeight;

        Debug.Log(mountObjects[(int)enumInteractiveObject]);

    }

    public void DeleteObjPropPlayer()
    {

        //foreach (GameObject go in InterObj)
        for (int i = InterObj.Count-1; i >= 0; i--)
        
        {
            
            InteractiveState IS = InterObj[i].GetComponent<InteractiveState>();


            if(IS != null)
            {

                if (photonManager == null)
                {
                    Debug.Log("없음");
                    return;
                }
                if (photonManager.MousePlayerListOneSort.Count <
                    IS.MinPlayerMount && 
                    IS.MinPlayerMount != 0)
                {
                    InterObj.Remove(IS.gameObject);
                    IS.gameObject.SetActive(false);

                    Debug.Log("삭***제완료");
                }


                
                
            }
        }
        Debug.Log("수행중");
    }
    
}
