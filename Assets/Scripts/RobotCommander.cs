using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCommander : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 startingLocation;
    public Vector3 endingLocation;
    public CustomRaycaster crc;
    public GameObject robotBase;

    private RobotMovementController rmc;
    private calculateoffset offsets;
    public bool startMove;
    private int facing_angle_relative_to_z;
    private float offset = 4.5f;
    float backArmAngle = 0;
    float foreArmAngle = 0;
    void Start()
    {
        startMove = false;
        facing_angle_relative_to_z = 180;
        rmc = GetComponent<RobotMovementController>();
        offsets = GetComponent<calculateoffset>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startMove)
        {
            StartCoroutine(MoveBlock());
            startMove = false;
        }
    }
    IEnumerator MoveBlock()
    {
        yield return StartCoroutine(MoveToPos(startingLocation));
        yield return StartCoroutine(PickUp());
        yield return StartCoroutine(MoveToPos(endingLocation));
        yield return StartCoroutine(PutDown());
        yield return null;
    }
    IEnumerator MoveToPos(Vector3 pos)
    {
        Vector3 moveVec = pos - robotBase.transform.position;
        Debug.Log(moveVec);
        if(Mathf.Abs(moveVec.z) > 0.1 )
        {
            if (moveVec.z < 0)
            {
                moveVec = new Vector3(moveVec.x, 0f, moveVec.z + 4.5f);
            }
            else
            {
                moveVec = new Vector3(moveVec.x, 0f, moveVec.z - 4.5f);
            }
            rmc.moveX(-moveVec.x);
            rmc.moveZ(-moveVec.z);
            if (moveVec.z < 0)
            {
                rmc.turn(180 - facing_angle_relative_to_z);
                facing_angle_relative_to_z = 180;
            }
            else if (moveVec.z > 0)
            {
                rmc.turn(-facing_angle_relative_to_z);
                facing_angle_relative_to_z = 0;
            }
        }
        else
        {
            if(moveVec.x > 0)
            {
                rmc.moveX(-(moveVec.x - offset));
                rmc.turn(90 - facing_angle_relative_to_z);
                facing_angle_relative_to_z = 90;
            }
            else
            {
                rmc.moveX(-(moveVec.x + offset));
                rmc.turn(270 - facing_angle_relative_to_z);
                facing_angle_relative_to_z = 270;
            }
        }
        while (!rmc.isEmpty())
        {
            yield return null;
        }
        Debug.Log("done");
        yield return null;
    }

    IEnumerator PickUp()
    {
        while (!crc.hasHit)
        {
            yield return null;
        }

        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 1.05f);
        rmc.rotateForearm(foreArmAngle);
        rmc.rotateBackarm(backArmAngle);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-backArmAngle);
        rmc.rotateForearm(-foreArmAngle);
        yield return null;
    }

    IEnumerator PutDown()
    {
        rmc.rotateForearm(foreArmAngle);
        rmc.rotateBackarm(backArmAngle);
        rmc.setUnfollow(crc);
        rmc.rotateBackarm(-backArmAngle);
        rmc.rotateForearm(-foreArmAngle);
        yield return null;
    }

    void calculateNumbers(GameObject brick, float y_distance, ref float backArmAngle, ref float foreArmAngle, float y_offset)
    {

        float x_distance = Mathf.Abs(brick.transform.parent.position.z - offsets.armCenter.z);
        y_distance = brick.transform.parent.position.y - offsets.armCenter.y + y_offset;
        //Debug.Log($"ydist: {y_distance}");
        //Debug.Log($"xdist:{x_distance}");
        //Debug.Log(brick.transform.parent.name);
        float hypot_dist = norm(x_distance, y_distance);
        // Debug.Log($"x_dist: {x_distance}, y_dist: {y_distance}, hypot_dist: {hypot_dist}");
        float d1 = Mathf.Rad2Deg * Mathf.Atan2(y_distance, x_distance);
        float c = lawOfCosines(offsets.backarmlength, offsets.forearmlength, hypot_dist);
        float d2 = lawOfCosines(hypot_dist, offsets.backarmlength, offsets.forearmlength);
        //Debug.Log(d1 + d2);
        //Debug.Log(c);
        backArmAngle = 90 - (d1 + d2);
        foreArmAngle = 90 - c;
        //Debug.Log(backArmAngle);
        //Debug.Log(foreArmAngle);
    }

    float norm(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }

    float lawOfCosines(float a, float b, float c)
    {
        return Mathf.Rad2Deg * Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));
    }
}
