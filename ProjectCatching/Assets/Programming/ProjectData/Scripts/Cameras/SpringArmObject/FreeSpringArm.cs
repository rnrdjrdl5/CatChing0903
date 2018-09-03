using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpringArm : SpringArmType {

    private float SpringArmRotationX;
    public float GetSpringArmRotationX() { return SpringArmRotationX; }
    public void SetSpringArmRotationX(float ar) {SpringArmRotationX = ar;}


    public override void AwakeInit(SpringArmObject _springArmObject)
    {

        base.AwakeInit(_springArmObject);

        SpringArmState = EnumSpringArm.FREE;

    }

public override void SetSpringArm()
    {
        base.SetSpringArm();
        // 1. 플레이어 스프링암의 위치를 동일하게 한다. 
        springArmObject.transform.localPosition = Vector3.up *
            springArmObject.SpringArmPosition.y;

        // 회전값은0으로 
        springArmObject.transform.rotation = Quaternion.identity;

        // 2. 스프링암을 제자리에서 회전시킨다.
        springArmObject.transform.rotation =
         Quaternion.Euler(new Vector3(
        SetSpringArmRotationY(),
        SetSpringArmRotationX(),
        springArmObject.transform.localRotation.z));


        // 3. 스프링암을 기준으로 카메라의 위치를 이동시킨다.
        springArmObject.armCamera.transform.localPosition =
            new Vector3(springArmObject.SpringArmPosition.x,
            0.0f,
            springArmObject.SpringArmPosition.z);


          // 4. 벽 충돌 실시
          springArmObject.armCamera.transform.position =
              MovedPosByWall(springArmObject.armCamera.gameObject);
              
    }



    protected float SetSpringArmRotationX()
    {


        float rotationSpeed = springArmObject.PlayerMove.RotationSpeed;

        SpringArmRotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;


        ClampRotationX();

        return SpringArmRotationX;



    }

    private void ClampRotationX()
    {
        if (SpringArmRotationX > 360)
            SpringArmRotationX -= 360;
        else if (SpringArmRotationX < -360)
            SpringArmRotationX += 360;
    }

}
