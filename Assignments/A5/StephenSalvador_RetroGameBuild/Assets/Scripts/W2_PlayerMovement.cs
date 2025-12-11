using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class W2_PlayerMovement : MonoBehaviour
{
    float movementSpeed = 3.0f;

    [SerializeField] InputAction moveAction;

    [SerializeField] Vector2 moveValue;

    float xValue, zValue;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // move the object based on the values of the gamepad
        transform.Translate(new Vector3(moveValue.x * movementSpeed * Time.fixedDeltaTime, 0, moveValue.y * movementSpeed * Time.fixedDeltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }
}