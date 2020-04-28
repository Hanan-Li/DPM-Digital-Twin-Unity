using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFollow : ObjectFollow
{
    public override void setFollow()
    {
        //Debug.Log("new");
        trans = transform.parent;
        offset = new Vector3(0, -0.4f, 0);
        StartCoroutine(setFollowDelay());
    }
}