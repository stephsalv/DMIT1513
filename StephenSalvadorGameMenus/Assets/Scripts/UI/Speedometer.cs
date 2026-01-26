using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public List<Rigidbody> targets;
    public int currentTargetIndex;

    public float maxSpeed = 0.0f;

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public Text speedLabel;
    public RectTransform arrow;

    private float speed = 0.0f;
    private void Update()
    {
        if (targets == null || targets.Count == 0)
            return;

        // Clamp index just in case
        currentTargetIndex = Mathf.Clamp(currentTargetIndex, 0, targets.Count - 1);

        Rigidbody target = targets[currentTargetIndex];
        if (target == null) return;

        speed = target.linearVelocity.magnitude * 3.6f;

        if (speedLabel != null)
            speedLabel.text = $"{(int)speed} km/h";

        if (arrow != null)
        {
            float t = Mathf.Clamp01(speed / maxSpeed);
            arrow.localEulerAngles = new Vector3(
                0, 0,
                Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, t)
            );
        }
    }

    public void SetTarget(int index)
    {
        currentTargetIndex = index;
    }
}
