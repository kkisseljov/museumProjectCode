using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntegerInputFieldValidator : MonoBehaviour {

    [Serializable]
    public class Range
    {
        public bool enabled = true;
        public int minValue = 1;
        public int maxValue = 100;
    }

    public Range range = new Range();
    private InputField _inputField;

    private void Start()
    {
        _inputField = GetComponent<InputField>();
    }

    public void Validate()
    {
        int currentValue;

        if (Int32.TryParse(_inputField.text, out currentValue)) {
            if (range.enabled)
            {
                if (currentValue < range.minValue)
                {
                    currentValue = range.minValue;
                }

                if (currentValue > range.maxValue)
                {
                    currentValue = range.maxValue;
                }
            }

        } else
        {
            if (range.enabled)
            {
                currentValue = range.minValue;
            }
        }
    }
}
