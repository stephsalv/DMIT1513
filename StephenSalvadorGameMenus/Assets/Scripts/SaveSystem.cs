using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Profiling;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public SaveProfile profileData;
    [SerializeField] private string fileName = "saveData.json";

    public List<SaveProfile> profiles = new List<SaveProfile>();

    public string filePath;
    string profileName;
    public TMP_InputField profileNameInput;

    public SaveData saveData = new SaveData();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadData();
    }
    [ContextMenu("JSON Save")]
    public void SaveData()
    {
        SaveProfile saveProfile = new SaveProfile("Stephen", 1, "GTR");
        string file = filePath + profileName + ".json";
        string json = JsonUtility.ToJson(saveProfile, true);

        File.WriteAllText(filePath, json);

    }
    public void SaveData(SaveData profile_)
    {
        string file = filePath + profile_.profiles + ".json";
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved successfully!");
    }
    public void SaveNewProfile(string vehicleName)
    {
        if (profileNameInput == null)
        {
            Debug.LogError("InputField is empty!");
            return;
        }

        string enteredName = profileNameInput.text.Trim();
        if (string.IsNullOrEmpty(enteredName))
        {
            Debug.LogWarning("Please enter a profile name!");
            return;
        }
        SaveProfile newProfile = new SaveProfile(enteredName, 0f, vehicleName);

        saveData.profiles.Add(newProfile);

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Profile '{enteredName}' with vehicle '{vehicleName}' saved successfully!");
    }

    [ContextMenu("JSON Load")]
    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded");
        }
        else
        {
            Debug.Log("No save file found, creating new one");
            saveData = new SaveData();
            return;
        }
    }
    [ContextMenu("JSON Delete")]
    public void DeleteData(SaveData profile_)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded");
        }
        else
        {
            Debug.Log("No save file found, creating new one");
            saveData = new SaveData();
            return;
        }
    }
}

[Serializable]
public class SaveProfile
{
    public string profileName;
    public float bestTime;
    public string vehicle;
    //public Color color;
    GhostData GhostData;

    public SaveProfile(string profileName_, float bestTime_,string vehicle_)
    {
        profileName = profileName_;
        bestTime = bestTime_;
        vehicle = vehicle_;
    }
}
[Serializable]
public class SaveData
{
    public List<SaveProfile> profiles = new List<SaveProfile>();
}


