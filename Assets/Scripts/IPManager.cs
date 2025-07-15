using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class IPManager : MonoBehaviour
{
    [SerializeField]
    private string IPCheckURL;

    public void GetIP(Action<string> callback)
    {
        StartCoroutine(GetIPCoroutine(callback));
    }

    IEnumerator GetIPCoroutine(Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(IPCheckURL))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

 
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    callback.Invoke(FormatIP(webRequest.downloadHandler.text));
                    break;
                default:
                    Debug.LogError("ERROR : " + webRequest.error);
                    callback.Invoke(null);
                    break;
            }
        }
    }

    private string FormatIP(string rawData)
    {
        int charIndex = rawData.IndexOf(":");
        rawData = rawData.Remove(0, charIndex + 1);
        rawData = rawData.Split("<")[0];
        string formatedIP = rawData.Replace(" ", "");

        return formatedIP;
    }
}
