using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * 

1. 충돌체 유지시간  : 충돌체가 자연스럽게 사라지는 시간

2. 충돌체 데미지 : 맞으면 데미지 입는 량
   충돌체 데미지 받는 횟수 : 얼마나 데미지 피해를 입을건지 ? 
    - 충돌 최대 횟수하고 나눈 이유 
     ex) 4타 맞고, 5타부터는 슬로우만 적용시킨다. 같은 스킬 때문에.

3. 충돌체 상태이상 : 넉백, 독 , 일반 , 빙결 등을 위한 스킬.

4. 충돌체 충돌 최대 횟수 : 충돌 최대횟수, 일정 횟수 이후에 삭제되게 하기 위해서. ex) 바드 q

5. 충돌체가 받는 데미지의 반복주기 ( 데미지 틱 )  : n초마다 데미지를 입습니다. 같은 스킬.

    */



  
public partial class CollisionObject
{

    // 다시 체크되는데 걸리는 시간.
    public float CollisionReCheckTime = 0.0f;

    public void SetCollisionReCheckTime(float CRCT){CollisionReCheckTime = CRCT;}
    public float GetCollisionReCheckTime() { return CollisionReCheckTime; }

    // 해당 Object를 생성한 플레이어. 
    public string UsePlayer;

    public void SetUsePlayer(string UP) { UsePlayer = UP; }
    public string GetUsePlayer() { return UsePlayer; }

    public int PlayerIOwnerID { get; set; }     // 공격한 사람의 클라이언트에서 충돌판정을 하기 위해서 사용   + push , pop 사용 시 ID값을 사용함.

    public GameObject UsePlayerObject;              // 공격한 플레이어의 GameObject


    public enum NoUseCollision { CAT, MOUSE, ROPE, NONE };

    private List<string> NoUseCollisionType;



    public void SetNoUseCollisionType(NoUseCollision noUseCollision)
    {

        if (noUseCollision == NoUseCollision.CAT)
            NoUseCollisionType.Add("Cat");

        else if (noUseCollision == NoUseCollision.MOUSE)
            NoUseCollisionType.Add("Mouse");

        else if(noUseCollision == NoUseCollision.ROPE)
            NoUseCollisionType.Add("Rope");

        else
            NoUseCollisionType.Add("None");
    }
    public List<string> GetNoUseCollisionType() { return NoUseCollisionType; }
}
