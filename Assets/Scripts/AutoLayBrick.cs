using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLayBrick : MonoBehaviour
{
    public CustomRaycaster crc;
    public GameObject robotBase;
    private Vector3 offsetFromBase = Vector3.zero;
    private RobotMovementController rmc;
    private float forearmLength;
    private float backarmLength;
    private Vector3 backarmOffset = new Vector3(0.5f, 1.5f, -1.0f);

    private void Start()
    {
        rmc = GetComponent<RobotMovementController>();
        StartCoroutine(ControlledMovement());

        forearmLength = 3.2f;
        backarmLength = 3.2f;
    }

    IEnumerator ControlledMovement()
    {
        Debug.Log("Starting autoroutine");
        yield return new WaitForSeconds(1f);
        while (!crc.hasHit)
        {
            yield return null;
        }

        float backArmAngle = 0;
        float foreArmAngle = 0;
        calculateNumbers(crc.gameObject, crc.distance, ref backArmAngle, ref foreArmAngle);
        Debug.Log($"backarmangle:{backArmAngle} forearmangle: {foreArmAngle}");
        for(int i = 0; i < 10; i++)
        {
            rmc.rotateBackarm(backArmAngle);
            rmc.rotateForearm(foreArmAngle);
            rmc.setFollow(crc);
            rmc.rotateForearm(0);
            rmc.rotateBackarm(90);
            /*rmc.turn(90f);
            rmc.moveForwards(7.3f + i*2);
            if(i != 0)
            {
                rmc.turn(90f);
                rmc.moveForwards(i * 2);
                rmc.turn(-90f);
            }

            rmc.rotateBackarm(backArmAngle);
            rmc.rotateForearm(foreArmAngle);
            rmc.setUnfollow(crc);
            rmc.rotateForearm(0);
            rmc.rotateBackarm(90);
            if(i != 0)
            {
                rmc.turn(90f);
                rmc.moveBackwards(i * 2);
                rmc.turn(-90f);
            }
            rmc.moveBackwards(7.3f + (i + 1) * 2);
            rmc.turn(-90f);*/
        }
    }

    /// <summary>
    /// Given brick to pickup, find the angle needed to rotate forearm and backarm to accurately pick up the brick
    /// DEBUG: needs to fine tune equations
    /// </summary>
    /// <param name="brick">brick gameobject needed to pickup</param>
    /// <param name="y_distance">height of brick from robot art</param>
    /// <param name="backArmAngle">reference backarmangle that will be set in this function</param>
    /// <param name="foreArmAngle">reference forearmangle that will be set in this function</param>
    void calculateNumbers(GameObject brick, float y_distance, ref float backArmAngle, ref float foreArmAngle)
    {
        offsetFromBase = robotBase.transform.position + robotBase.transform.rotation * backarmOffset;
        Debug.Log(offsetFromBase);
        Debug.Log($"Brick transform:{brick.transform.position}");
        Debug.Log("y:" + y_distance);
        float x_distance = Mathf.Abs(brick.transform.position.z - offsetFromBase.z);
        y_distance -= offsetFromBase.y;
        float hypot_dist = norm(x_distance, y_distance);
        Debug.Log($"x: {x_distance} y:{y_distance} hypot: {hypot_dist}");
        float d1 = Mathf.Rad2Deg * Mathf.Atan2(y_distance, x_distance);
        float c = lawOfCosines(backarmLength, forearmLength, hypot_dist);
        float d2 = lawOfCosines(hypot_dist, backarmLength, forearmLength);
        Debug.Log($"d1: {d1} d2:{d2} c: {c}");

        backArmAngle = d1 + d2;
        foreArmAngle = -(180 - backArmAngle - c);
        Debug.Log("c" + c);
    }

    float norm(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }

    float lawOfCosines(float a , float b, float c)
    {
        return Mathf.Rad2Deg * Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));
    }
}
