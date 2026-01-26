using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GhostTransform
{
    public Vector3 position;
    public Quaternion rotation;

    public GhostTransform(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
}

public class GhostManager : MonoBehaviour
{
    public Transform car;
    public Transform ghostCar;

    public bool recording;
    public bool playing;

    private List<GhostTransform> recordedGhostTransform = new List<GhostTransform>();
    private GhostTransform lastRecordedGhostTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (recording == true)
        {
            if (car.position != lastRecordedGhostTransform.position || car.rotation != lastRecordedGhostTransform.rotation)
            {
                var newGhostTransform = new GhostTransform(car);
                recordedGhostTransform.Add(newGhostTransform);

                lastRecordedGhostTransform = newGhostTransform;
            }
        }

        if (playing == true)
        {
            Play();
        }

    }

    void Play()
    {
        ghostCar.gameObject.SetActive(true);
        StartCoroutine(StartGhost());
        playing = false;

    }

    IEnumerator StartGhost()
    {
        for (int i = 0; i < recordedGhostTransform.Count; i++)
        {
            ghostCar.position = recordedGhostTransform[i].position;
            ghostCar.rotation = recordedGhostTransform[i].rotation;
            yield return new WaitForFixedUpdate();
        }
    }
}
