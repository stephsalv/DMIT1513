using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private ProfileItemUI profileItemPrefab;

    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TMP_Text confirmationText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [SerializeField] private SaveSystem saveSystem;

    public string filePath;

    private void OnEnable()
    {
        confirmationPanel.SetActive(false);
        RefreshProfileList();
    }
    private void RefreshProfileList()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<string> profiles = GetAllProfiles();

        foreach (string profileName in profiles)
        {
            ProfileItemUI item = Instantiate(profileItemPrefab, contentParent);
            item.Init(profileName, this);
        }
    }
    private List<string> GetAllProfiles()
    {
        List<string> names = new List<string>();

        if (!Directory.Exists(filePath))
            return names;

        string[] files = Directory.GetFiles(filePath, "*.json");

        foreach (string file in files)
            names.Add(Path.GetFileNameWithoutExtension(file));

        return names;
    }

    public void LoadProfile(string profileName)
    {
        bool success = saveSystem.LoadData(profileName);

        if (success)
        {
            Debug.Log($"Profile selected: {profileName}");
            gameObject.SetActive(false);
        }
    }

    public void RequestDelete(string profileName)
    {
        pendingDeleteProfile = profileName;

        confirmationText.text =
            $"Delete profile \"{profileName}\"?\nThis action cannot be undone.";

        confirmationPanel.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(ConfirmDelete);
        noButton.onClick.AddListener(() => confirmationPanel.SetActive(false));
    }

    private void ConfirmDelete()
    {
        string file = Path.Combine(filePath, pendingDeleteProfile + ".json");

        if (File.Exists(file))
        {
            File.Delete(file);
            Debug.Log($"Profile deleted: {pendingDeleteProfile}");
        }

        confirmationPanel.SetActive(false);
        RefreshProfileList();
    }

    [Serializable]
    public class ProfileItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;

        private string profileName;
        private ProfileSelectionUI parent;

        public void Init(string name, ProfileSelectionUI parentUI)
        {
            profileName = name;
            parent = parentUI;

            nameText.text = name;

            loadButton.onClick.RemoveAllListeners();
            deleteButton.onClick.RemoveAllListeners();

            loadButton.onClick.AddListener(() =>
                parent.LoadProfile(profileName));

            deleteButton.onClick.AddListener(() =>
                parent.RequestDelete(profileName));
        }
    }
}
