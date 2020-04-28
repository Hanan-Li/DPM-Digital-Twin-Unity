using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public GameObject axis;
    public bool isFollow;
    private bool firstTime;
    private Vector3 euleroffset;
    public Transform trans;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        isFollow = false;
        firstTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollow)
        {
            /*//Make brick's position follow robot arm, with an offset for rotation
            trans.position = axis.transform.position + offset;
            if (firstTime)
            {
                euleroffset = axis.transform.eulerAngles - axis.transform.localEulerAngles;
                firstTime = false;
                Debug.Log($" current rotation {trans.eulerAngles}");
                Debug.Log($"axis rotation {axis.transform.eulerAngles}");
                Debug.Log($"offset: {euleroffset}");
            }
            trans.localEulerAngles = axis.transform.eulerAngles + euleroffset;
            //trans.localRotation = Quaternion.Euler(trans.eulerAngles + euleroffset);
            //trans.rotation = axis.transform.rotation;*/
            //trans.position = axis.transform.position + offset;
            //gameObject.transform.parent = axis.transform;
        }
    }

    /// <summary>
    /// Low level wrapper to make this brick follow a robot arm
    /// </summary>
    public virtual void setFollow()
    {
        //StartCoroutine(setFollowDelay());
    }

    /// <summary>
    /// Low level wrapper to make this brick unfollow a robot arm
    /// </summary>
    public void setUnfollow()
    {
        StartCoroutine(setUnFollowDelay());
    }
    public IEnumerator setFollowDelay()
    {
        trans.position = axis.transform.position + offset;
        trans.transform.parent = axis.transform;
        isFollow = true;
        yield return null;
    }

    public IEnumerator setUnFollowDelay()
    {
        isFollow = false;
        trans.transform.parent = null;
        yield return null;
    }
}
