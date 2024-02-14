using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PalServerObject : MonoBehaviour
{
    public PalServerValue palServerValue;
    TextMeshProUGUI textName;
    TMP_InputField inputValue;
    protected virtual void Awake()
    {
        textName = GetComponentInChildren<TextMeshProUGUI>(true);
        inputValue = GetComponentInChildren<TMP_InputField>(true);
    }

    void Start()
    {
        inputValue.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string changedText)
    {
        palServerValue.Value = changedText;
    }

    protected virtual void Update()
    {
        textName.text = palServerValue.displayName;
        inputValue.text = palServerValue.Value;
    }
}
