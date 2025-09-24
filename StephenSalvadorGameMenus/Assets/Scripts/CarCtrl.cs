using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;
    public Transform groundRayPoint;
    public LayerMask WhatIsGround;
    public float groundRayLength = 0.75f;
    public float maxSpeed;

    private float speedInput;
    private float turnInput;
    private bool grounded;
    private float dragOnGround;

    // Added audio variables
    public AudioClip idleEngineSound;
    public AudioClip dashEngineSound;
    private AudioSource engineSource;
    private AudioSource driftSource;
    private bool isMoving = false; // Moving check variables

    void Start()
    {
        theRB.transform.parent = null;
        dragOnGround = theRB.linearDamping;

        // Load settings from Singleton
        speedInput = 0f;

        // Add audio source
        engineSource = gameObject.AddComponent<AudioSource>();
        driftSource = gameObject.AddComponent<AudioSource>();

        // Set engine sound
        engineSource.clip = idleEngineSound;
        engineSource.loop = true;
        engineSource.playOnAwake = false;
        engineSource.volume = 0.3f; // Setting the default volume

        engineSource.Play();
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
        // Update engine sound
        UpdateEngineSound();
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
    private void UpdateEngineSound()
    {
        float speedMagnitude = theRB.linearVelocity.magnitude;
        bool isCurrentlyMoving = speedMagnitude > 0.5f;

        if (isCurrentlyMoving != isMoving)
        {
            isMoving = isCurrentlyMoving;
            if (isMoving)
            {
                engineSource.clip = dashEngineSound;
            }
            else
            {
                engineSource.clip = idleEngineSound;
            }
            engineSource.Play();
        }
        engineSource.pitch = Mathf.Lerp(0.8f, 2.0f, speedMagnitude / maxSpeed);
    }
}