using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAnimator : MonoBehaviour {

    private string ObjectName;

    Animator animator;

    SphereCollider[] sc;
    BoxCollider[] bc;
    CapsuleCollider[] cc;

    public void SetAnimatorMode()
    {
        animator = GetComponent<Animator>();
        // 애니메이션 상태로 돌입합니다. 
        // 충돌 콜리더를 모두 꺼버리고 강체도 꺼버립니다.




        sc = GetComponents<SphereCollider>();
        bc = GetComponents<BoxCollider>();
        cc = GetComponents<CapsuleCollider>();

        if (sc != null)
        {
            for (int i = 0; i < sc.Length; i++)
            {
                sc[i].enabled = false;
            }
        }

        if (bc != null)
        {
            for (int i = 0; i < bc.Length; i++)
            {
                bc[i].enabled = false;
            }
        }

        if (cc != null)
        {
            for (int i = 0; i < cc.Length; i++)
            {
                cc[i].enabled = false;
            }
        }



        // 강체도 끕니다.
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;


        // 애니메이션 재생
        animator.SetBool("isAction", true);


    }

    // 애니메이션 이벤트
    public void OffObject()
    {
        animator.SetBool("isAction", false);

        Debug.Log("asdf");

        if (sc != null)
        {
            for (int i = 0; i < sc.Length; i++)
            {
                sc[i].enabled = true;
            }
        }

        if (bc != null)
        {
            for (int i = 0; i < bc.Length; i++)
            {
                bc[i].enabled = true;
            }
        }

        if (cc != null)
        {
            for (int i = 0; i < cc.Length; i++)
            {
                cc[i].enabled = true;
            }
        }


        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(GetComponent<ObjectIDScript>().ID);

        go.GetComponent<CollisionObject>().ResetSkillOption();

        PoolingManager.GetInstance().PushObject(go);
    }


}
