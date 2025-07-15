using System;
using System.Collections;
using System.Text;
using System.Threading;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class DynDNSUpdater : MonoBehaviour
{
    [SerializeField]
    private IPManager ipManager;
    [SerializeField]
    private ConfigManager configManager;

    private float currentDelay = 0;

    private void Update()
    {
        currentDelay += Time.deltaTime;

        if(currentDelay >= configManager.GetConfig().UpdateInterval)
        {
            if (string.IsNullOrEmpty(ipManager.GetIP()))
            {
                Debug.LogError("No IP Fetched. Could not Update");
            }
            else
            {
                UpdateIP();
            }
            
            currentDelay = 0;
        }
    }

    private void UpdateIP()
    {
        Debug.Log("Update IP");
        StartCoroutine(SetIP());
    }

    IEnumerator SetIP()
    {
        string uri = configManager.GetConfig().ServiceURL;
        uri = uri.Replace("$HOSTNAME", configManager.GetConfig().HostName);
        uri = uri.Replace("$IP", ipManager.GetIP());

        Debug.Log("URL : " + uri);

        //myfriendbob.fr-bob:BobUpdatePass1
        //bXlmcmllbmRib2IuZnItYm9iOkJvYlVwZGF0ZVBhc3Mx

        int charIndex = configManager.GetConfig().HostName.IndexOf(".");
        string domain = configManager.GetConfig().HostName.Remove(0, charIndex + 1);
        string authorization = domain + "-" + configManager.GetConfig().UserName + ":" + configManager.GetConfig().Password;

        //byte[] plainTextBytes = Encoding.UTF8.GetBytes(authorization);
        byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(authorization);

        string encodedCredential = Convert.ToBase64String(plainTextBytes);

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        Debug.Log("Basic " + encodedCredential);

        //webRequest.Headers.Add("Authorization", "Basic " + svcCredentials);

        webRequest.SetRequestHeader("Authorization", "Basic " + encodedCredential);

        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received : " + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.LogError("ERROR : " + webRequest.error);
        }

        /*using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received : " + webRequest.downloadHandler.text);
                    break;
                default:
                    Debug.LogError("ERROR : " + webRequest.error);
                    break;

            }
        }*/
    }
}
