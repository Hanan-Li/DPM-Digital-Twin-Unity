using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensor;
using System.Linq;

public class SimpleRobotAgent : Agent
{
    public Transform FinalBrickLocation;
    public Transform CorrectTransform;
    public float speed;
    public float rewardFactor;
    Vector3 correctPos;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public override void AgentReset() {
        transform.position = new Vector3(1.2f, -0.36f, 0f);

    }

    public override void CollectObservations()
    {
        AddVectorObs(FinalBrickLocation.position.x - CorrectTransform.position.x);
        AddVectorObs(FinalBrickLocation.position.z - CorrectTransform.position.z);
    }

    public override void AgentAction(float[] vectorAction)
    {
        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];
        Vector3 move = new Vector3(horizontal, 0, vertical);
        transform.Translate(move.normalized * speed * Time.deltaTime);
        //rb.AddForce(move * speed);
        //correctPos = new Vector3(CorrectTransform.position.x, CorrectTransform.position.y + 1, CorrectTransform.position.z);
        //Collider[] hitObjects = Physics.OverlapBox(correctPos,
        //                                   new Vector3(0.4f, 1f, 0.6f));
        //move.y = CorrectTransform.position.y + 1;
       float distance = Vector3.Distance(FinalBrickLocation.position, CorrectTransform.position);
        AddReward(-distance/8000);
        //AddReward(-0.0002f);

        if (Vector3.Distance(FinalBrickLocation.position, CorrectTransform.position) > 40)
        {
            SetReward(-1f);
            Done();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        Debug.Log("collided");
        if(obj.tag == "obstacle")
        {
            SetReward(-1f);
            Done();
            obj.GetComponent<ResetObject>().reset();

        }
        else if (obj.tag == "Brick")
        {
            SetReward(1f);
            Done();
            obj.GetComponent<ResetObject>().changePos();

        }
    }
    public override float[] Heuristic()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log(horizontal);
        float[] res = new float[2];
        res[0] = horizontal;
        res[1] = vertical;
        return res;
    }
}
