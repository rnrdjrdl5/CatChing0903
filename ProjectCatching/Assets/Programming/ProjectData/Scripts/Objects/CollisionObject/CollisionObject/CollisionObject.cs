using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************
 * 제작일 : 2018. 02.22
 * 제작목적 : 총알, 공격판정 등 데미지를 주는 것들 ( 충돌물체 라 칭함 ) 의
 *           정보를 통해 데미지, 특수효과 , 상태이상 등을 넣을 때 사용됨. 
 *           
 * 사용하는 곳 : 총알, 충돌물체 등.
 * **********************************/
public partial class CollisionObject : MonoBehaviour
{

    private void Awake()
    {
        NoUseCollisionType = new List<string>();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
