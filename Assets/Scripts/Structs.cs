using UnityEngine;

public class Structs{}

[System.Serializable]
public struct ConfigFile
{
    public string ServiceURL;
    public string HostName;
    public string UserName;
    public string Password;
    public int UpdateInterval;
}