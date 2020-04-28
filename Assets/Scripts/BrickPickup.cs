using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPickup : MonoBehaviour
{
    GameObject pickedUpBrick = null;

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUpBrick) return;
        Debug.Log("We hit something");
        if (other.gameObject.tag == "Brick")
        {
            pickedUpBrick = other.gameObject.transform.parent.gameObject;
            pickedUpBrick.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("staying");
    }

    Vector3 offset = new Vector3(0, -0.5f, -0.75f);
    Quaternion rotationOffset = new Quaternion(0, 1, 0, 0);
    private void Update()
    {
        Vector3 brickPos = transform.position + transform.rotation * offset;
        Quaternion brickRot = transform.rotation * rotationOffset;
        Debug.DrawLine(brickPos, brickPos + Vector3.forward);
        Debug.DrawLine(brickPos, brickPos + Vector3.left);
        Debug.DrawLine(brickPos, brickPos + Vector3.up);


        if (pickedUpBrick)
        {
            pickedUpBrick.transform.position = brickPos;
            pickedUpBrick.transform.rotation = brickRot;
            if (Input.GetKey(KeyCode.R))
            {
                pickedUpBrick.GetComponent<Rigidbody>().isKinematic = false;
                pickedUpBrick = null;
            }
        }
    }
}
