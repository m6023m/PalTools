using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PalServerSlider : PalServerObject
{
    Slider slider;
    protected override void Awake()
    {
        base.Awake();
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { SliderValueChanged(slider); });
    }

    void SliderValueChanged(Slider slider)
    {
        if (palServerValue.type == PalServerValue.ValueType.Int)
        {
            slider.wholeNumbers = true;
        }
        else
        {
            slider.wholeNumbers = false;
        }
        palServerValue.Value = slider.value.ToString();
    }

    protected override void Update()
    {
        base.Update();
        slider.value = palServerValue.ValueToFloat();
        slider.minValue = palServerValue.minValue;
        slider.maxValue = palServerValue.maxValue;
    }
}
