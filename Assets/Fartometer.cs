using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fartometer : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;
    public void setMaxFartometer(float fartometer)
    {
        slider.maxValue = fartometer;
        slider.value = fartometer;
        fill.color = gradient.Evaluate(1f);
    }
    public void setFartometer(float fartometer)
    {
        slider.value = fartometer;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
