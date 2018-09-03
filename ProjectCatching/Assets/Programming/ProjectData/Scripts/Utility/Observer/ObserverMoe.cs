using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverMoe : MonoBehaviour {

    public float ObserverRotateSpeed = 5.0f;
    public float ObserverMoveSpeed = 5.0f;

    public float PlayerRotationY;

    public float PlayerRotationX;

    private void Start()
    {

        SpringArmObject.GetInstance().PlayerObject = gameObject;
        SpringArmObject.GetInstance().SwapSpringArm(SpringArmType.EnumSpringArm.DRAMATIC);

        UIManager.GetInstance().UICanvas.SetActive(false);
        UIManager.GetInstance().InGameCanvas.SetActive(false);
        UIManager.GetInstance().ResultUI.SetActive(false);
    }

    private void Update()
    {

        PlayerRotationY += -(Input.GetAxis("Mouse Y")) * Time.deltaTime * ObserverRotateSpeed;

        PlayerRotationX += (Input.GetAxis("Mouse X")) * Time.deltaTime * ObserverRotateSpeed;


        // 입력값에 따라 회전시키기.

        Quaternion rotation =  Quaternion.Euler(PlayerRotationY, PlayerRotationX, 0);

        transform.rotation = rotation;


        transform.position += (Input.GetAxisRaw("Horizontal") * transform.right +
            Input.GetAxisRaw("Vertical") * transform.forward).normalized * ObserverMoveSpeed * Time.deltaTime;
    }
}
