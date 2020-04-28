using System;
using System.Net;
using System.IO;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;

public class AzureDevice : MonoBehaviour
{
    private readonly string s_connectionString = "HostName=DigitalTwinDPM.azure-devices.net;DeviceId=UnitySimulatedBlock1;SharedAccessKey=QV9nl6kkmXqoR9BLiMWmlsFGOo11sY65d/nYBR4pTp4=";
    private readonly string sastoken = "SharedAccessSignature sr=DigitalTwinDPM.azure-devices.net%2Fdevices%2FUnitySimulatedBlock1&sig=tYYunPdOsMOS9ZlDMaD4RgbtI4vo4UIUXIlY5%2FD0vd8%3D&se=1604559945";
    private readonly string api_addr = "https://DigitalTwinDPM.azure-devices.net/devices/UnitySimulatedBlock1/messages/events?api-version=2018-06-30";
    private Rigidbody rb;
    private bool correctlyPlaced;
    public int blocknum;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SendInfo());
        correctlyPlaced = false;

    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SendInfo()
    {
        while (true)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_addr);
            request.Headers.Add("Authorization", sastoken);
            string placed = "false";
            if (correctlyPlaced)
            {
                placed = "true";
            }
            string postdata = $"{{ \"block_num\" : {blocknum}, \"x\" : {(int)transform.position.x}, \"y\" : {(int)transform.position.y} , \"z\" : {(int)transform.position.z}, \"placed\" : {placed}  }}";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(postdata);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.ContentLength = byte1.Length;

            Stream newStream = request.GetRequestStream();

            newStream.Write(byte1, 0, byte1.Length);


            // Close the Stream object.
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            Debug.Log(resStream);
            yield return new WaitForSeconds(2f);
        }
        
    }

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

    public void SetCorrectlyPlaced(bool hasPlaced)
    {
        correctlyPlaced = hasPlaced;
    }
}
