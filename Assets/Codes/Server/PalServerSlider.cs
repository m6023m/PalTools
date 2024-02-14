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
    }

    void Start()
    {
        slider.onValueChanged.AddListener(SliderValueChanged);
    }
    void SliderValueChanged(float changedValue)
    {
        palServerValue.Value = changedValue.ToString();
    }

    public void Init()
    {
        slider.minValue = palServerValue.minValue;
        slider.maxValue = palServerValue.maxValue;
        if (palServerValue.type == PalServerValue.ValueType.Float)
        {
            slider.wholeNumbers = false;
        }
        else
        {
            slider.wholeNumbers = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (palServerValue.type == PalServerValue.ValueType.Float)
        {
            slider.value = palServerValue.ValueToFloat();
        }
        else
        {
            slider.value = palServerValue.ValueToInt();
        }
    }
}
