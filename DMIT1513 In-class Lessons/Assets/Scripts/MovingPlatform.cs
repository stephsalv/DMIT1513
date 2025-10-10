using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float movementTime, movementSpeed, timeStamp;
    Rigidbody rbody;
    void Start()
    {
        movementSpeed = 3.0f;
        movementTime = 5.0f;

        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeStamp + movementTime)
        {
            movementSpeed = -movementSpeed;
            timeStamp = Time.time;
        }
        rbody.linearVelocity = Vector3.up * 100 * movementSpeed * Time.fixedDeltaTime;
        //transform.Translate(Vector3.right * movementSpeed * Time.fixedDeltaTime);
    }
}
