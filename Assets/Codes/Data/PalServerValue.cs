using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PalServerValue
{
    public enum ValueType
    {
        Float,
        Int,
        Bool,
        String,
        List
    }

    [Serializable]
    public class ListValue
    {
        public string name;
        public string displayName;
    }

    public static ListValue CreateListValue(string _name, string _displayName)
    {
        return new ListValue { name = _name, displayName = _displayName };
    }
    public string name;
    public ValueType type = ValueType.Float;
    public string displayName;
    public string Value = "";
    public float minValue = 0.0f;
    public float maxValue = 1.0f;
    public List<ListValue> listValues;
    public PalServerValue(ValueType type, string name, string displayName, string Value, List<ListValue> listValues = null, float minValue = 0.0f, float maxValue = 1.0f)
    {
        this.type = type;
        this.name = name;
        this.displayName = displayName;
        this.Value = Value;
        this.listValues = listValues;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public PalServerValue()
    {

    }
    public PalServerValue InitPalServerValue(PalServerValue palServerValue)
    {
        type = palServerValue.type;
        name = palServerValue.name;
        displayName = palServerValue.displayName;
        Value = palServerValue.Value;
        listValues = palServerValue.listValues;
        minValue = palServerValue.minValue;
        maxValue = palServerValue.maxValue;
        return this;
    }

    public string ValueToString()
    {
        return Value;
    }

    public float ValueToFloat()
    {
        if (float.TryParse(Value, out float floatValue)) return floatValue;
        if (int.TryParse(Value, out int intValue)) return intValue;
        return 0.0f;
    }
    public int ValueToInt()
    {
        if (int.TryParse(Value, out int intValue)) return intValue;
        return 0;
    }

    public bool ValueToBoolean()
    {
        if (bool.TryParse(Value, out bool boolValue)) return boolValue;
        return false;
    }
}