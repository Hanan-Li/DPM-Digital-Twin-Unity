using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBlock : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hasHit = false;
    private Material mat;
    public int blockNum;
    float startTime;
    float duration;
    bool hasSpoken;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startTime = Time.time;
        duration = 0f;
        hasSpoken = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        if (!hasHit && obj.tag == "centered_long_block" && !obj.GetComponent<ObjectFollow>().isFollow)
        {
            if((obj.transform.parent.position - transform.position).magnitude < 0.5f)
            {
                other.transform.parent.position = transform.position;
                other.transform.parent.rotation = transform.rotation;
                if (other.GetComponentInParent<AzureDevice>())
                {
                    other.GetComponentInParent<AzureDevice>().SetCorrectlyPlaced(true);
                }
                hasHit = true;
                mat.color = Color.green;
                //GetComponent<Outline>().OutlineColor = Color.green;
                duration = Time.time - startTime;
            }
            
        }
        if (!hasHit && obj.tag == "centered_short_block" && !obj.GetComponent<ObjectFollow>().isFollow)
        {
            if ((obj.transform.parent.position - transform.position).magnitude < 0.5f)
            {
                other.transform.parent.position = transform.position;
                other.transform.parent.rotation = transform.rotation;
                hasHit = true;
                mat.color = Color.green;
                //GetComponent<Outline>().OutlineColor = Color.green;
                duration = Time.time - startTime;
            }

        }
        if (!hasHit && obj.tag == "centered_female_block" && !obj.GetComponent<ObjectFollow>().isFollow)
        {
            if ((obj.transform.parent.position - transform.position).magnitude < 0.5f)
            {
                other.transform.parent.position = transform.position;
                other.transform.parent.rotation = transform.rotation;
                hasHit = true;
                mat.color = Color.green;
                //GetComponent<Outline>().OutlineColor = Color.green;
                duration = Time.time - startTime;
            }

        }
        if (!hasHit && obj.tag == "centered_male_block" && !obj.GetComponent<ObjectFollow>().isFollow)
        {
            if ((obj.transform.parent.position - transform.position).magnitude < 0.2f)
            {
                other.transform.parent.position = transform.position;
                other.transform.parent.rotation = transform.rotation;
                hasHit = true;
                mat.color = Color.green;
                //GetComponent<Outline>().OutlineColor = Color.green;
                duration = Time.time - startTime;
            }

        }
        if(duration != 0 && !hasSpoken)
        {
            Debug.Log($"Block Number: {blockNum}, time : {duration}");
            hasSpoken = true;
        }
    }


}
