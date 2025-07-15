using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class IPManager : MonoBehaviour
{
    [SerializeField]
    private string IPCheckURL;

    private string IP;

    private void Awake()
    {
        StartCoroutine(GetIP(IPCheckURL));
    }

    public string GetIP()
    {
        return IP;
    }

    IEnumerator GetIP(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

 
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    SaveIP(webRequest.downloadHandler.text);
                    break;
                default:
                    Debug.LogError("ERROR : " + webRequest.error);
                    break;

            }
        }
    }

    private void SaveIP(string rawData)
    {
        rawData = rawData.Split(":")[1];
        rawData = rawData.Split("<")[0];
        IP = rawData.Replace(" ", "");

        Debug.Log("IP fetched : " + IP);
    }
}
