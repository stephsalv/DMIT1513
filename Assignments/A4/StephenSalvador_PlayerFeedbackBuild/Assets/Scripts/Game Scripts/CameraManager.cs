using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public SecurityCamera[] cameras;
    public float minDelay = 10f;
    public float maxDelay = 20f;

    private void Start()
    {
        StartCoroutine(RandomShutdownLoop());
    }

    IEnumerator RandomShutdownLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            SecurityCamera cam = cameras[Random.Range(0, cameras.Length)];

            if (cam.isOn)
                cam.TurnOff();
        }
    }
}
