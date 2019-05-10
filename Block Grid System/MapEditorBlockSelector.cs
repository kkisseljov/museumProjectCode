using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorBlockSelector : MonoBehaviour {
    public static MapEditorBlockSelector Singleton { get; private set; }

    public enum Mode { MapEditor, GameLevel };

    public Mode mode = Mode.MapEditor;

    private MapEditorController _mapEditorController;

    private void Start()
    {
        Singleton = this;
        _mapEditorController = MapEditorController.Singleton;
    }

    void Update() {
        if (Input.GetMouseButtonDown(1) && mode == Mode.GameLevel)
        {
            ProcessClick();
        } else if (Input.GetMouseButtonDown(0) && mode == Mode.MapEditor)
        {
            ProcessClick();
        }
    }

    void ProcessClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)
            //Layers 'Walkable' and 'Wall'
            && (hitInfo.transform.gameObject.layer == 8 || hitInfo.transform.gameObject.layer == 9))    //TODO rename layers
        {
            //Debug.Log("layer: " + hitInfo.transform.gameObject.layer);
            BlockRenderer renderer = hitInfo.transform.parent.gameObject.GetComponent<BlockRenderer>();

            if (renderer != null)
            {
                if(mode == Mode.MapEditor)
                {
                    renderer.OnClick(_mapEditorController.selectedBlockType);
                } else
                {
                    renderer.OnClick();
                }
            }
        }
    }
}
