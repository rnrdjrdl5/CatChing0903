using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollisionReCheck : MonoBehaviour {

    private GameObject playerObject;

    public void SetPlayerObject(GameObject GO) { playerObject = GO; }
    public GameObject GetPlayerObject() { return playerObject; }
	

    private float playerReCheckTime;
    
    public void SetPlayerReCheckTime(float PRCT) { playerReCheckTime = PRCT; }
    public float GetPlayerReCheckTime() { return playerReCheckTime; }


	// Update is called once per frame
	void Update () {
		if(playerReCheckTime>0)
        {
            playerReCheckTime -= Time.deltaTime;
            if(playerReCheckTime <=0)
            {
                Destroy(this);
            }
        }
	}
}
