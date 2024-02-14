using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PalServerList : PalServerObject
{
    TMP_Dropdown dropdown;
    bool isInit = false;
    protected override void Awake()
    {
        base.Awake();
        dropdown = GetComponentInChildren<TMP_Dropdown>();
    }
    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
    }
    void DropdownValueChanged(TMP_Dropdown changed)
    {
        if (palServerValue.type == PalServerValue.ValueType.Bool)
        {
            palServerValue.Value = changed.options[changed.value].text;
        }
        if (palServerValue.type == PalServerValue.ValueType.List)
        {
            palServerValue.Value = palServerValue.listValues[changed.value].name;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isInit)
        {
            dropdown.options.Clear();
            if (palServerValue.type == PalServerValue.ValueType.Bool) InitBool();
            if (palServerValue.type == PalServerValue.ValueType.List) InitList();
        }


        int index = dropdown.options.FindIndex(option => option.text.Equals(palServerValue.Value));

        if (palServerValue.type == PalServerValue.ValueType.List)
        {
            index = palServerValue.listValues.FindIndex(listValue => listValue.name.Equals(palServerValue.Value));
        }

        if (index != -1)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
    }

    void InitBool()
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData("False"));
        dropdown.options.Add(new TMP_Dropdown.OptionData("True"));
        isInit = true;
    }

    void InitList()
    {
        palServerValue.listValues.ForEach(listValue =>
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(listValue.displayName));
        });
        isInit = true;
    }

}
