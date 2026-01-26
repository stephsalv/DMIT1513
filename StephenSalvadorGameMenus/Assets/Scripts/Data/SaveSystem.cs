using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public Profile profile;
    public string filePath;

    public void SaveData()
    {
        if (profile == null)
        {
            Debug.LogError("Save failed: Profile is null.");
        }

        if (string.IsNullOrEmpty(profile.profileName))
        {
            Debug.LogError("Save failed: Profile name is empty.");
        }

        string file = Path.Combine(filePath, profile.profileName + ".json");

        ProfileData newData = new ProfileData(
            profile.profileName,
            profile.vehicleID,
            profile.bestTime
        );

        if (File.Exists(file))
        {
            string oldJson = File.ReadAllText(file);
            ProfileData existingData = JsonUtility.FromJson<ProfileData>(oldJson);

            if (existingData.bestTime > 0 &&
                (newData.bestTime <= 0 || existingData.bestTime < newData.bestTime))
            {
                newData.bestTime = existingData.bestTime;
            }
        }

        string json = JsonUtility.ToJson(newData, true);
        File.WriteAllText(file, json);

        Debug.Log($"Profile saved/overwritten: {file}");
    }

    public void LoadData(string profileName)
    {
        string file = Path.Combine(filePath, profileName + ".json");

        if (!File.Exists(file))
        {
            Debug.LogError($"Load failed: {file}");
        }

        string json = File.ReadAllText(file);
        ProfileData data = JsonUtility.FromJson<ProfileData>(json);

        profile.profileName = data.profileName;
        profile.vehicleID = data.vehicleID;
        profile.bestTime = data.bestTime;
    }
    public void CreateProfile(Profile profile)
    {
        //if (profile == null || string.IsNullOrEmpty(profile.profileName)) ;

        //string file = Path.Combine(filePath, profile.profileName + ".json");

        //if (File.Exists(file))

        //ProfileData data = new ProfileData(
        //    profile.profileName,
        //    profile.vehicleID,
        //    profile.bestTime
        //);

        //string json = JsonUtility.ToJson(data, true);
        //File.WriteAllText(file, json);
    }
}
