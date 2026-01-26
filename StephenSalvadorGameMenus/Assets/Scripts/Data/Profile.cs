using UnityEngine;

public class Profile : MonoBehaviour
{
    public string profileName;
    public int vehicleID;
    public float bestTime;

    public void SaveProfile()
    {
        bool success = NewSaveSystem.SaveProfile(this);

        if (success)
        {
            FeedbackUI.Instance.ShowMessage("Profile saved successfully!", Color.green);
        }
        else
        {
            FeedbackUI.Instance.ShowMessage("Failed to save profile", Color.red);
        }
    }

    public void LoadProfile()
    {
        ProfileData data = NewSaveSystem.LoadProfile();
        if (data != null)
        {
            profileName = data.profileName;
            vehicleID = data.vehicleID;
            bestTime = data.bestTime;

            FeedbackUI.Instance.ShowMessage("Profile loaded successfully!", Color.green);
        }
        else
        {
            FeedbackUI.Instance.ShowMessage("Failed to load profile", Color.red);
        }
    }
    public string GetProfileInfo()
    {
        return $"Name: {profileName}\n" +
               $"Vehicle ID: {vehicleID}\n" +
               $"Best Time: {bestTime:F2} sec";
    }
}
