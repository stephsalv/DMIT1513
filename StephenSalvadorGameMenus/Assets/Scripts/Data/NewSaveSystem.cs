using UnityEngine;
using System.IO;

public static class NewSaveSystem
{
    private static string SavePath =>
         Path.Combine(Application.dataPath, "Resources/profile.json");

    public static bool SaveProfile(Profile profile)
    {
        if (profile == null)
        {
            Debug.LogError("[SaveProfile] Failed: profile is null.");
            return false;
        }

        if (string.IsNullOrEmpty(profile.profileName?.Trim()))
        {
            Debug.LogWarning("[SaveProfile] Failed: profile name is empty.");
            return false;
        }

        try
        {
            ProfileData data = new ProfileData(profile);
            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(SavePath, json);
            Debug.Log($"[SaveProfile] Success! Profile saved to {SavePath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveProfile] Failed to save profile: {e.Message}");
            return false;
        }
    }

    public static ProfileData LoadProfile()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found at " + SavePath);
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<ProfileData>(json);
    }
}