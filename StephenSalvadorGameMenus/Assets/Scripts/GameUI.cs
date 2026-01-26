using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject createProfilePanel;
    public GameObject loadProfilePanel;

    public TMP_InputField profileNameInput;
    public TextMeshProUGUI vehicleNameText;
    public TextMeshProUGUI createFeedbackText;
    public TextMeshProUGUI deleteFeedbackText;

    public TMP_Dropdown profileDropdown;
    public CarSelection carSelection;
    public SaveSystem saveSystem;

    public TextMeshProUGUI playerInfoText;

    private List<SaveProfile> savedProfiles = new();
    private bool deleteConfirm;

    private void Start()
    {
        ShowMainMenu();
        UpdatePlayerInfoUI();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        createProfilePanel.SetActive(false);
        loadProfilePanel.SetActive(false);
    }

    public void ShowCreateProfilePanel()
    {
        mainMenuPanel.SetActive(false);
        createProfilePanel.SetActive(true);
        loadProfilePanel.SetActive(false);

        UpdateVehicleText();
        createFeedbackText.text = "";
        profileNameInput.text = "";
    }

    public void ShowLoadProfilePanel()
    {
        mainMenuPanel.SetActive(false);
        createProfilePanel.SetActive(false);
        loadProfilePanel.SetActive(true);

        PopulateLoadProfiles();
    }

    public void CreateProfile()
    {
        string profileName = profileNameInput.text.Trim();

        if (string.IsNullOrEmpty(profileName))
        {
            createFeedbackText.text = "Profile name cannot be blank!";
            return;
        }

        List<SaveProfile> allProfiles = saveSystem.LoadAllSaveData();
        foreach (var profile in allProfiles)
        {
            if (profile.profileName == profileName)
            {
                createFeedbackText.text = "Profile name already exists!";
                return;
            }
        }

        SaveProfile newProfile = new SaveProfile(profileName, carSelection.cars[carSelection.currentCar].name);
        saveSystem.CreateSaveData(newProfile);

        GameManager.instance.currentProfile = newProfile;
        UpdatePlayerInfoUI();

        createFeedbackText.text = "Profile created successfully!";
        CloseMenus();
    }

    private void UpdateVehicleText()
    {
        if (vehicleNameText != null && carSelection != null)
            vehicleNameText.text = carSelection.cars[carSelection.currentCar].name;
    }

    private void PopulateLoadProfiles()
    {
        profileDropdown.ClearOptions();
        savedProfiles = saveSystem.LoadAllSaveData();

        List<string> options = new();
        foreach (var profile in savedProfiles)
            options.Add(profile.profileName);

        profileDropdown.AddOptions(options);
        deleteFeedbackText.text = "";
    }

    public void OnDropdownChanged()
    {
        deleteConfirm = false;
        deleteFeedbackText.text = "";
        OnProfileSelected();
    }

    private void OnProfileSelected()
    {
        int index = profileDropdown.value;
        if (index < 0 || index >= savedProfiles.Count) return;

        GameManager.instance.currentProfile = savedProfiles[index];
        UpdatePlayerInfoUI();
    }

    public void LoadSelectedProfile()
    {
        int index = profileDropdown.value;
        if (index < 0 || index >= savedProfiles.Count) return;

        GameManager.instance.currentProfile = savedProfiles[index];

        SaveProfile profile = GameManager.instance.currentProfile;
        for (int i = 0; i < carSelection.cars.Length; i++)
        {
            if (carSelection.cars[i].name == profile.vehicleName)
            {
                carSelection.ActivateCar(i);
                break;
            }
        }

        UpdatePlayerInfoUI();
        deleteFeedbackText.text = $"Loaded {profile.profileName}";
    }

    public void DeleteSelectedProfile()
    {
        int index = profileDropdown.value;
        if (index < 0 || index >= savedProfiles.Count) return;

        SaveProfile profile = savedProfiles[index];

        if (!deleteConfirm)
        {
            deleteFeedbackText.text = $"Delete {profile.profileName}? Press delete again to confirm.";
            deleteConfirm = true;
            return;
        }

        string file = Path.Combine(saveSystem.filePath, profile.profileName + ".json");
        if (File.Exists(file))
            File.Delete(file);

        deleteConfirm = false;
        deleteFeedbackText.text = $"{profile.profileName} deleted";

        PopulateLoadProfiles();
    }

    private void CloseMenus()
    {
        mainMenuPanel.SetActive(false);
        createProfilePanel.SetActive(false);
        loadProfilePanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }

    private void UpdatePlayerInfoUI()
    {
        SaveProfile profile = GameManager.instance.currentProfile;

        if (playerInfoText != null && profile != null)
        {
            playerInfoText.text =
                $"Profile: {profile.profileName}\n" +
                $"Vehicle: {profile.vehicleName}";
        }
        else if (playerInfoText != null)
        {
            playerInfoText.text = "No profile selected";
        }
    }
}
