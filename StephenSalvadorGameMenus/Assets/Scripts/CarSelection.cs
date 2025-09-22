using UnityEngine;
using UnityEngine.UI;


public class CarSelection : MonoBehaviour
{
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;

    private int currentCar;

    private void Awake()
    {
        SelectCar(0);
    }

    private void SelectCar(int index)
    {
        previousBtn.interactable = (index != 0);
        nextBtn.interactable = (index != transform.childCount -1);

        for (int i = 0; 1 < transform.childCount; i++)
        {
            transform.GetChild(1).gameObject.SetActive(i == index);
        }
    }

    public void ChangeCar(int change)
    {
        currentCar += change;
        SelectCar(currentCar);
    }

}
