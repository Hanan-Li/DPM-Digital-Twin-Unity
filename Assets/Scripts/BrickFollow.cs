using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to brick that controls whether or not it is picked up by a robot arm
/// </summary>
public class BrickFollow : ObjectFollow
{

    /// <summary>
    /// Low level wrapper to make this brick follow a robot arm
    /// </summary>
    public override void setFollow()
    {
        trans = transform;
        StartCoroutine(setFollowDelay());
    }

}
