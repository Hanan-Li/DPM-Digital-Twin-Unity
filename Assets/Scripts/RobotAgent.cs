using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RobotAgent : Agent
{
    // Start is called before the first frame update
    private RobotMovementController rmc;
    public GameObject StartingBlock;
    public GameObject EndingBlock;
    public GameObject EndAttachment;
    bool hasPickedUpBlock;
    int steps;
    void Start()
    {
        rmc = GetComponent<RobotMovementController>();
        hasPickedUpBlock = false;
        steps = 0;
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
    }

    public override void AgentReset()
    {
        transform.position = new Vector3(0, -0.36f, 0);
        rmc.ForceStop();
        rmc.foreBackArm.transform.localEulerAngles = new Vector3(90, 0, 180);
        rmc.forearm.transform.localEulerAngles = new Vector3(90,0,0);
        rmc.robotRotator.transform.eulerAngles = new Vector3(359.6f, 180.9f, 181);
        if (hasPickedUpBlock)
        {
            rmc.setUnfollow(rmc.crc);
            StartingBlock.transform.position = new Vector3(17, 1.4f, -5);
            StartingBlock.transform.eulerAngles = Vector3.zero;
        }
    }

    public override void CollectObservations()
    {
        //Add observations to see how far away from block
        AddVectorObs(StartingBlock.transform.position.x - transform.position.x);
        AddVectorObs(StartingBlock.transform.position.z - transform.position.z);

        //Add observations to see how far away from destination
        AddVectorObs(EndingBlock.transform.position.x - transform.position.x);
        AddVectorObs(EndingBlock.transform.position.z - transform.position.z);

        //Add observations to current rotation
        AddVectorObs(rmc.robotRotator.transform.eulerAngles.y);

        //Add observations of Arm
        AddVectorObs(rmc.forearm.transform.localEulerAngles);
        AddVectorObs(rmc.backarm.transform.localEulerAngles);
        //AddVectorObs(rmc.handBaseRotator.transform.localEulerAngles);
        // AddVectorObs(rmc.handrotator.transform.localEulerAngles);
        // AddVectorObs(rmc.circlerotator.transform.localEulerAngles);

        //Add observation for distance of circlerotator
        AddVectorObs(Vector3.Distance(EndAttachment.transform.position, EndingBlock.transform.position));
        AddVectorObs(Vector3.Distance(EndAttachment.transform.position, EndingBlock.transform.position));

        //Add observations of RayCastHit and canPickup
        AddVectorObs(rmc.crc.hasHit);
        AddVectorObs(rmc.crc.canPickup());

        //Add Action Masks
        if (!hasPickedUpBlock && (!rmc.crc.canPickup() || rmc.crc.brickPickup != StartingBlock))
        {
            SetActionMask(5, 1);
            SetActionMask(5, 2);
            if (Vector3.Distance(EndAttachment.transform.position, StartingBlock.transform.position) > 3f)
            {
                SetActionMask(2, 1);
                SetActionMask(2, 2);
                SetActionMask(3, 1);
                SetActionMask(3, 2);
            }
        }
        if(hasPickedUpBlock)
        {
            SetActionMask(5, 1);
        }

        

    }
    public float moveDist;
    public float rotateAngle;
    public override void AgentAction(float[] vectorAction)
    {
        //Action Branch 0 will be movement
        int movementX = Mathf.FloorToInt(vectorAction[0]);
        if(movementX == 1)
        {
            rmc.moveX(moveDist);
        }
        else if(movementX == 2)
        {
            rmc.moveX(-moveDist);
        }

        int movementZ = Mathf.FloorToInt(vectorAction[1]);
        if (movementZ == 1)
        {
            rmc.moveZ(moveDist);
        }
        else if(movementZ == 2)
        {
            rmc.moveZ(-moveDist);
        }

        //Action Branch 1 will be backarm
        int backarm = Mathf.FloorToInt(vectorAction[2]);
        if(backarm == 1)
        {
            rmc.rotateBackarm(rotateAngle);
        }
        else if(backarm == 2)
        {
            rmc.rotateBackarm(-rotateAngle);
        }


        //Action Branch 2 will be backarm
        int forearm = Mathf.FloorToInt(vectorAction[3]);
        if(forearm == 1)
        {
            rmc.rotateForearm(rotateAngle);
        }
        else if(forearm == 2)
        {
            rmc.rotateForearm(-rotateAngle);
        }

        //Action Branch 3 will be robot turning
        int rotate = Mathf.FloorToInt(vectorAction[4]);
        if(rotate == 1)
        {
            rmc.turn(rotateAngle);
        }
        else if(rotate == 2)
        {
            rmc.turn(-rotateAngle);
        }
        


        //Action Branch 5 will be circlerotator
        /*int handend = Mathf.FloorToInt(vectorAction[4]);
        if(handend == 1)
        {
            rmc.rotateHandEnd(rotateAngle);
        }
        else if(handend == 2)
        {
            rmc.rotateHandEnd(-rotateAngle);
        }*/
        

        //Action Branch 6 will be pick up or drop
        int follow = Mathf.FloorToInt(vectorAction[5]);
        if(follow == 1)
        {
            rmc.setFollow(rmc.crc);
        }
        else if(follow == 2)
        {
            rmc.setUnfollow(rmc.crc);
        }


        //Add Rewards
        
        //negative reward for each step
        if (!hasPickedUpBlock)
        {
            if (Vector3.Distance(transform.position, StartingBlock.transform.position) > 20f)
            {
                SetReward(-1f);
                Done();
            }

            if (Vector3.Distance(EndAttachment.transform.position, StartingBlock.transform.position) > 1f)
            {
                AddReward(-Vector3.Distance(EndAttachment.transform.position, StartingBlock.transform.position));
            }
            else
            {
                AddReward(Vector3.Distance(EndAttachment.transform.position, StartingBlock.transform.position));
            }
            if (rmc.crc.brickPickup == StartingBlock)
            {
                hasPickedUpBlock = true;
                AddReward(0.5f);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, EndingBlock.transform.position) > 20f)
            {
                SetReward(-1f);
                Done();
            }
            if (Vector3.Distance(StartingBlock.transform.position, EndingBlock.transform.position) > 1f)
            {
                AddReward(-Vector3.Distance(StartingBlock.transform.position, EndingBlock.transform.position) / 100);
            }
            else
            {
                AddReward(Vector3.Distance(StartingBlock.transform.position, EndingBlock.transform.position) / 100);
            }
            if (follow == 2)
            {
                if(Vector3.Distance(StartingBlock.transform.position, EndingBlock.transform.position) > 0.5f){
                    SetReward(-1f);
                    Done();
                }
                else
                {
                    SetReward(1f);
                    Done();
                }
            }
        }

    }

    public override float[] Heuristic()
    {
        float[] heuristic = new float[6];
        
        steps++;
        /*if(steps % 10 == 0 || steps % 10 ==  1 || steps % 10 == 2 || steps % 10 == 3)
        {
            AgentReset();
        }
        else
        {
            heuristic[3] = 1f;
            //Debug.Log(rmc.robotRotator.transform.eulerAngles);
        }*/
        
        return heuristic;
    }

}
