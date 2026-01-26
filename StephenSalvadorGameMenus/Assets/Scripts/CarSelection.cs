using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public int currentCar;
    public bool inGameplayScene = false;

    public Speedometer speedometer;

    void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedCarID", 0);

        if (inGameplayScene)
        {
            ActivateCar(selectedCar);
        }
    }

    public void Next()
    {
        if (currentCar < cars.Length - 1)
        {
            ActivateCar(currentCar + 1);
        }
    }

    public void Previous()
    {
        if (currentCar > 0)
        {
            ActivateCar(currentCar - 1);
        }
    }

    public void Select()
    {
        PlayerPrefs.SetInt("SelectedCarID", currentCar);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }

    private void ActivateCar(int index)
    {
        currentCar = index;

        for (int i = 0; i < cars.Length; i++)
            cars[i].SetActive(i == currentCar);

        if (speedometer != null)
            speedometer.SetTarget(currentCar);
    }
}
