using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles detection of a brick underneath its arm
/// </summary>
public class CustomRaycaster : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject brickPickup;
    public bool hasHit;
    public float distance;
    private bool canCastRay;
    void Start()
    {
        hasHit = false;
        canCastRay = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Define Layer so that robot can only detect bricks
        int layerMask = 9;
        layerMask = ~layerMask;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.yellow);

        //Checks if ray hits a brick
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask) && canCastRay)
        {
            brickPickup = hit.transform.gameObject;
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            distance = hit.distance;
            hasHit = true;
            //canCastRay = false;
        }
        //Check if robot arm is close enough to brick to automatically pick it up
        /*if(hasHit && distance < 0.05f && canCastRay)
        {
            brickPickup.GetComponent<ObjectFollow>().axis = this.gameObject;
            brickPickup.GetComponent<ObjectFollow>().setFollow();
            canCastRay = false;
            distance = 10;
            
        }*/
    }

    /// <summary>
    /// Internal wrapper that sets the brick to follow the robot arm, effectively picking up the brick
    /// </summary>
    public void setFollow()
    {
        //Check if robot arm is close enough to brick to pick it up
        if (hasHit && distance < 0.2 && canCastRay)
        {
            brickPickup.GetComponent<ObjectFollow>().axis = gameObject;
            brickPickup.GetComponent<ObjectFollow>().setFollow();
            canCastRay = false;
            distance = 10;

        }
    }

    public bool canPickup()
    {
        return hasHit && distance < 0.2 && canCastRay;
    }


    /// <summary>
    /// Internal wrapper that sets the brick to stop following the robot arm, effectively dropping the brick
    /// </summary>
    public void setUnfollow()
    {
        //Debug.Log("Setting Unfollow");
        //Debug.Log(canCastRay);
        brickPickup.GetComponent<ObjectFollow>().setUnfollow();
        StartCoroutine(activateRayCast());
    }

    /// <summary>
    /// Coroutine that activates raycast used for detection bricks
    /// </summary>
    /// <returns></returns>
    IEnumerator activateRayCast()
    {
        yield return new WaitForSeconds(1f);
        canCastRay = true;
    }
}
