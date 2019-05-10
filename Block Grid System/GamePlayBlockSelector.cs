using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayBlockSelector : MonoBehaviour {
    public static GamePlayBlockSelector Singleton { get; private set; }

    private void Start()
    {
        Singleton = this;
    }

    void Update()
    {
        BlockHighlight.Hide();

        if (CharactersController.Singleton.selectedCharacter == null 
            || ActionToolbar.Singleton.selectedAction == null 
            || ActionToolbar.Singleton.selectedAction.actionName == "None")
        {
            return;
        } else
        {
            BlockHighlight.Singleton.gameObject.SetActive(true);
        }

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)
            && (hitInfo.transform.gameObject.layer == 8 || hitInfo.transform.gameObject.layer == 9))    //TODO rename layers
        {
            BlockRenderer renderer = hitInfo.transform.parent.gameObject.GetComponent<BlockRenderer>();

            if (renderer != null && !renderer.block.coordinates.OnGridEdge(BlockGridController.Singleton.grid))
            {
                if(ActionToolbar.Singleton.selectedAction.actionName == "Mine" && renderer.block.type == BlockType.RockShale)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        renderer.OnClick();
                    }

                    BlockHighlight.SetPosition(renderer.block.coordinates.GetPositionOffset());
                    BlockHighlight.Show();
                }
            }
        }
    }
}
