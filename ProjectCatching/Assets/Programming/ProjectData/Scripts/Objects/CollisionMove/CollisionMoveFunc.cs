using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionMove
{
    void MoveObject()
    {
        gameObject.GetComponent<Rigidbody>().velocity = CollisionMoveDirect * CollisionMoveSpeed;
    }


}
