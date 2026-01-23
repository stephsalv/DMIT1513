using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public List<SaveProfile> profiles = new List<SaveProfile>();
    public string filePath;

    public void Start()
    {
        //CreateSave(new SaveProfile("Stephen", 1));

    }

    public void CreateSave(SaveProfile profile)
    {
        bool fileExists = File.Exists(filePath);

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                sw.WriteLine("Profile Name, Score");
            }

            sw.WriteLine($"{profile.profileName}, {profile.bestTime}");
            profiles.Add(profile);
        }
    }
    public void DeleteSave()
    {

    }
    public void LoadSave(string profileToLoad)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            string[] columns = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            if (columns[0] == profileToLoad)
            {
                int score = int.Parse(columns[1]);
                Debug.Log($"Profile {profileToLoad} loaded with score {score}.");
                return;
            }
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
        profileName = profileName_;
        bestTime = bestTime_;
        vehicle = vehicle_;
        color = color_;
        GhostData = ghostData_;
    }
}


