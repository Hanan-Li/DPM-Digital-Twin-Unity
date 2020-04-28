using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotSpeed = 1;
    public Transform target;
    public Transform player;
    float mouseX, mouseY;
    bool isThird;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isThird = true;
    }

    // Update is called once per frame
    void Update()
    {
        switchmode();
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, -50, 20);

        transform.LookAt(target);
        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    void switchmode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isThird)
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                isThird = false;
            }
            else
            {
                transform.localPosition = new Vector3(0, 10, -10);
                transform.localEulerAngles = new Vector3(45,0,0);
                isThird = true;
            }
        }
    }
}
