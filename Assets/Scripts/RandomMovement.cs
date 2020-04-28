using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotateAngle = new Vector3(0, 10, 0);
        transform.Rotate(rotateAngle);
        transform.Translate(rotateAngle);

    }
}
