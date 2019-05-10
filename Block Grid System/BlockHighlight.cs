using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHighlight : MonoBehaviour {

	public static BlockHighlight Singleton { get; private set; }

    public GameObject highlightGo;

    private void Awake()
    {
        Singleton = this;
    }

    public static void SetPosition(Vector3 position)
    {
        if (Singleton != null)
        {
            Singleton.transform.position = new Vector3(position.x, 0f, position.z);
        }
    }

    public static void Show()
    {
        if(Singleton != null)
        {
            Singleton.highlightGo.SetActive(true);
        }
    }

    public static void Hide()
    {
        if (Singleton != null)
        {
            Singleton.highlightGo.SetActive(false);
        }
    }
}
