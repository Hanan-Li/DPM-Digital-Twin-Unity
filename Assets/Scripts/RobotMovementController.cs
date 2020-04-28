using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that handles scheduling of robot's movements, picking up bricks, and dropping bricks
/// </summary>
public class RobotMovementController : MonoBehaviour
{
    public CustomRaycaster crc;
    float speed = -2;
    float rotSpeed = 40;
    float rot = 4;
    float rotoffset = 4;
    float gravity = 4;
    float currRotation = 90f;
    float currForeRotation = 0f;
    float currRobotRotation = 0f;
    float currHandFrontRotation = 90f;
    private bool firstRot;
    public static RobotMovementController instance;
    Vector3 moveDir = Vector3.zero;

    CharacterController controller;

    public GameObject robotBase;
    public GameObject backarm;
    public GameObject forearm;

    public GameObject handrotator;
    public GameObject handBaseRotator;
    public GameObject circlerotator;

    public GameObject robotRotator;
    public GameObject foreBackArm;
    Vector3 forearmOffset = new Vector3(-0.33f, 1.0f, -5.15f);
    Vector3 backarmOffset = new Vector3(0.5f, 1.5f, -1.0f);

    private bool moveForearmForwards;
    private bool moveForearmBackwards;
    private bool moveBackarmForwards;
    private bool moveBackarmBackwards;
    private bool rotateBase;
    private bool moveRobotForwards;
    private bool moveRobotBackwards;

    private bool forceStop;
    
    struct Operation
    {
        public string direction;
        public float parameter;
        public bool waitForEnd;
    }
    private Queue<Operation> moveOperations = new Queue<Operation>();
    private bool doingOp;
    private bool waiting;
    private int numOps;

    // Start is called before the first frame update

    private void Awake()
    {
        /*if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }*/
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        doingOp = false;
        firstRot = true;
        waiting = false;
        forceStop = false;
        numOps = 0;
    }

    private void Update()
    {
        if(moveOperations.Count != 0 && !waiting)
        {
            Operation op = moveOperations.Dequeue();
            if (op.waitForEnd)
            {
                StartCoroutine(waitUntilFree(op));
            }
            else
            {
                doMove(op);
            }
        }
    }

    
    private void doMove(Operation op)
    {
        numOps++;
        doingOp = true;
        if (op.direction == "forwards")
        {
            StartCoroutine(forwards(op.parameter));
            
        }
        else if(op.direction == "sideways")
        {
            StartCoroutine(sideways(op.parameter));
        }
        else if(op.direction == "turn")
        {
            StartCoroutine(turnMachine(op.parameter));
        }
        else if(op.direction == "rotateFore")
        {
            StartCoroutine(rotateForeArm(op.parameter));
        }
        else if(op.direction == "rotateBack")
        {
            StartCoroutine(rotateBackArm(op.parameter));
        }
        else if(op.direction == "unfollow")
        {
            StartCoroutine(unfollow());
        }
        else if(op.direction == "follow")
        {
            StartCoroutine(follow());
        }
        else if(op.direction == "rotateHandEnd")
        {
            StartCoroutine(rotateEndHand(op.parameter));
        }
        else if(op.direction == "pause")
        {
            StartCoroutine(pause(op.parameter));
        }
        else if(op.direction == "rotateHandBase")
        {
            StartCoroutine(rotateBaseHand(op.parameter));
        }
        else if (op.direction == "rotateHandFront")
        {
            StartCoroutine(rotateFrontHand(op.parameter));
        }
    }


    #region COMMANDS_INTERFACE
    public bool isEmpty()
    {
        return moveOperations.Count == 0;
    }

    public void ForceStop()
    {
        moveOperations.Clear();
        forceStop = true;
        doingOp = false;
        firstRot = true;
        waiting = false;
        forceStop = false;
        numOps = 0;
    }

