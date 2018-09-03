using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpringArmType{


    public enum EnumSpringArm { FOLLOW, FREE , DRAMATIC}
    public EnumSpringArm SpringArmState { get; set; }


    protected float SpringArmRotationY;

    public float GetSpringArmRotationY() { return SpringArmRotationY; }
    public void SetSpringArmRotationY (float _springArmRotationY){SpringArmRotationY = _springArmRotationY; }

    protected PointToLocation pointToLocation;

    protected SpringArmObject springArmObject;



    public virtual void SetSpringArm() {

    }

    public virtual void AwakeInit(SpringArmObject _springArmObject)
    {

        pointToLocation = new PointToLocation();

        springArmObject = _springArmObject;
    }

    protected float SetSpringArmRotationY()
    {

        float rotationSpeed = springArmObject.PlayerMove.RotationSpeed;



        SpringArmRotationY += -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        if (SpringArmRotationY < springArmObject.MinRotationY)
            SpringArmRotationY = springArmObject.MinRotationY;

        else if (SpringArmRotationY > springArmObject.MaxRotationY)
            SpringArmRotationY = springArmObject.MaxRotationY;
        return SpringArmRotationY;

    }

    protected Vector3 MovedPosByWall(GameObject gameObject)
    {

        /*     return 
                  pointToLocation.FindWall(
                      springArmObject.PlayerObject,
                      gameObject,
                      springArmObject.SpringArmPosition.magnitude);*/

        return pointToLocation.FindWall(springArmObject.gameObject, springArmObject.armCamera, springArmObject.SpringArmPosition.magnitude);
            
    }




}
