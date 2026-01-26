using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject createProfilePanel;
    [SerializeField] private float messageDisplayTime = 2f;
    [SerializeField] private TMP_Text profileInfoText;

    public void OnCreateClicked()
    {

    }
}
