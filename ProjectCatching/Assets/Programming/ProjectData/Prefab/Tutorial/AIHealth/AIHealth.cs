using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour {

    public float health { get; set; }
    

	// Use this for initialization
	void Awake () {
        health = 100;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyDamage(float _damage)
    {

            // 데미지 입음
             health -= _damage;



        // 체력 0이하면 밧줄로 변경
        if (health <= 0)
        {
            health = 0;
            //BindRope(); 로프처리
        }

        else if (health >= 100)
            health = 100.0f;
    }
}
