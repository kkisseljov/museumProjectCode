using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsData : MonoBehaviour {

    public float audioVolume;
    public Button saveButton;
    string path;
    string jsonString;

    public void SaveSettings()
    {
          
    }


    public void LoadSettings()
    {
   
    }

    void Start ()
    {
        path = Application.streamingAssetsPath + "/Settings.json";
        jsonString = File.ReadAllText(path);
        Setting MusicVolume = JsonUtility.FromJson<Setting>(jsonString);
        Debug.Log(MusicVolume.Volume);

        saveButton.onClick.AddListener(SaveSettings);
	}

    void Update()
    {
        audioVolume = SettingsController.audioVolume;
    }


    [System.Serializable]
    public class Setting
    {

        public float Volume;
        public bool Fullscreen;
        public float MouseSensitivity;
    }

}

