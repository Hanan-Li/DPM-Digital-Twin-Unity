using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridgeRightSeq2 : MonoBehaviour
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
        //yield return new WaitForSeconds(5f);
        float sidesBackArmAngle = 19.73204f;
        float sidesForeArmAngle = 57.573f;
        float secondBack = 41.38668f;
        float secondFore = -13.07807f;
        float destbackArmAngle = 30.75943f;
        float destforeArmAngle = 4.27f;
        float foreArmShortSecond = -11.40f;
        float backArmShortSecond = 47.16f;
        secondFore = -11f;
        float thirdFore = -24.08363f;
        float thirdBack = 47.69112f;
        float[] rowGetBlockBack = new float[2];
        float[] rowGetBlockFore = new float[2];
        float[] backArmArray = new float[3];
        float[] foreArmArray = new float[3];
        backArmArray[1] = destbackArmAngle;
        backArmArray[2] = thirdBack;
        foreArmArray[1] = destforeArmAngle;
        foreArmArray[2] = thirdFore;
        rowGetBlockBack[1] = secondBack;
        rowGetBlockFore[1] = secondFore;
        while (!crc.hasHit)
        {
            yield return null;
        }
        float backArmAngle = 0;
        float foreArmAngle = 0;
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 1.05f);
        backArmArray[0] = backArmAngle;
        foreArmArray[0] = foreArmAngle;
        rowGetBlockBack[0] = backArmAngle;
        rowGetBlockFore[0] = foreArmAngle;

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                rmc.rotateForearm(rowGetBlockFore[j]);
                rmc.rotateBackarm(rowGetBlockBack[j]);
                rmc.setFollow(crc);
                rmc.rotateBackarm(-rowGetBlockBack[j]);
                rmc.rotateForearm(-rowGetBlockFore[j]);
                rmc.moveX(-2 * i);
                if (i == 1)
                {
                    if(j == 0)
                    {
                        rmc.moveZ(-(-1.2f * j + 0.3f));
                    }
                    else
                    {
                        rmc.moveZ(-(-1.2f * j + 0.6f));
                    }
                }
                else
                {
                    rmc.moveZ(1.2f * j);
                }
                rmc.turn(90f);
                rmc.rotateHandEnd(90f);
                rmc.rotateForearm(foreArmArray[i]);
                rmc.rotateBackarm(backArmArray[i]);
                rmc.setUnfollow(crc);
                rmc.rotateBackarm(-backArmArray[i]);
                rmc.rotateForearm(-foreArmArray[i]);
                rmc.turn(-90f);
                if (i == 1)
                {
                    if (j == 0)
                    {
                        rmc.moveZ((-1.2f * j + 0.3f));
                    }
                    else
                    {
                        rmc.moveZ((-1.2f * j + 0.6f));
                    }
                }
                else
                {
                    rmc.moveZ(-1.2f * j);
                }

                if (i < 2)
                {
                    rmc.moveX(2 * (i + 1));
                }
            }
        }

        rmc.moveX(2 * 2 + 1.2f);
        while (!crc.hasHit)
        {
            yield return null;
        }
        //Calculating small block
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 0.4f);
        rmc.rotateForearm(foreArmAngle);
        rmc.rotateBackarm(backArmAngle);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-backArmAngle);
        rmc.rotateForearm(-foreArmAngle);
        rmc.turn(90f);
        rmc.rotateForearm(sidesForeArmAngle);
        rmc.rotateBackarm(sidesBackArmAngle);
        rmc.rotateHandFront(-90f);
        rmc.rotateHandEnd(90f);
        rmc.moveZ(0.6f);
        rmc.moveX(-2 * 2 - 1.2f);
        rmc.setUnfollow(crc);
        rmc.rotateHandFront(90f);
        rmc.rotateHandEnd(-90f);
        rmc.moveZ(-0.6f);
        rmc.turn(-90f);
        rmc.rotateForearm(-sidesForeArmAngle);
        rmc.rotateBackarm(-sidesBackArmAngle);

        for (int j = 0; j < 2; j++)
        {
            rmc.moveX((2 * 2 + 3.2f));
            for (int i = 0; i < 3; i++)
            {
                rmc.rotateForearm(rowGetBlockFore[j]);
                rmc.rotateBackarm(rowGetBlockBack[j]);
                rmc.setFollow(crc);
                rmc.rotateBackarm(-rowGetBlockBack[j]);
                rmc.rotateForearm(-rowGetBlockFore[j]);

                rmc.moveX(-2 * i);
                rmc.moveX(-2 * 2 - 3.2f);
                if (i == 1)
                {
                    rmc.moveZ(-(-2.4f - 1.2f * j + 0.6f));
                }
                else
                {
                    rmc.moveZ(-(-2.4f - 1.2f * j));
                }
                rmc.turn(90f);
                rmc.rotateHandEnd(90f);
                rmc.rotateForearm(foreArmArray[i]);
                rmc.rotateBackarm(backArmArray[i]);
                rmc.setUnfollow(crc);
                rmc.rotateBackarm(-backArmArray[i]);
                rmc.rotateForearm(-foreArmArray[i]);
                rmc.turn(-90f);
                if (i == 1)
                {
                    rmc.moveZ((-2.4f - 1.2f * j + 0.6f));

                }
                else
                {
                    rmc.moveZ((-2.4f - 1.2f * j));
                }
                if (i < 2)
                {
                    rmc.moveX(2 * (i + 1));
                    rmc.moveX((2 * 2 + 3.2f));
                }
            }
        }

        rmc.moveX(2 * 2 + 2f);
        while (!crc.hasHit)
        {
            yield return null;
        }
        //Calculating small block
        calculateNumbers(crc.brickPickup, crc.distance, ref backArmAngle, ref foreArmAngle, 0.4f);
        rmc.rotateForearm(foreArmAngle);
        rmc.rotateBackarm(backArmAngle);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-backArmAngle);
        rmc.rotateForearm(-foreArmAngle);
        rmc.turn(90f);
        rmc.rotateForearm(sidesForeArmAngle);
        rmc.rotateBackarm(sidesBackArmAngle);
        rmc.rotateHandFront(-90f);
        rmc.rotateHandEnd(90f);
        rmc.moveZ(1.8f);
        rmc.moveX(-2 * 2 - 2f);
        rmc.setUnfollow(crc);
        rmc.rotateHandFront(90f);
        rmc.rotateHandEnd(-90f);
        rmc.moveZ(-1.8f);
        rmc.turn(-90f);
        rmc.rotateForearm(-sidesForeArmAngle);
        rmc.rotateBackarm(-sidesBackArmAngle);
        rmc.moveX(2 * 2 + 1.2f);

        rmc.rotateForearm(foreArmShortSecond);
        rmc.rotateBackarm(backArmShortSecond);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-backArmShortSecond);
        rmc.rotateForearm(-foreArmShortSecond);
        rmc.turn(90f);
        rmc.rotateForearm(sidesForeArmAngle);
        rmc.rotateBackarm(sidesBackArmAngle);
        rmc.rotateHandFront(-90f);
        rmc.rotateHandEnd(90f);
        rmc.moveZ(0.6f + 1.2f * 2);
        rmc.moveX(-2 * 2 - 1.2f);
        rmc.setUnfollow(crc);
        rmc.rotateHandFront(90f);
        rmc.rotateHandEnd(-90f);
        rmc.moveZ(-0.6f - 1.2f * 2);
        rmc.turn(-90f);
        rmc.rotateForearm(-sidesForeArmAngle);
        rmc.rotateBackarm(-sidesBackArmAngle);

        rmc.moveX(2 * 2 + 3.2f + 6);

        for (int i = 0; i < 3; i++)
        {
            rmc.rotateForearm(rowGetBlockFore[0]);
            rmc.rotateBackarm(rowGetBlockBack[0]);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-rowGetBlockBack[0]);
            rmc.rotateForearm(-rowGetBlockFore[0]);

            rmc.moveX(-2 * i);
            rmc.moveX(-2 * 2 - 3.2f - 6);
            if (i == 1)
            {
                rmc.moveZ(3.6f + 1.2f - 0.6f);
            }
            else
            {
                rmc.moveZ(3.6f + 1.2f);
            }
            rmc.turn(90f);
            rmc.rotateHandEnd(90f);
            rmc.rotateForearm(foreArmArray[i]);
            rmc.rotateBackarm(backArmArray[i]);
            rmc.setUnfollow(crc);
            rmc.rotateBackarm(-backArmArray[i]);
            rmc.rotateForearm(-foreArmArray[i]);
            rmc.turn(-90f);
            if (i == 1)
            {
                rmc.moveZ((-3.6f - 1.2f + 0.6f));

            }
            else
            {
                rmc.moveZ((-3.6f - 1.2f));
            }
            if (i < 2)
            {
                rmc.moveX(2 * (i + 1));
                rmc.moveX((2 * 2 + 3.2f + 6));
            }
        }

        rmc.moveX(2 * 2 + 1.2f + 0.8f);
        rmc.rotateForearm(foreArmShortSecond);
        rmc.rotateBackarm(backArmShortSecond);
        rmc.setFollow(crc);
        rmc.rotateBackarm(-backArmShortSecond);
        rmc.rotateForearm(-foreArmShortSecond);
        rmc.turn(90f);
        rmc.rotateForearm(sidesForeArmAngle);
        rmc.rotateBackarm(sidesBackArmAngle);
        rmc.rotateHandFront(-90f);
        rmc.rotateHandEnd(90f);
        rmc.moveZ(0.6f + 1.2f * 3);
        rmc.moveX(-2 * 2 - 1.2f - 0.8f);
        rmc.setUnfollow(crc);
        rmc.rotateHandFront(90f);
        rmc.rotateHandEnd(-90f);
        rmc.moveZ(-0.6f - 1.2f * 3);
        rmc.turn(-90f);
        rmc.rotateForearm(-sidesForeArmAngle);
        rmc.rotateBackarm(-sidesBackArmAngle);

        rmc.moveX((2 * 2 + 3.2f + 6));
        for (int i = 0; i < 5; i++)
        {
            rmc.rotateForearm(foreArmShortSecond);
            rmc.rotateBackarm(backArmShortSecond);
            rmc.setFollow(crc);
            rmc.rotateBackarm(-backArmShortSecond);
            rmc.rotateForearm(-foreArmShortSecond);
            rmc.turn(90f);
            rmc.rotateForearm(sidesForeArmAngle);
            rmc.rotateBackarm(sidesBackArmAngle);
            rmc.rotateHandFront(-90f);
            rmc.rotateHandEnd(90f);
            rmc.moveZ(0.6f + 1.2f * (4 + i));
            rmc.moveX(-(4 + 3.2f + 6 + 0.8f * i));
            rmc.setUnfollow(crc);
            rmc.rotateHandFront(90f);
            rmc.rotateHandEnd(-90f);
            rmc.moveZ(-0.6f - 1.2f * (4 + i));
            if (i < 4)
            {
                rmc.moveX((4 + 3.2f + 6 + 0.8f * (i + 1)));
            }
            rmc.turn(-90f);
            rmc.rotateForearm(-sidesForeArmAngle);
            rmc.rotateBackarm(-sidesBackArmAngle);
        }
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
