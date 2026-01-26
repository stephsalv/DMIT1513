using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void CreateSaveData(SaveProfile saveProfile)
    {
        var profileName = saveProfile.profileName;
        string file = Path.Combine(GameManager.instance.dataPath, saveProfile.profileName + ".json");

        string json = JsonUtility.ToJson(saveProfile, true);

        File.WriteAllText(file, json);
        Debug.Log("Saving to file");
    }
    public void LoadSaveData(SaveProfile saveProfile)
    {
        var profileName = saveProfile.profileName;
        string file = Path.Combine(GameManager.instance.dataPath, saveProfile.profileName + ".json");

        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);

            saveProfile = JsonUtility.FromJson<SaveProfile>(json);
        }
        else
        {
            Debug.LogError("Save file not found");
        }
    }
    public List<SaveProfile> LoadAllSaveData()
    {
        List<SaveProfile> list = new();

        if (!Directory.Exists(GameManager.instance.dataPath))
            return list;

        string[] files = Directory.GetFiles(GameManager.instance.dataPath, "files.json");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            SaveProfile profile = JsonUtility.FromJson<SaveProfile>(json);
            list.Add(profile);
        }
        return list;
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
    }
}

