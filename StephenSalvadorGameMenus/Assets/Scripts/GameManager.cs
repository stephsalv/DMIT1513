using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string dataPath { get; private set; }
    public static GameManager instance { get; private set; }
    public SaveProfile currentProfile;
    public SaveSystem SavingSystem;

    [SerializeField]
    public List<SaveProfile> profiles;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        dataPath = System.IO.Path.Combine(Application.persistentDataPath, "SaveData");
        System.IO.Directory.CreateDirectory(dataPath);
        Debug.Log($"Data Path set as {dataPath}");

        profiles = SavingSystem.LoadAllSaveData();
    }
    public void AddProfile(SaveProfile profile)
    {
        profiles.Add(profile);
        SavingSystem.CreateSaveData(profile);
    }
    public static void MoveScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
