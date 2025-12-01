using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public GameObject spotLight;
    public float range = 10f;
    public LayerMask ghostLayer;

    private bool isON = false;

    void Start()
    {
        spotLight.SetActive(false);
    }

    void Update()
    {
        // Toggle flashlight
        if (Input.GetKeyUp(KeyCode.F))
        {
            isON = !isON;
            spotLight.SetActive(isON);
            Debug.Log("Flashlight " + (isON ? "ON" : "OFF"));
        }

        if (isON)
            ShineLight();
    }

    private void ShineLight()
    {
        // Use a ray from the spotlight forward
        Ray ray = new Ray(spotLight.transform.position, spotLight.transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * range, Color.yellow);

        if (Physics.Raycast(ray, out RaycastHit hit, range, ghostLayer))
        {
            Debug.Log("Flashlight hit: " + hit.collider.name);

            // If the ghost has CryingState, directly trigger it
            CryingState crying = hit.collider.GetComponent<CryingState>();
            if (crying != null)
            {
                // Reset timer and play audio immediately
                crying.enabled = true;
                Debug.Log("Ghost entered CryingState: " + hit.collider.name);
            }

            // Optionally, if using AttackState → Crying transition
            AttackState attack = hit.collider.GetComponent<AttackState>();
            if (attack != null)
            {
                attack.isHitByFlashlight = true;
                Debug.Log("Triggered CryingState via AttackState: " + hit.collider.name);
            }
        }
    }
}
