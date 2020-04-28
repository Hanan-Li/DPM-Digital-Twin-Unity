using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CommandGetter : MonoBehaviour
{
    // Start is called before the first frame update
    RobotMovementController rmc;
    private calculateoffset offsets;
    float backArmAngle;
    float foreArmAngle;

    void Start()
    {
        rmc = GetComponent<RobotMovementController>();
        offsets = GetComponent<calculateoffset>();
        StartCoroutine(GetRequest("http://localhost:8000/api/get_data_dt"));
    }

    [Serializable]
    public class Data
    {
        public string type;
        public string ipfsAddr;
        public int x;
        public int y;
        public int z;
    }
    IEnumerator GetRequest(string uri)
    {
        while (true)
        {
            // Request and wait for the desired page.
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();

            string data = webRequest.downloadHandler.text;
            Data serverData = new Data();
            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(data);
                if(data[5] != 'N')
                {
                    serverData = JsonUtility.FromJson<Data>(data);
                    if(serverData.type == "robot")
                    {
                        if(serverData.y == 0)
                        {
                            rmc.moveX(transform.position.x - serverData.x);
                            rmc.moveZ(transform.position.z - serverData.z);
                        }
                        else if(serverData.y == 1)
                        {
                            yield return StartCoroutine(Pickup());
                        }
                        else if(serverData.y == -1)
                        {
                            yield return StartCoroutine(DropOff());
                        }
                        
                    }
                }
                
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Pickup()
    {
        calculateNumbers(rmc.crc.brickPickup, rmc.crc.distance, ref backArmAngle, ref foreArmAngle, 1.05f);
        rmc.rotateBackarm(backArmAngle, false);
        rmc.rotateForearm(foreArmAngle, false);
        rmc.setFollow(rmc.crc, true);
        rmc.rotateForearm(-foreArmAngle, false);
        rmc.rotateBackarm(-backArmAngle, false);
        yield return null;
    }

    IEnumerator DropOff()
    {
        rmc.rotateBackarm(backArmAngle, false);
        rmc.rotateForearm(foreArmAngle, false);
        rmc.setUnfollow(rmc.crc, true);
        rmc.rotateForearm(-foreArmAngle, false);
        rmc.rotateBackarm(-backArmAngle, false);
        yield return null;
    }

    void calculateNumbers(GameObject brick, float y_distance, ref float backArmAngle, ref float foreArmAngle, float y_offset)
    {

        float x_distance = Mathf.Abs(brick.transform.parent.position.z - offsets.armCenter.z);
        y_distance = brick.transform.parent.position.y - offsets.armCenter.y + y_offset;
        //Debug.Log($"ydist: {y_distance}");
        //Debug.Log($"xdist:{x_distance}");
        //Debug.Log(brick.transform.parent.name);
        float hypot_dist = norm(x_distance, y_distance);
        // Debug.Log($"x_dist: {x_distance}, y_dist: {y_distance}, hypot_dist: {hypot_dist}");
        float d1 = Mathf.Rad2Deg * Mathf.Atan2(y_distance, x_distance);
        float c = lawOfCosines(offsets.backarmlength, offsets.forearmlength, hypot_dist);
        float d2 = lawOfCosines(hypot_dist, offsets.backarmlength, offsets.forearmlength);
        //Debug.Log(d1 + d2);
        //Debug.Log(c);
        backArmAngle = 90 - (d1 + d2);
        foreArmAngle = 90 - c;
        //Debug.Log(backArmAngle);
        //Debug.Log(foreArmAngle);
    }

    float norm(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }

    float lawOfCosines(float a, float b, float c)
    {
        return Mathf.Rad2Deg * Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));
    }
}
