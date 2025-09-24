using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;
    public Transform groundRayPoint;
    public LayerMask WhatIsGround;
    public float groundRayLength = 0.75f;

    private float speedInput;
    private float turnInput;
    private bool grounded;
    private float dragOnGround;

    void Start()
    {
        theRB.transform.parent = null;
        dragOnGround = theRB.linearDamping;

        // Load settings from Singleton
        speedInput = 0f;
    }

    void Update()
    {
        float maxSpeed = CarSettingsManager.Instance.maxSpeed;
        float forwardAccel = CarSettingsManager.Instance.forwardAccel;
        float reverseAccel = forwardAccel * 0.5f; // Optional tweak
        float turnStrength = CarSettingsManager.Instance.turnStrength;

        speedInput = 0f;
        float vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
            speedInput = vertical * forwardAccel;
        else if (vertical < 0)
            speedInput = vertical * reverseAccel;

        turnInput = Input.GetAxis("Horizontal");

        if (vertical != 0)
        {
            float turnAmount = turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.linearVelocity.magnitude / maxSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnAmount, 0f));
        }

        if (theRB.linearVelocity.magnitude > maxSpeed)
        {
            theRB.linearVelocity = theRB.linearVelocity.normalized * maxSpeed;
        }
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(groundRayPoint.position, -transform.up, groundRayLength, WhatIsGround);

        if (grounded)
        {
            theRB.linearDamping = dragOnGround;
            theRB.AddForce(transform.forward * speedInput * 1000f);
        }

        transform.position = theRB.position;
    }
}