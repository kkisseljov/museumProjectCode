using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMapMenu : MonoBehaviour
{
    public InputField gridWidthInput;
    public InputField gridLengthInput;
    public InputField startingPositionXInput;
    public InputField startingPositionYInput;

    public void Start()
    {
        if(gridWidthInput == null 
            || gridLengthInput == null 
            || startingPositionXInput == null 
            || startingPositionYInput == null)
        {
            Debug.LogError("Not all input fields are connected to NewMapMenu script!");
            enabled = false;
        }
    }

    public void OnGenerateButtonClick()
    {
        int gridWidth = Int32.Parse(gridWidthInput.text);
        int gridLength = Int32.Parse(gridLengthInput.text);
        GridCoordinates startingPosition = new GridCoordinates(Int32.Parse(startingPositionXInput.text), Int32.Parse(startingPositionYInput.text));

        NewMapLoaderData data = new NewMapLoaderData(gridWidth, gridLength, startingPosition);

        SceneSwitcher.Singleton.PrepareToLoadLevel(data);
    }
}
