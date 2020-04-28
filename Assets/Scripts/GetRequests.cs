using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:8000/api/data"));

    }
    [Serializable]
    public class Data
    {
        public int  robot_back_angle;
        public int robot_fore_angle;
        public int robot_loc;
        public int turn_robot;
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
                serverData = JsonUtility.FromJson<Data>(data);
                Debug.Log(serverData.robot_loc);
                RobotMovementController.instance.moveZ(serverData.robot_loc);
                RobotMovementController.instance.rotateBackarm(serverData.robot_back_angle);
                RobotMovementController.instance.rotateForearm(serverData.robot_fore_angle);
                RobotMovementController.instance.turn(serverData.turn_robot);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
