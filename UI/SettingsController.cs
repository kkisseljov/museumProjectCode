using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    public static float audioVolume;
    public  Slider musicVolume;
	public  AudioSource backgroundMusic;

	void Start()
    {

	}
    void Update()
    {
        backgroundMusic.volume = musicVolume.value;

        audioVolume = musicVolume.value;
    }


}
