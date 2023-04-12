using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AlertBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetAlertness(float alertness)
    {
        slider.value = alertness;
    }

    public void SetMaxAlertness(float alertness)
    {
        slider.maxValue = alertness;
        slider.value = 0;
    }

}
