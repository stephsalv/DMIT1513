using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            myAnimator.SetBool("Open", !myAnimator.GetBool("Open"));
        }
    }
    public void Resume()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ExitButton()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
