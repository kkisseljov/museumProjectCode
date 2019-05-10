using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockRenderer : MonoBehaviour
{
    public Block block;
    //public List<BlockRenderer> nearbyBlocks = new List<BlockRenderer>();

    private BlockGridPrefabs _prefabs;
    private BlockGridController _blockGridController;
    private GameObject _blockModel;

    public List<BlockRenderer> nearbyBlocks
    {
        get
        {
            return BlockGridRenderer.Singleton.GetNearestBlockRenderers(this);
        }
    }

    public bool isRendered
    {
        get
        {
            return _blockModel != null;
        }
    }

    public bool hasGroundNearby
    {
        get
        {
            return nearbyBlocks.Any(x => x.block.type == BlockType.Ground);
        }
    }

    private void Awake()
    {
        _blockGridController = BlockGridController.Singleton;

        if (_blockGridController == null)
        {
            Debug.LogError("BlockGridController is missing on scene!");
            return;
        }

        _prefabs = BlockGridController.Singleton.prefabs;
    }

    public void Render(bool replaceModel = false)
    {
        //Debug.Log("Render " + block.coordinates);

        if (replaceModel)
        {
            if (isRendered)
            {
                Destroy(_blockModel);
            }
        }
        else if (isRendered)
        {
            //Nothing changes -> skip
            return;
        }

        //Debug.Log("Render block: " + block.type + " " + block.coordinates + "\t isRendered: " + isRendered + "\t replaceModel: " + replaceModel);

        GameObject prefab = _prefabs.GetBlockPrefab(block.type);
        if (prefab != null)
        {
            _blockModel = Instantiate(prefab, transform);
        }
    }

    public void OnClick()
    {
        Debug.Log("Clicked block: " + block.type.ToString() + " - " + block.coordinates.ToString());

        if (CharactersController.Singleton != null
            && CharactersController.Singleton.selectedCharacter != null)
        {
            CharactersController.Singleton.selectedCharacter.OnBlockClicked(this);
        }
    }

    

    /// <summary>
    /// Currently used on MapEditor only
    /// </summary>
    /// <param name="selectedBlockType">Selected block type in Map Editor toolbar</param>
    public void OnClick(BlockType selectedBlockType)
    {
        Debug.Log("Clicked block: " + block.type.ToString() + " - " + block.coordinates.ToString() + "\n\tGoing to be changed to " + selectedBlockType.ToString());

        if (_blockGridController.grid.startingPosition == block.coordinates)
        {
            Debug.Log("Block is on starting position -> do not change");
            return;
        }

        if (block.coordinates.OnGridEdge(_blockGridController.grid))
        {
            Debug.Log("Block is on grid edge -> do not change");
            return;
        }

        if (block.type == selectedBlockType)
        {
            Debug.Log("Same block type -> nothing to change");
            return;
        }

        ChangeBlockTypeAndUpdate(selectedBlockType);
    }

    public void ChangeBlockTypeAndUpdate(BlockType newBlockType)
    {
        block.type = newBlockType;
        name = block.ToString();

        Render(true);

        nearbyBlocks.ForEach(x => x.OnNearbyBlockChanged(this));
        nearbyBlocks
            .FindAll(x => x.block.type == BlockType.RockShale)
            .ForEach(x => BlockGridRenderer.Singleton.InstantiateFarWallBlocks(x, true));
    }

    public void OnNearbyBlockChanged(BlockRenderer changedBlock)
    {
        Debug.Log("OnNearbyBlockChanged " + block.coordinates);
        if (changedBlock.block.type == BlockType.Ground)
        {
            if (!isRendered)
            {
                Render();
            }
            else if (block.type == BlockType.RockShale)
            {
                Debug.Log("BlockRenderer -> OnNearbyBlockChanged() -> Send 'RenderBlockParts' to " + block.coordinates + " (" + block.type + ")");
                _blockModel.gameObject.SendMessage("RenderBlockParts");
            }
        }

        if (changedBlock.block.type == BlockType.RockShale)
        {
            if (block.type == BlockType.RockShale)
            {
                if (hasGroundNearby)
                {
                    Render();
                }
                else if (isRendered)
                {
                    Destroy(_blockModel);
                }
            }
        }
    }
}
