using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [SerializeField] private string fileName = "saveData.json";

    public List<SaveProfile> profiles = new List<SaveProfile>();

    public string filePath;

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
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved successfully!");
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
}

[Serializable]
public class SaveProfile
{
    public string profileName;
    public float bestTime;
    public string vehicle;
    public Color color;
    GhostData GhostData;

    public SaveProfile(string profileName_, float bestTime_,string vehicle_, Color color_,  GhostData ghostData_)
    {
        this.profileName = profileName_;
        this.bestTime = bestTime_;
        this.vehicle = vehicle_;
        this.color = color_;
        this.GhostData = ghostData_;
    }
}
[Serializable]
public class SaveData
{
    public List<SaveProfile> profiles = new List<SaveProfile>();
}


