using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLoadingScreen : MonoBehaviour {

    /*

    public Image LoadingScreenImage1;       //ПЕРЕМЕННЫЕ С МАЛЕНЬКОЙ БУКВЫ
    public Image LoadingScreenImage2;       //ПЕРЕМЕННЫЕ С МАЛЕНЬКОЙ БУКВЫ

    void Start () {
        pickLoadingScreen();
		
	}

    public void pickLoadingScreen()         //МЕТОДЫ С БОЛЬШОЙ БУКВЫ
    {
        int indexNumber = Random.Range(1, 3);
        if(indexNumber == 1)
        {
            LoadingScreenImage1.enabled = true;
            LoadingScreenImage2.enabled = false;
        }
        else if (indexNumber == 2)
        {
            LoadingScreenImage2.enabled = true;
            LoadingScreenImage1.enabled = false;
        }
        
        
    }
		
	*/

    public Image backgroundImage;
    public List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        int randomIndex = Random.Range(0, sprites.Count - 1);
        backgroundImage.sprite = sprites[randomIndex];
    }
}
