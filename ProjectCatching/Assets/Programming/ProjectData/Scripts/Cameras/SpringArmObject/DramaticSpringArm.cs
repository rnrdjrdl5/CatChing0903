using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaticSpringArm : SpringArmType {

    public override void SetSpringArm()
    {

        springArmObject.armCamera.transform.localPosition = Vector3.zero;

        springArmObject.transform.localPosition = Vector3.zero;

        springArmObject.transform.localRotation = Quaternion.identity;
        springArmObject.armCamera.transform.localRotation = Quaternion.identity;
    }
}
