using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPos;
    Vector3 initalRot;
    int count;
    public bool rotate;
    Vector3[] innerPositions;
    Vector3[] positions;
    RandomRotation rr;
    void Start()
    {
        initialPos = transform.position;
        initalRot = transform.eulerAngles;
        positions = new[]{ new Vector3(-2.39f, 0, -17),
            new Vector3(-2.39f, 0, 17),
            new Vector3(17, 0, 0),
            new Vector3(-17, 0, 0)
            
        };
        innerPositions = new[]{ new Vector3(9, 0, -9),
            new Vector3(9, 0, 9),
            new Vector3(-11, 0, 9),
            new Vector3(-11, 0, -9)
        };
        count = 0;
        if (rotate)
        {
            rr = GetComponentInParent<RandomRotation>();
        }
    }

    public void reset()
    {
        if (!rotate)
        {
            transform.position = initialPos;
            transform.eulerAngles = initalRot;
        }
        else
        {
 
            rr.speed = Random.Range(-5, 5);
        }

    }

    public void changePos()
    {
        if (!rotate)
        {
            int selection = Random.Range(0, 4);
            if (count < 20)
            {
                Vector3 nextPos = innerPositions[selection];
                transform.parent.position = nextPos;
                transform.parent.eulerAngles = initalRot;
            }
            else
            {
                Vector3 nextPos = positions[selection];
                transform.parent.position = nextPos;
                transform.parent.eulerAngles = initalRot;
            }
            count += 1;
        }
        else
        {
            int side = Random.Range(0, 4);
            float range = Random.Range(-14, 14.1f);
            Vector3 nextPos = Vector3.zero;
            if (side == 0)
            {
                nextPos.x = -16f;
                nextPos.z = range;
            }
            else if (side == 1)
            {
                nextPos.x = 16f;
                nextPos.z = range;
            }
            else if(side == 2)
            {
                nextPos.z = -16f;
                nextPos.x = range;
            }
            else
            {
                nextPos.z = 16f;
                nextPos.x = range;
            }
            transform.parent.position = nextPos;
        }

    }
}
