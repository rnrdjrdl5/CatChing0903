using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionMove
{
    public float CollisionMoveSpeed;

    public void SetCollisionMoveSpeed(float CMS) { CollisionMoveSpeed = CMS; }
    public float GetCollisionMoveSpeed() { return CollisionMoveSpeed; }

    public Vector3 CollisionMoveDirect;

    public void SetCollisionMoveDirect(Vector3 CMD) { CollisionMoveDirect = CMD; }
    public Vector3 GetCollisionMoveDirect() { return CollisionMoveDirect; }

    


}
