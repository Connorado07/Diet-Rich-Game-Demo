using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script handles slider.

public class SliderText : MonoBehaviour
{

    public Text sliderValue;
    public Slider slider;

    public void Awake()
    {

    }
    public void Update()
    {

        sliderValue.text = slider.value.ToString("0.0");

        if (slider.value >= (slider.maxValue - 0.05f) || slider.value <= slider.minValue + 0.05f)
        {
            sliderValue.text = slider.value.ToString("0");
        }

    }

}
