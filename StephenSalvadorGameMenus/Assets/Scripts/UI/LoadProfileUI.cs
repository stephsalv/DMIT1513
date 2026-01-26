using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Button loadButton;
    public Button deleteButton;

    private string profileName;
    private ProfileSelectionUI manager;

    public void Init(string name, ProfileSelectionUI manager)
    {
        profileName = name;
        this.manager = manager;

        nameText.text = name;

        // IMPORTANT: Clear old listeners
        loadButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();

        loadButton.onClick.AddListener(() =>
            manager.LoadProfile(profileName));

        deleteButton.onClick.AddListener(() =>
            manager.RequestDelete(profileName));
    }
}
