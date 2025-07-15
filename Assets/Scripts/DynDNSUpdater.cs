using System;
using System.Collections;
using System.Text;
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
        StartCoroutine(SetIP());
    }

    IEnumerator SetIP()
    {
        //Create URL
        string uri = configManager.GetConfig().ServiceURL;
        uri = uri.Replace("$HOSTNAME", configManager.GetConfig().HostName);
        uri = uri.Replace("$IP", ipManager.GetIP());

        //Create Credentials
        int charIndex = configManager.GetConfig().HostName.IndexOf(".");
        string domain = configManager.GetConfig().HostName.Remove(0, charIndex + 1);
        string authorization = domain + "-" + configManager.GetConfig().UserName + ":" + configManager.GetConfig().Password;

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(authorization);

        string encodedCredential = Convert.ToBase64String(plainTextBytes);

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        webRequest.SetRequestHeader("Authorization", "Basic " + encodedCredential);

        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("IP Update OK : " + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.LogError("ERROR : " + webRequest.error);
        }
    }
}
