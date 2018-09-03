using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInterObject : MonoBehaviour
{


    private PhotonView pv;



    public float FindRad;
    private void Awake()
    {
        gameObject.GetComponent<SphereCollider>().radius = FindRad;
        pv = GetComponentInParent<PhotonView>();
        if (!pv.isMine)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    int a = 0;

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "Interaction")
         {


             // 해당 스크립트를 받아옵니다.
             InteractiveState IS = other.gameObject.GetComponent<InteractiveState>();

            if (IS != null)
            {
                // 사용 불가능하면
                if (!IS.GetCanUseObject())
                {

                    // 반투명 키는 부분
                    

                    for (int i = 0; i < IS.GetInterMaterials().Count; i++)
                    {

                        Debug.Log("Materials i : " + i);

                        Color color = new Color(IS.GetInterMaterials()[i].color.r,
                            IS.GetInterMaterials()[i].color.g,
                            IS.GetInterMaterials()[i].color.b,
                            0.2f);

                        IS.GetInterMaterials()[i].color = color;
                    }

                    Debug.Log(" 반투명 시작!");
                }

                else
                {
                    Debug.Log("qwer33");
                }
            }
         }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interaction")
        {

            // 해당 스크립트를 받아옵니다.
            InteractiveState IS = other.gameObject.GetComponent<InteractiveState>();

            if (IS != null)
            {

                if (!IS.GetCanUseObject())
                {
                    for (int i = 0; i < IS.GetInterMaterials().Count; i++)
                    {

                        Color color = new Color(IS.GetInterMaterials()[i].color.r,
                            IS.GetInterMaterials()[i].color.g,
                            IS.GetInterMaterials()[i].color.b,
                            1.0f);

                        IS.GetInterMaterials()[i].color = color;
                    }

                    Debug.Log(" 반투명 off");
                }

            }
        }
    }
}



