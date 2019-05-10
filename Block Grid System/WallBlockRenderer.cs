using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlockRenderer : MonoBehaviour {

    public GameObject front;
    public GameObject rear;
    public GameObject left;
    public GameObject right;

    private BlockRenderer _blockRenderer;

    private void Start()
    {
        _blockRenderer = transform.GetComponentInParent<BlockRenderer>();

        if(_blockRenderer == null)
        {
            Debug.LogError("WallBlockRenderer is not able to get Block instance!");
        } else
        {
            RenderBlockParts();
        }
    }

    public void RenderBlockParts()
    {
        Block block = _blockRenderer.block;
        BlockGrid grid = BlockGridController.Singleton.grid;

        Block frontBlock = grid.GetBlock(block.coordinates.GetFront(grid));
        Block rearBlock = grid.GetBlock(block.coordinates.GetRear(grid));
        Block leftBlock = grid.GetBlock(block.coordinates.GetRight(grid));
        Block rightBlock = grid.GetBlock(block.coordinates.GetLeft(grid));

        /*
        Debug.Log("RenderBlockParts() -> " + block.coordinates + " (" + block.type + ")"
            + "\n\t front: \t" + frontBlock.coordinates + " (" + frontBlock.type + ")"
            + "\n\t rear: \t" + rearBlock.coordinates + " (" + rearBlock.type + ")"
            + "\n\t left: \t" + leftBlock.coordinates + " (" + leftBlock.type + ")"
            + "\n\t right: \t" + rightBlock.coordinates + " (" + rightBlock.type + ")"
            );
            */

        front.SetActive(frontBlock != null && frontBlock.type == BlockType.Ground);
        rear.SetActive(rearBlock != null && rearBlock.type == BlockType.Ground);
        left.SetActive(leftBlock != null && leftBlock.type == BlockType.Ground);
        right.SetActive(rightBlock != null && rightBlock.type == BlockType.Ground);
    }
}
