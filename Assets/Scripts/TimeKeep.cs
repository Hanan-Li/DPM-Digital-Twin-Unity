using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeep : MonoBehaviour
{
    // Start is called before the first frame update
    float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
