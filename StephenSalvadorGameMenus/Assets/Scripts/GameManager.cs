using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public SaveSystem SavingSystem;
    public SaveProfile currentProfile;

    public List<SaveProfile> profiles;

    public string dataPath => SavingSystem != null ? SavingSystem.filePath : string.Empty;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (SavingSystem == null)
            return;

        if (string.IsNullOrWhiteSpace(SavingSystem.filePath))
            SavingSystem.filePath = System.IO.Path.Combine(Application.persistentDataPath, "SaveData") + "/";

        if (!System.IO.Directory.Exists(SavingSystem.filePath))
            System.IO.Directory.CreateDirectory(SavingSystem.filePath);

        profiles = SavingSystem.LoadAllSaveData();
    }

    public void AddProfile(SaveProfile profile)
    {
        if (profiles == null)
            profiles = new List<SaveProfile>();

        profiles.Add(profile);
        SavingSystem.CreateSaveData(profile);
    }

    public static void MoveScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
