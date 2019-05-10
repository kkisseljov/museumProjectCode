using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO rework to use coroutines
public class MineAction : CharacterAction
{
    public BlockRenderer blockRenderer;
    
    private float _spentTime = 0f;
    private float _damageTimeInterval = 1.5f;
    private int _damagePerHit = 5;

    public MineAction(BlockRenderer blockRenderer, CharacterControl characterControl): base(characterControl)
    {
        this.blockRenderer = blockRenderer;
    }

    public override void Start()
    {
        List<BlockRenderer> nearbyGroundBlocks = BlockGridRenderer.Singleton.GetNearestGroundBlockRenderers(blockRenderer);

        if (nearbyGroundBlocks.Count > 0)
        {
            //TODO rework and refactor target picker script
            characterControl.targetPicker.transform.position = nearbyGroundBlocks[0].block.coordinates
                .GetPositionOffset(characterControl.targetPicker.surfaceOffset);
            characterControl.transform.SendMessage("SetTarget", characterControl.targetPicker.transform);

            inProgress = true;
        } else
        {
            Debug.LogError("MineAction -> Can't reach target block. Haven't found any ground nearby");

            isDone = true;
        }
    }

    public override void Perform()
    {
        if(characterControl.ReachedDestination)
        {
            characterControl.animator.SetBool("Mining", true);
            characterControl.tools.pickaxe.SetActive(true);
            DamageBlock();
        } else
        {
            characterControl.animator.SetBool("Mining", false);
        }
    }

    public override void Stop()
    {
        inProgress = false;
        isDone = true;

        characterControl.animator.SetBool("Mining", false);
        characterControl.tools.pickaxe.SetActive(false);
    }

    private void DamageBlock()
    {
        _spentTime += Time.deltaTime;
        Block block = blockRenderer.block;

        if (block.type == BlockType.Ground)
        {
            Debug.LogError("Trying to mine ground!");
            Stop();
            return;
        }

        if (_spentTime >= _damageTimeInterval)
        {
            _spentTime = 0f;

            int minedShaleAmount = blockRenderer.block.MineBlock(_damagePerHit);

            if(!characterControl.character.inventory.AddShale(minedShaleAmount))
            {
                Debug.Log("Inventory is full");
                Stop();
            }

            if (block.durability <= 0)
            {
                blockRenderer.ChangeBlockTypeAndUpdate(BlockType.Ground);
                Stop();
            }

            Debug.Log("MineAction -> DamageBlock -> " + blockRenderer.block.durability);
        }
    }
}
