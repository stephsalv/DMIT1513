using UnityEngine;
using UnityEngine.InputSystem;

public class FrontLoaderArmBucket : MonoBehaviour
{
    [Header("Arm & Bucket")]
    public Transform arm;
    public Transform bucket;
    public float armSpeed = 50f;
    public float bucketSpeed = 50f;
    public float armMaxUp = 60f;
    public float armMaxDown = 25f;
    public float bucketMaxUp = 40f;
    public float bucketMaxDown = 70f;

    [Header("Pickup")]
    public Transform bucketPickupPoint;
    public float pickupDistance = 2f;

    [Header("Input Actions")]
    public InputAction armAction;    // Vector2: y = up/down
    public InputAction bucketAction; // Vector2: y = up/down
    public InputAction pickupAction; // Button

    private Rigidbody pickedObject;

    private void OnEnable()
    {
        armAction.Enable();
        bucketAction.Enable();
        pickupAction.Enable();
    }

    private void OnDisable()
    {
        armAction.Disable();
        bucketAction.Disable();
        pickupAction.Disable();
    }

    private void FixedUpdate()
    {
        HandleArmAndBucket();
        HandlePickup();
    }

    private void HandleArmAndBucket()
    {
        Vector2 armInput = armAction.ReadValue<Vector2>();
        Vector2 bucketInput = bucketAction.ReadValue<Vector2>();

        // Arm rotation (X-axis)
        if (armInput.y != 0)
        {
            float newArmX = NormalizeAngle(arm.localEulerAngles.x - armInput.y * armSpeed * Time.fixedDeltaTime);
            newArmX = Mathf.Clamp(newArmX, -armMaxDown, armMaxUp);
            arm.localEulerAngles = new Vector3(newArmX, arm.localEulerAngles.y, arm.localEulerAngles.z);
        }

        // Bucket rotation (X-axis)
        if (bucketInput.y != 0)
        {
            float newBucketX = NormalizeAngle(bucket.localEulerAngles.x - bucketInput.y * bucketSpeed * Time.fixedDeltaTime);
            newBucketX = Mathf.Clamp(newBucketX, -bucketMaxDown, bucketMaxUp);
            bucket.localEulerAngles = new Vector3(newBucketX, bucket.localEulerAngles.y, bucket.localEulerAngles.z);
        }
    }

    private void HandlePickup()
    {
        if (pickupAction.triggered)
        {
            if (pickedObject == null)
            {
                // Pick up nearby Rigidbody
                Collider[] hits = Physics.OverlapSphere(bucketPickupPoint.position, pickupDistance);
                foreach (var hit in hits)
                {
                    if (hit.attachedRigidbody != null)
                    {
                        pickedObject = hit.attachedRigidbody;
                        pickedObject.transform.SetParent(bucketPickupPoint);
                        pickedObject.isKinematic = true;
                        break;
                    }
                }
            }
            else
            {
                // Release object
                pickedObject.transform.SetParent(null);
                pickedObject.isKinematic = false;
                pickedObject = null;
            }
        }
    }

    // Converts Euler 0-360 to -180 to 180 for clamping
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
    }

    private void OnDrawGizmosSelected()
    {
        if (bucketPickupPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(bucketPickupPoint.position, pickupDistance);
        }
    }
}
