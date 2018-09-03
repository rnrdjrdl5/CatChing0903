using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawn : MonoBehaviour {

    public List<GameObject> RandomObjectPosition;              // 랜덤 생성 포지션

    public string ObjectName;

    public void ObjectSpawn()
    {
        if (PoolingManager.GetInstance() == null)
            return;

        int CatPlayerID = -1;

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Cat")
                CatPlayerID = PhotonNetwork.playerList[i].ID;
        }


        for (int i = 0; i < RandomObjectPosition.Count; i++)
        {

            GameObject go = PoolingManager.GetInstance().PopObject(ObjectName);

            go.transform.position = RandomObjectPosition[i].transform.position;



            ObjectData objectData = go.GetComponent<ObjectData>();

            if (go != null)
                objectData.SetData(CatPlayerID , ObjectName);
        }
        
    }

}
