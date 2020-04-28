using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculateoffset : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject forebackarm;
    public GameObject forearm;
    public GameObject Hand;
    public float backarmlength;
    public float forearmlength;
    public Vector3 armCenter;
    void Start()
    {
        Vector3 backarmLength = forearm.transform.position - forebackarm.transform.position;
        Vector3 forearmLength = Hand.transform.position - forearm.transform.position;
        backarmlength = backarmLength.magnitude;
        forearmlength = forearmLength.magnitude * 2;
        //Debug.Log($"backarmlength: {backarmLength.magnitude}");
        //Debug.Log($"forearmlength: {forearmLength.magnitude * 2}");
        //Debug.Log($"distance from ground: {forebackarm.transform.position.y}");
    }

    // Update is called once per frame
    void Update()
    {
        armCenter = forebackarm.transform.position;
        //Debug.Log(armCenter);
    }
}
