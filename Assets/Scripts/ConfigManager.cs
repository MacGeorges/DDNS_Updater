using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [SerializeField]
    private string configFilePath;

    [SerializeField] // Display for debug and setup default
    private ConfigFile configFile;

    private void Awake()
    {
        if (!File.Exists(configFilePath))
        {
            CreateConfigFile();
        }
        else
        {
            LoadConfigFile();
        }
    }

    public ConfigFile GetConfig()
    {
        return configFile;
    }

    private void CreateConfigFile()
    {
        string blankConfigFile = JsonUtility.ToJson(configFile);
        File.WriteAllText(configFilePath, blankConfigFile);

        Debug.Log("Blank Config File Created");
    }

    private void LoadConfigFile()
    {
        string loadedConfigFile = File.ReadAllText(configFilePath);
        configFile = JsonUtility.FromJson<ConfigFile>(loadedConfigFile);

        Debug.Log("Config File Loaded");
    }
}