    /// <summary>
    /// Command that rotates hand's X axis
    /// </summary>
    /// <param name="angle">raycaster for robot</param>
    public void rotateHandFront(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "rotateHandFront";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that rotates hand's Y axis
    /// </summary>
    /// <param name="angle">raycaster for robot</param>
    public void rotateHandBase(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "rotateHandBase";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that rotates hand's end axis
    /// </summary>
    /// <param name="angle">raycaster for robot</param>
    public void rotateHandEnd(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "rotateHandEnd";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that pauses the robot's movement
    /// </summary>
    /// <param name="duration">raycaster for robot</param>
    public void Pause(float duration)
    {
        Operation op = new Operation();
        op.direction = "pause";
        op.parameter = duration;
        op.waitForEnd = true;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to pick up a brick
    /// </summary>
    /// <param name="raycast">raycaster for robot</param>
    public void setFollow(CustomRaycaster raycast, bool waitForEnd = true)
    {
        crc = raycast;
        Operation op = new Operation();
        op.direction = "follow";
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to drop a brick
    /// </summary>
    public void setUnfollow(CustomRaycaster raycast, bool waitForEnd = true)
    {
        crc = raycast;
        Operation op = new Operation();
        op.direction = "unfollow";
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to turn a certain angle
    /// </summary>
    /// <param name="angle">Float angle for robot to turn in degrees</param>
    public void turn(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "turn";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to move backwards by some distance
    /// </summary>
    /// <param name="distance">Float distance for robot to move</param>
    public void moveBackwards(float distance, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "backwards";
        op.parameter = distance;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to move forwards by some distance
    /// </summary>
    /// <param name="distance">Float distance for robot to move</param>
    public void moveZ(float distance, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "forwards";
        op.parameter = distance;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    public void moveX(float distance, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "sideways";
        op.parameter = distance;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);

    }

    /// <summary>
    /// Command that schedules the robot to rotate the backarm to a set angle with respect to the horizontal ground
    /// </summary>
    /// <param name="angle">angle to rotate backarm by</param>
    public void rotateBackarm(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "rotateBack";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    /// <summary>
    /// Command that schedules the robot to rotate the foream to a set angle, with respect to the backarm
    /// </summary>
    /// <param name="angle">angle to rotate forearm by</param>
    public void rotateForearm(float angle, bool waitForEnd = true)
    {
        Operation op = new Operation();
        op.direction = "rotateFore";
        op.parameter = angle;
        op.waitForEnd = waitForEnd;
        moveOperations.Enqueue(op);
    }

    #endregion

    #region COROUTINES
    IEnumerator waitUntilFree(Operation op)
    {
        waiting = true;
        while (numOps > 0)
        {
            yield return null;
        }
        waiting = false;
        doMove(op);
        yield return null;
    }

    IEnumerator rotateFrontHand(float angle)
    {
        float time = Mathf.Abs(angle / rotSpeed);
        int dir = (int)(Mathf.Abs(angle) / angle);
        Vector3 rotationDir = new Vector3(dir, 0, 0);
        Vector3 init = handrotator.transform.eulerAngles;
        Vector3 end = init + new Vector3(angle, 0, 0);
        currHandFrontRotation += angle;
        end = new Vector3(currHandFrontRotation, 180, 0);
        //Debug.Log.Log(end);
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            handrotator.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime, relativeTo: Space.Self);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        handrotator.transform.localEulerAngles = end;
        doingOp = false;
        numOps--;
        yield return null;
    }
    IEnumerator rotateBaseHand(float angle)
    {
        float time = Mathf.Abs(angle / rotSpeed);
        int dir = (int)(Mathf.Abs(angle) / angle);
        Vector3 rotationDir = new Vector3(0, 0, dir);
        Vector3 init = handBaseRotator.transform.localEulerAngles;
        Vector3 end = init + new Vector3(0, 0, angle);

        for (float t = 0; t < time; t += Time.deltaTime)
        {
            handBaseRotator.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime, Space.World);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        handBaseRotator.transform.localEulerAngles = end;
        doingOp = false;
        numOps--;
        yield return null;
    }
    IEnumerator pause(float duration)
    {
        waiting = true;
        yield return new WaitForSeconds(duration);
        doingOp = false;
        numOps--;
        waiting = false;
        yield return null;
    }

    IEnumerator forwards(float distance)
    {
        float time = Mathf.Abs(distance / speed);
        int dir = (int)(Mathf.Abs(distance) / distance);
        Vector3 dest = transform.position - new Vector3(0, 0, distance);
        for (; time > 0; time -= Time.deltaTime)
        {
            moveDir = new Vector3(0, 0, dir);
            moveDir = transform.TransformDirection(moveDir * speed);
            moveDir.y -= gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        moveDir = Vector3.zero;
        transform.position = dest;
        doingOp = false;
        numOps--;
        yield return null;
    }

    IEnumerator sideways(float distance)
    {
        float time = Mathf.Abs(distance / speed);
        int dir = (int)(Mathf.Abs(distance) / distance);
        Vector3 dest = transform.position - new Vector3(distance, 0, 0);
        for (; time > 0; time -= Time.deltaTime)
        {
            moveDir = new Vector3(dir, 0, 0);
            moveDir = transform.TransformDirection(moveDir * speed);
            moveDir.y -= gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        moveDir = Vector3.zero;
        transform.position = dest;
        doingOp = false;
        numOps--;
        yield return null;
    }

    IEnumerator rotateEndHand(float angle)
    {
        
        float time = Mathf.Abs(angle / rotSpeed);
        int dir = (int)(Mathf.Abs(angle) / angle);
        Vector3 rotationDir = new Vector3(0, 0, dir);
        Vector3 init = circlerotator.transform.eulerAngles;
        Vector3 end = circlerotator.transform.eulerAngles + new Vector3(0, 0, angle);

        for(float t = 0; t < time; t += Time.deltaTime)
        {
            circlerotator.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        circlerotator.transform.eulerAngles = end;
        doingOp = false;
        numOps--;
        yield return null;
    }

    //Rotate arm To angle
    IEnumerator rotateBackArm(float angle)
    {
        int dir = (int)(Mathf.Abs(angle) / angle);
        Vector3 rotationDir = transform.TransformDirection(dir, 0, 0);
        float time = Mathf.Abs(angle)/rotSpeed;
        Vector3 init = foreBackArm.transform.localRotation.eulerAngles;
        Vector3 destforeBackArmEuler;
        Vector3 destHandRot = handrotator.transform.localEulerAngles;
        //Debug.Log.Log($"BACK: initial backarm rot: {init}");
        /*if (dir == -1)
        {
            destforeBackArmEuler = init + new Vector3(dir * angle, 0, 0);
        }
        else
        {
            destforeBackArmEuler = init + new Vector3(-dir * angle, 0, 0);
        }*/

        
        if(Mathf.Abs(init.y-180f) < 0.5f)
        {
            init.x = 180f - init.x;
            init.y = 0f;
            init.z = 180f;
        }
        //Debug.Log.Log($"BACK: initial backarm rot modified: {init}");
        destforeBackArmEuler = init +  new Vector3(-angle,0, 0);
        //Debug.Log.Log($"BACK: initial backarm rot: {destforeBackArmEuler}");
        if (destHandRot.z == 180f)
        {
            destHandRot.x = 180f - destHandRot.x;
            destHandRot.y = 180f;
            destHandRot.z = 0f;
        }
        currHandFrontRotation -= angle;
        destHandRot = new Vector3(currHandFrontRotation, 180, 0);
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            ////Debug.Log.Log(foreBackArm.transform.rotation.eulerAngles);
            foreBackArm.transform.Rotate(dir*rotSpeed * Time.deltaTime, 0f, 0f);
            handrotator.transform.Rotate(-rotationDir, rotSpeed * Time.deltaTime, relativeTo: Space.Self);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        foreBackArm.transform.localEulerAngles = destforeBackArmEuler;
        handrotator.transform.localEulerAngles = destHandRot;
        doingOp = false;
        numOps--;
        yield return null;
    }

    IEnumerator rotateForeArm(float angle)
    {

        int dir = (int)(Mathf.Abs(angle) / angle);
        Vector3 rotationDir = transform.TransformDirection(dir, 0, 0);
        float time = Mathf.Abs(angle) / rotSpeed;
        Vector3 init = forearm.transform.localEulerAngles;
        if(init.y == 180f && init.z == 180f)
        {
            init.x = 180 - init.x;
            init.y = 0f;
            init.z = 0f;
        }
        Vector3 dest;
        Vector3 destHandRot = handrotator.transform.localEulerAngles;
        
        if (destHandRot.z == 180f)
        {
            destHandRot.x = 180f - destHandRot.x;
            destHandRot.y = 180f;
            destHandRot.z = 0f;
        }
        currHandFrontRotation -= angle;
        destHandRot = new Vector3(currHandFrontRotation, 180, 0);
        if (dir == 1)
        {
            dest = init + new Vector3(dir * angle, 0f, 0f);
            
        }
        else
        {
            dest = init + new Vector3(-dir * angle, 0f, 0f);
        }
        //destHandRot +=  new Vector3(-angle, 0, 0);
        dest.y = 0f;
        dest.z = 0f;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            forearm.transform.Rotate(rotationDir, rotSpeed * Time.deltaTime);
            handrotator.transform.Rotate(-rotationDir, rotSpeed * Time.deltaTime, relativeTo: Space.Self);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        forearm.transform.localEulerAngles = dest;
        handrotator.transform.localEulerAngles = destHandRot;
        doingOp = false;
        numOps--;
        yield return null;

    }

    IEnumerator turnMachine(float angle)
    {
        currRobotRotation += angle;
        float time = Mathf.Abs(angle)/ rotSpeed;
        float dir = 1;
        if(angle < 0)
        {
            dir = -1;
        }
        Vector3 destrot = robotRotator.transform.eulerAngles + new Vector3(0, angle, 0);
        Vector3 startrot = robotRotator.transform.eulerAngles;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            
            rot = dir * rotSpeed * Time.deltaTime;
            robotRotator.transform.eulerAngles += new Vector3(0, rot, 0);
            moveDir.y -= gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        robotRotator.transform.eulerAngles = destrot;
        doingOp = false;
        numOps--;
        yield return null;
    }

    IEnumerator backwards(float distance)
    {
        ////Debug.Log.Log("Coroutine started");
        float time = Mathf.Abs(distance / speed);
        ////Debug.Log.Log(time);
        for (; time >= 0; time -= Time.deltaTime)
        {
            moveDir = new Vector3(0, 0, -1);
            moveDir = transform.TransformDirection(moveDir * speed);
            moveDir.y -= gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            if (forceStop)
            {
                yield break;
            }
            yield return null;
        }
        moveDir = Vector3.zero;
        doingOp = false;
        numOps--;
        yield return null;
    }

    IEnumerator unfollow()
    {
        crc.setUnfollow();
        if (forceStop)
        {
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        doingOp = false;
        numOps--;
    }

    IEnumerator follow()
    {
        crc.setFollow();
        if (forceStop)
        {
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        doingOp = false;
        numOps--;
    }

    #endregion
}
