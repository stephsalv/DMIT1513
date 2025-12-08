using UnityEngine;

public class SpinFan : MonoBehaviour
{
    public float spinSpeed = 200f; // adjust in inspector

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}