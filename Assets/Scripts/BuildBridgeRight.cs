using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridgeRight : MonoBehaviour
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
        //Debug.Log("Starting autoroutine");
        yield return new WaitForSeconds(1f);
        while (!crc.hasHit)
        {
            yield return null;
        }
        float backArmAngle = 0;
        float foreArmAngle = 0;
        float destbackArmAngle = 30.75943f;
        float destforeArmAngle = 4.27f;
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 1.05f);
        rmc.rotateBackarm(backArmAngle);
        rmc.rotateForearm(foreArmAngle);
        rmc.setFollow(crc);
        rmc.rotateForearm(-foreArmAngle);
        rmc.rotateBackarm(-backArmAngle);
        rmc.moveZ(0.25f);
        rmc.turn(-90f);
        rmc.rotateHandEnd(180f);
        rmc.Pause(0.5f);
        rmc.rotateForearm(destforeArmAngle);
        rmc.rotateBackarm(destbackArmAngle);
        rmc.setUnfollow(crc);
        rmc.rotateBackarm(-destbackArmAngle);
        rmc.rotateForearm(-destforeArmAngle);
        rmc.moveZ(-0.25f);
        rmc.turn(90f);
        rmc.moveX(2);
        for (int i = 0; i < 5; i++)
        {
            rmc.rotateBackarm(backArmAngle);
            rmc.rotateForearm(foreArmAngle);
            rmc.setFollow(crc);
            rmc.rotateForearm(-foreArmAngle);
            rmc.rotateBackarm(-backArmAngle);
            rmc.moveX(-2 * (i + 1));
            rmc.moveZ(-i * 1.2f - 0.6f);
            rmc.turn(-90f);
            rmc.rotateHandEnd(-90f);
            rmc.rotateForearm(destforeArmAngle);
            rmc.rotateBackarm(destbackArmAngle);
            rmc.setUnfollow(crc);
            rmc.rotateBackarm(-destbackArmAngle);
            rmc.rotateForearm(-destforeArmAngle);
            rmc.moveZ(i * 1.2f + 0.6f);
            rmc.turn(90f);
            if(i < 4)
            {
                rmc.moveX(2 * (i + 2));
            }

        }
        float secondBack = 41.38668f;
        float secondFore = -13.07807f;
        secondFore = -10.5f;
        rmc.moveX(2f);
        for (int i = 0; i < 4; i++)
        {
            rmc.rotateForearm(secondFore);
            rmc.rotateBackarm(secondBack);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-secondBack);
            rmc.rotateForearm(-secondFore);
            rmc.moveX(-2 * (i + 1));
            rmc.moveZ((i + 5) * -1.2f - 0.6f);
            rmc.turn(-90f);
            rmc.rotateHandEnd(-90f);
            rmc.rotateForearm(destforeArmAngle);
            rmc.rotateBackarm(destbackArmAngle);
            rmc.setUnfollow(crc);
            rmc.rotateBackarm(-destbackArmAngle);
            rmc.rotateForearm(-destforeArmAngle);
            rmc.moveZ((i + 5) * 1.2f + 0.6f);
            rmc.turn(90f);
            if (i < 3)
            {
                rmc.moveX(2 * (i + 2));
            }
        }

        rmc.rotateForearm(secondFore);
        rmc.rotateBackarm(secondBack);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-secondBack);
        rmc.rotateForearm(-secondFore);
        rmc.moveZ(9 * -1.2f - 0.3f);
        rmc.turn(-90f);
        rmc.rotateHandEnd(180f);
        rmc.rotateForearm(destforeArmAngle);
        rmc.rotateBackarm(destbackArmAngle);
        rmc.setUnfollow(crc);
        rmc.rotateBackarm(-destbackArmAngle);
        rmc.rotateForearm(-destforeArmAngle);
        rmc.moveZ(9 * 1.2f + 0.3f);
        rmc.turn(90f);
        rmc.moveX(2 * 6f);
        for(int i = 0; i < 2; i++)
        {
            rmc.rotateForearm(foreArmAngle);
            rmc.rotateBackarm(backArmAngle);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-backArmAngle);
            rmc.rotateForearm(-foreArmAngle);
            rmc.moveX(-(i + 6) * 2f);
            rmc.moveZ((i + 5) * -1.2f);
            rmc.turn(-90f);
            rmc.rotateHandEnd(-90f);
            rmc.rotateForearm(foreArmAngle);
            rmc.rotateBackarm(backArmAngle);
            rmc.setUnfollow(crc);
            rmc.rotateBackarm(-backArmAngle);
            rmc.rotateForearm(-foreArmAngle);
            rmc.moveZ((i + 5) * 1.2f);
            rmc.turn(90f);
            if (i < 1)
            {
                rmc.moveX((i + 7) * 2f);
            }
        }
        rmc.moveX(5 * 2f);
        for(int i = 0; i < 3; i++)
        {
            rmc.rotateForearm(secondFore);
            rmc.rotateBackarm(secondBack);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-secondBack);
            rmc.rotateForearm(-secondFore);
            rmc.moveX(-(i + 5) * 2f);
            rmc.moveZ((i + 7) * -1.2f);
            rmc.turn(-90f);
            rmc.rotateHandEnd(-90f);
            rmc.rotateForearm(foreArmAngle);
            rmc.rotateBackarm(backArmAngle);
            rmc.setUnfollow(crc);
            rmc.rotateBackarm(-backArmAngle);
            rmc.rotateForearm(-foreArmAngle);
            rmc.moveZ((i + 7) * 1.2f);
            rmc.turn(90f);
            if (i < 2)
            {
                rmc.moveX((i + 6) * 2f);
            }
        }

        rmc.moveX(2 * 8);
        while (!crc.hasHit)
        {
            yield return null;
        }
        float sidesBackArmAngle = 19.73204f;
        float sidesForeArmAngle = 57.573f;
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 0.4f);
        for(int i = 0; i < 9; i++)
        {
            rmc.rotateForearm(foreArmAngle);
            rmc.rotateBackarm(backArmAngle);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-backArmAngle);
            rmc.rotateForearm(-foreArmAngle);
            rmc.turn(-90f);
            rmc.rotateForearm(sidesForeArmAngle);
            rmc.rotateBackarm(sidesBackArmAngle);
            rmc.rotateHandFront(-90f);
            rmc.rotateHandEnd(90f);

            rmc.moveZ(-(i * 1.2f + 0.6f));
            rmc.moveX(-8 * 2f);
            rmc.moveX(-i * 0.8f);
            rmc.setUnfollow(crc);
            rmc.rotateHandFront(90f);
            rmc.rotateHandEnd(-90f);
            if (i < 8)
            {
                rmc.moveX(8 * 2f);
                rmc.moveX((i + 1) * 0.8f);
            }
            rmc.moveZ((i * 1.2f + 0.6f));
            rmc.rotateBackarm(-sidesBackArmAngle);
            rmc.rotateForearm(-sidesForeArmAngle);
            rmc.turn(90f);
        }

    }

    void calculateNumbers(GameObject brick, float y_distance, ref float backArmAngle, ref float foreArmAngle, float offset)
    {

        float x_distance = Mathf.Abs(brick.transform.parent.position.z - offsets.armCenter.z);
        y_distance = brick.transform.parent.position.y - offsets.armCenter.y + offset;
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
