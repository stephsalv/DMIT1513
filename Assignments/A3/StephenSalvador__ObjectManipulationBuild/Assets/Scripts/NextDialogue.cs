using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    int index = 2;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && transform.childCount > 1)
        {
            if (!PlayerController.dialogue)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                index += 1;
                if (transform.childCount == index)
                {
                    index = 2;
                    PlayerController.dialogue = false;
                }
                else
                {
                    gameObject.SetActive(false);
                }

            }
        }
    }
}
