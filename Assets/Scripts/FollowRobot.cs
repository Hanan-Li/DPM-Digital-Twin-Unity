using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRobot : MonoBehaviour
{

    public GameObject robot;
    public GameObject brick;

    Vector3 offset = Vector3.zero;
    // new Vector3(0, -4.5f, -1.75f);
    Vector3 rotationOffset = new Vector3(0, 90, 0);


    private void OnEnable()
    {
        robot.GetComponent<PrintTransform>().enabled = true;
    }

    private void OnDisable()
    {
        robot.GetComponent<PrintTransform>().enabled = false;
    }

    /* 
        // Update is called once per frame
        void Update()
        {
            Debug.DrawLine(robot.transform.position, robot.transform.position + Vector3.forward * 10, Color.yellow);
            Debug.DrawLine(robot.transform.position, robot.transform.position + Vector3.up * 10, Color.yellow);
            Debug.DrawLine(robot.transform.position, robot.transform.position + Vector3.left * 10, Color.yellow);
            Vector3 brickPos = robot.transform.position + robot.transform.rotation * offset;
            brick.transform.rotation = robot.transform.rotation;
            brick.transform.Rotate(rotationOffset);
            Debug.DrawLine(brickPos, brickPos + Vector3.forward * 10, Color.red);
            Debug.DrawLine(brickPos, brickPos + Vector3.left * 10, Color.red);
            Debug.DrawLine(brickPos, brickPos + Vector3.up * 10, Color.red);
            brick.transform.position = brickPos;

        }
        */
}

