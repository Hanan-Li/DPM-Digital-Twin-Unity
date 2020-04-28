using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTransform : MonoBehaviour
{

    public GameObject brick;
    Vector3 offset = Vector3.zero; //= new Vector3(0, -4.5f, -1.75f);
    Vector3 rotationOffset = new Vector3(0, 90, 0);
    public Vector3 timelinePosOffset;
    // Update is called once per frame
    void Update()
    {
        brick.transform.parent = transform;
        /*
        Vector3 pos = transform.position + transform.rotation * timelinePosOffset;

        Debug.DrawLine(pos, pos + Vector3.forward * 10, Color.yellow);
        Debug.DrawLine(pos, pos + Vector3.up * 10, Color.yellow);
        Debug.DrawLine(pos, pos + Vector3.left * 10, Color.yellow);
        Vector3 brickPos = transform.position + transform.rotation * offset;
        brick.transform.rotation = transform.rotation;
        brick.transform.Rotate(rotationOffset);
        Debug.DrawLine(brickPos, brickPos + Vector3.forward * 10, Color.red);
        Debug.DrawLine(brickPos, brickPos + Vector3.left * 10, Color.red);
        Debug.DrawLine(brickPos, brickPos + Vector3.up * 10, Color.red);
        brick.transform.position = brickPos;
        */
    }
}
