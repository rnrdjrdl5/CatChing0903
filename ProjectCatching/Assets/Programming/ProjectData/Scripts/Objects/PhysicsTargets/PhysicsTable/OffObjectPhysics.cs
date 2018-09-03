using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌 시 N초 뒤에 물리 제거
public class OffObjectPhysics : MonoBehaviour {

    public float OffTime;
    IEnumerator CoroOffPhysics;
    bool isCheck;

    public enum EnumPhysicsObject { SUBOBJECT, MAINOBJECT }
    public EnumPhysicsObject PhysicsObjectType;

    public EnumPhysicsObject GetPhysicsObjectType() { return PhysicsObjectType; }
    public void SetPhysicsObjectType(EnumPhysicsObject PO) { PhysicsObjectType = PO; }

    private void Awake()
    {
        PhysicsObjectType = EnumPhysicsObject.SUBOBJECT;
        CoroOffPhysics = OffPhysics();
        isCheck = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor") && !isCheck)
        {
            isCheck = true;
            // StartCoroutine(CoroOffPhysics);
            gameObject.layer = LayerMask.NameToLayer("NoPlayerInterEnd");


            GameObject go;

            if (PhysicsObjectType == EnumPhysicsObject.MAINOBJECT)
            {
                go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_BIG);
                go.transform.position = collision.contacts[0].point;

                go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_SMALL);
                go.transform.position = collision.contacts[0].point;
            }

            else if (PhysicsObjectType == EnumPhysicsObject.SUBOBJECT)
            {
                go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.SMALL_DUST_SMALL);
                go.transform.position = collision.contacts[0].point;
            }





        }
    }

    IEnumerator OffPhysics()
    {
        yield return new WaitForSeconds(OffTime);

        BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();


        for (int i = 0; i < boxColliders.Length; i++)
        {
            Destroy(boxColliders[i]);
        }

        Destroy(GetComponent<Rigidbody>());


        yield return null;
    }
}
