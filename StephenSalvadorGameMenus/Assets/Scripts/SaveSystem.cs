using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public string filePath;

    public void CreateSaveData(SaveProfile saveProfile)
    {
        string file = Path.Combine(filePath, saveProfile.profileName + ".json");
        string json = JsonUtility.ToJson(saveProfile, true);
        File.WriteAllText(file, json);
        Debug.Log($"Saved profile: {saveProfile.profileName} at {file}");
    }

    public SaveProfile LoadSaveData(string profileName)
    {
        string file = Path.Combine(filePath, profileName + ".json");

        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            SaveProfile profile = JsonUtility.FromJson<SaveProfile>(json);
            Debug.Log($"Loaded profile: {profileName}");
            return profile;
        }

        Debug.LogError($"Save file not found: {file}");
        return null;
    }

    public List<SaveProfile> LoadAllSaveData()
    {
        List<SaveProfile> list = new List<SaveProfile>();

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        string[] files = Directory.GetFiles(filePath, "*.json");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            SaveProfile profile = JsonUtility.FromJson<SaveProfile>(json);
            list.Add(profile);
        }

        return list;
    }

    public void DeleteProfile(string profileName)
    {
        string file = Path.Combine(filePath, profileName + ".json");

        if (File.Exists(file))
        {
            File.Delete(file);
            Debug.Log($"Deleted profile: {profileName}");
        }
        else
        {
            Debug.LogWarning($"Profile not found: {profileName}");
        }
    }
}

[Serializable]
public class SaveProfile
{
    public string profileName;
    public float bestTime;
    public GhostData ghostData;
    public string vehicleName;

    public SaveProfile(string profileName, string vehicleName)
    {
        this.profileName = profileName;
        this.vehicleName = vehicleName;
        bestTime = 0f;
        ghostData = null;
    }
}
