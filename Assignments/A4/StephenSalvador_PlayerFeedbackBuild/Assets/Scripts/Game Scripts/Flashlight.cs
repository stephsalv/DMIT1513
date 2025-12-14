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
        if (spotLight != null)
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
        if (spotLight != null)
            spotLight.SetActive(true);

        Debug.Log("Flashlight ON");
    }

    private void TurnOff()
    {
        isON = false;
        onTimer = 0f;
        if (spotLight != null)
            spotLight.SetActive(false);

        Debug.Log("Flashlight OFF");
    }

    private void ShineLight()
    {
        if (spotLight == null) return;

        Ray ray = new Ray(spotLight.transform.position, spotLight.transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * range, Color.yellow);

        // Detect all ghosts in the flashlight ray
        RaycastHit[] hits = Physics.RaycastAll(ray, range, ghostLayer);
        foreach (RaycastHit hit in hits)
        {
            LightDetector detector = hit.collider.GetComponent<LightDetector>();
            if (detector != null)
            {
                detector.isLightOn = true;
            }
        }
    }

    // Other scripts (like states) can read this
    public bool IsFlashlightOn()
    {
        return isON;
    }
}
