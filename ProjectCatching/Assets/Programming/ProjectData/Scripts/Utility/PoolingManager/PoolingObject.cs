using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject
{
    public PoolingManager poolingManager { get; set; }

    public List<GameObject> Objects { get; set; }           // 관리대상 오브젝트들

    public List<GameObject> ActiveObjects { get; set; }

    public int ID { get; set; }                  // 프리팹 번호


    public void InitProp(GameObject go , Transform tf)
    {

        // 설정
        go.SetActive(false);
        go.transform.SetParent(tf);
    }

    public GameObject PopObject()
    {
        GameObject go = Objects[0];

        go.SetActive(true);
        go.transform.SetParent(null);

        Objects.RemoveAt(0);

        ActiveObjects.Add(go);

        return go;
    }

    public void PushObject(GameObject go,Transform tf)
    {
        go.SetActive(false);
        go.transform.SetParent(tf);
        Objects.Add(go);
        ActiveObjects.Remove(FindActiveObject(go));
        
    }

    public GameObject FindActiveObject(GameObject go)
    {
        for (int i = 0; i < ActiveObjects.Count; i++)
        {
            if (ActiveObjects[i].gameObject == go)
            {
                return go;
            }

        }
        return null;
    }




}
