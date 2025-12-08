using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public GameObject spotLight;
    public float range = 10f;
    public LayerMask ghostLayer;

    [Header("Battery Settings")]
    public float maxOnTime = 5f;
    public float cooldownTime = 30f;

    private bool isON = false;
    private bool isCoolingDown = false;

    private float onTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        spotLight.SetActive(false);
    }

    void Update()
    {
        HandleToggle();
        HandleTimers();

        if (isON)
            ShineLight();
    }

    private void HandleToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Only allow turning on if not cooling down
            if (!isON && !isCoolingDown)
            {
                TurnOn();
            }
            else if (isON)
            {
                TurnOff();
            }
        }
    }

    private void HandleTimers()
    {
        if (isON)
        {
            onTimer += Time.deltaTime;
            if (onTimer >= maxOnTime)
            {
                TurnOff();
                isCoolingDown = true;
            }
        }
        else if (isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isCoolingDown = false;
                cooldownTimer = 0f;
            }
        }
    }

    private void TurnOn()
    {
        isON = true;
        onTimer = 0f;
        spotLight.SetActive(true);
        Debug.Log("Flashlight ON");
    }

    private void TurnOff()
    {
        isON = false;
        spotLight.SetActive(false);
        onTimer = 0f;
        Debug.Log("Flashlight OFF");
    }

    private void ShineLight()
    {
        Ray ray = new Ray(spotLight.transform.position, spotLight.transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * range, Color.yellow);

        if (Physics.Raycast(ray, out RaycastHit hit, range, ghostLayer))
        {
            Debug.Log("Flashlight hit: " + hit.collider.name);

            CryingState crying = hit.collider.GetComponent<CryingState>();
            if (crying != null)
            {
                crying.enabled = true;
                Debug.Log("Ghost entered CryingState: " + hit.collider.name);
            }

            AttackState attack = hit.collider.GetComponent<AttackState>();
            if (attack != null)
            {
                attack.isHitByFlashlight = true;
                Debug.Log("Triggered CryingState via AttackState: " + hit.collider.name);
            }
        }
    }

    // Other scripts (like LightDetector) can read this
    public bool IsFlashlightOn()
    {
        return isON;
    }
}
