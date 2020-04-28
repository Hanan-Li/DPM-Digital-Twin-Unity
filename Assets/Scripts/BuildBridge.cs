using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridge : MonoBehaviour
{
    public CustomRaycaster crc;
    public GameObject robotBase;
    private Vector3 offsetFromBase = Vector3.zero;
    private RobotMovementController rmc;
    private calculateoffset offsets;

    // Start is called before the first frame update
    void Start()
    {
        rmc = GetComponent<RobotMovementController>();
        offsets = GetComponent<calculateoffset>();
        StartCoroutine(ControlledMovement());
    }

    IEnumerator ControlledMovement()
    {
        while (!crc.hasHit)
        {
            yield return null;
        }
        float backArmAngle = 0;
        float foreArmAngle = 0;
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 1.05f);
        rmc.rotateBackarm(backArmAngle, false);
        rmc.rotateForearm(foreArmAngle, false);
        rmc.Pause(1f);
        rmc.setFollow(crc, true);
        rmc.rotateForearm(-foreArmAngle, false);
        rmc.rotateBackarm(-backArmAngle, false);
        rmc.turn(90f, false);
        rmc.moveX(-5f, false);
        rmc.rotateBackarm(backArmAngle, true);
        rmc.rotateForearm(foreArmAngle, false);
        rmc.rotateForearm(-foreArmAngle, true);
        rmc.rotateBackarm(-backArmAngle, false);
        rmc.turn(90f, false);
        rmc.moveZ(-5f, false);

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
