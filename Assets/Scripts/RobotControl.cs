using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour
{
    float speed = -4;
    float rotSpeed = 20;
    float rot = 4;
    float gravity = 4;

    Vector3 moveDir = Vector3.zero;

    CharacterController controller;

    public GameObject robotBase;
    public GameObject backarm;
    public GameObject forearm;
    public GameObject handrotator;
    Vector3 forearmOffset = new Vector3(-0.33f, 1.0f, -5.15f);
    Vector3 backarmOffset = new Vector3(0.5f, 1.5f, -1.0f);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);
        //transform.Translate(move.normalized * speed * Time.deltaTime);
        GetComponent<Rigidbody>().AddForce(move.normalized * speed);
        /*if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDir = new Vector3(0, 0, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector3(0, 0, -1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                moveDir = Vector3.zero;
            }
            Vector3 rotationDir = transform.TransformDirection(new Vector3(1, 0, 0));
            Vector3 backarmPivot = robotBase.transform.position + robotBase.transform.rotation * backarmOffset;
            Debug.Log(rotationDir);
            if (Input.GetKey(KeyCode.J))
            {
                Debug.Log(backarm.transform.rotation.eulerAngles);
                forearm.transform.RotateAround(backarmPivot, rotationDir, rotSpeed * Time.deltaTime);
                backarm.transform.RotateAround(backarmPivot, rotationDir, rotSpeed * Time.deltaTime);
                handrotator.transform.Rotate(rotationDir, -rotSpeed * Time.deltaTime, relativeTo: Space.World);
            }
            if (Input.GetKey(KeyCode.L))
            {
                Debug.Log(backarm.transform.rotation.eulerAngles);
                forearm.transform.RotateAround(backarmPivot, rotationDir, -rotSpeed * Time.deltaTime);
                backarm.transform.RotateAround(backarmPivot, rotationDir, -rotSpeed * Time.deltaTime);
                handrotator.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime, relativeTo: Space.World);
            }
            Vector3 pos = backarm.transform.position;
            Vector3 pivot = pos - backarm.transform.rotation * forearmOffset;
            Debug.DrawLine(pivot, pivot + Vector3.forward, Color.white);
            Debug.DrawLine(pivot, pivot + Vector3.left, Color.white);
            Debug.DrawLine(pivot, pivot + Vector3.up, Color.white);
            if (Input.GetKey(KeyCode.I))
            {
                forearm.transform.RotateAround(pivot, rotationDir, rotSpeed * Time.deltaTime);
                handrotator.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.K))
            {
                forearm.transform.RotateAround(pivot, rotationDir, -rotSpeed * Time.deltaTime);
                handrotator.transform.Rotate(-rotationDir, rotSpeed * Time.deltaTime);
            }
        }
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);*/

    }


}
