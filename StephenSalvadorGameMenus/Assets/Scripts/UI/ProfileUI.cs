using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text profileInfoText;
    [SerializeField] private Profile profile;

    [Header("Settings")]
    [SerializeField] private float messageDisplayTime = 2f;

    private void Awake()
    {
        profile = FindObjectOfType<Profile>();

        saveButton.onClick.AddListener(OnSaveClicked);
        loadButton.onClick.AddListener(OnLoadClicked);
    }


    public void OnSaveClicked()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            ShowMessage("Name cannot be blank", Color.red);
            return;
        }

        profile.profileName = playerName;

        bool success = NewSaveSystem.SaveProfile(profile);
        if (success)
            ShowMessage("Profile saved successfully!", Color.green);
        else
            ShowMessage("Failed to save profile", Color.red);

        UpdateProfileInfoDisplay();
    }

    public void OnLoadClicked()
    {
        ProfileData data = NewSaveSystem.LoadProfile();

        if (data == null)
        {
            ShowMessage("No save file found", Color.red);
            return;
        }

        profile.profileName = data.profileName;
        profile.vehicleID = data.vehicleID;
        profile.bestTime = data.bestTime;

        nameInput.text = profile.profileName;

        ShowMessage("Profile loaded successfully!", Color.green);

        UpdateProfileInfoDisplay();
    }

    private void ShowMessage(string msg, Color color)
    {
        messageText.text = msg;
        messageText.color = color;

        StopAllCoroutines();
        StartCoroutine(HideMessageAfterDelay());
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.text = "";
    }
    public void UpdateProfileInfoDisplay()
    {
        if (profileInfoText != null && profile != null)
        {
            profileInfoText.text = profile.GetProfileInfo();
        }
    }
}
