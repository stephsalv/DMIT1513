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

    public Transform scrollContent;
    public GameObject profileButtonPrefab;

    public CarSelection carSelection;
    public SaveSystem saveSystem;

    public TextMeshProUGUI playerInfoText;

    private SaveProfile currentProfile;

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

        currentProfile = newProfile;
        UpdatePlayerInfoUI();

        createFeedbackText.text = "Profile created successfully!";
        CloseMenus();
    }

    private void UpdateVehicleText()
    {
        if (vehicleNameText != null && carSelection != null)
            vehicleNameText.text = "Vehicle: " + carSelection.cars[carSelection.currentCar].name;
    }

    private void PopulateLoadProfiles()
    {
        foreach (Transform child in scrollContent)
            Destroy(child.gameObject);

        List<SaveProfile> allProfiles = saveSystem.LoadAllSaveData();

        foreach (var profile in allProfiles)
        {
            GameObject buttonObj = Instantiate(profileButtonPrefab, scrollContent);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = profile.profileName;

            Button btn = buttonObj.GetComponent<Button>();
            btn.onClick.AddListener(() => LoadProfile(profile));

            Button deleteBtn = buttonObj.transform.Find("DeleteButton")?.GetComponent<Button>();
            if (deleteBtn != null)
                deleteBtn.onClick.AddListener(() => DeleteProfile(profile));
        }
    }

    private void LoadProfile(SaveProfile profile)
    {
        for (int i = 0; i < carSelection.cars.Length; i++)
        {
            if (carSelection.cars[i].name == profile.vehicleName)
            {
                carSelection.ActivateCar(i);
                break;
            }
        }

        PlayerPrefs.SetInt("SelectedCarID", carSelection.currentCar);
        PlayerPrefs.Save();

        currentProfile = profile;
        UpdatePlayerInfoUI();
        CloseMenus();

        Debug.Log($"Loaded profile: {profile.profileName}");
    }

    private void DeleteProfile(SaveProfile profile)
    {
        string file = Path.Combine(GameManager.instance.dataPath, profile.profileName + ".json");

        if (File.Exists(file))
            File.Delete(file);

        PopulateLoadProfiles();
        Debug.Log($"Deleted profile: {profile.profileName}");
    }

    private void CloseMenus()
    {
        mainMenuPanel.SetActive(false);
        createProfilePanel.SetActive(false);
        loadProfilePanel.SetActive(false);
    }

    private void UpdatePlayerInfoUI()
    {
        if (playerInfoText != null && currentProfile != null)
        {
            playerInfoText.text =
                $"Profile: {currentProfile.profileName}\n" +
                $"Vehicle: {currentProfile.vehicleName}\n" +
                $"Best Time: {currentProfile.bestTime:F2}";
        }
        else if (playerInfoText != null)
        {
            playerInfoText.text = "No profile selected";
        }
    }
}
