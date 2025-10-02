using UnityEngine;
using UnityEngine.UI;


public class CarSettingsUI : MonoBehaviour
{
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    public Button[] speedButtons;
    public Button[] accelButtons;
    public Button[] turnButtons;

    public void SetSpeedFast()
    {
        CarSettingsManager.Instance.SetMaxSpeed(60f);
        HighlightSelected(speedButtons[0], speedButtons);
    }

    public void SetSpeedSlow()
    {
        CarSettingsManager.Instance.SetMaxSpeed(40f);
        HighlightSelected(speedButtons[1], speedButtons);
    }

    public void SetAccelHigh()
    {
        CarSettingsManager.Instance.SetForwardAccel(40f);
        HighlightSelected(accelButtons[0], accelButtons);
    }

    public void SetAccelLow()
    {
        CarSettingsManager.Instance.SetForwardAccel(20f);
        HighlightSelected(accelButtons[1], accelButtons);
    }

    public void SetTurnSharp()
    {
        CarSettingsManager.Instance.SetTurnStrength(80f);
        HighlightSelected(turnButtons[0], turnButtons);
    }

    public void SetTurnWide()
    {
        CarSettingsManager.Instance.SetTurnStrength(50f);
        HighlightSelected(turnButtons[1], turnButtons);
    }

    private void HighlightSelected(Button selected, Button[] group)
    {
        foreach (Button btn in group)
        {
            btn.image.color = defaultColor;
        }
        selected.image.color = selectedColor;
    }

}

