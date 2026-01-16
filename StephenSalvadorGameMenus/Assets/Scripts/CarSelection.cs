using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public int currentCar;
    public bool inGameplayScene = false;

    void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedCarID");
        if (inGameplayScene == true)
        {
            cars[selectedCar].SetActive(true);
            currentCar = selectedCar;
        }
    }

    void Update()
    {

    }

    public void Next()
    {
        if (currentCar < cars.Length - 1)
        {
            currentCar += 1;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].SetActive(false);
                cars[currentCar].SetActive(true);
            }
        }
    }

    public void Previous()
    {
        if (currentCar > 0)
        {
            currentCar -= 1;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].SetActive(false);
                cars[currentCar].SetActive(true);
            }
        }
    }

    public void Select()
    {
        PlayerPrefs.SetInt("SelectedCarID", currentCar);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2); // Load gameplay scene
    }
}
