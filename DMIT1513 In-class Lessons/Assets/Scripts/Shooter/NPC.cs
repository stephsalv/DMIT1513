using UnityEngine;

public class NPC : MonoBehaviour
{
    bool player_detection = false;

    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.F))
        {
            print("Interaction started!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BodyModel")
        {
            player_detection = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "BodyModel")
        {
            player_detection = false;
        }
    }
}
