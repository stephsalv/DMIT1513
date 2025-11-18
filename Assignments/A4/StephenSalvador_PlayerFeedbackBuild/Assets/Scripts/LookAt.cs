using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign your player in Inspector
    [SerializeField] private bool onlyRotateY = true; // Optional: only rotate horizontally

    void Update()
    {
        if (player == null) return;

        if (onlyRotateY)
        {
            // Rotate only around Y axis (so it doesn’t tilt up/down)
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Ignore vertical difference
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            // Fully face the player
            transform.LookAt(player);
        }
    }
}
//    private Transform player;

//void Start()
//{
//    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//    if (playerObj != null)
//        player = playerObj.transform;
//}

//void Update()
//{
//    if (player == null) return;

//    Vector3 direction = player.position - transform.position;
//    direction.y = 0; // only rotate horizontally
//    if (direction != Vector3.zero)
//        transform.rotation = Quaternion.LookRotation(direction);
//}