using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGridRenderer : MonoBehaviour
{
    public static BlockGridRenderer Singleton { get; private set; }

    public static readonly int WALL_BLOCK_RENDER_DEPTH = 1;

    public Transform blockPool;

    public List<BlockRenderer> blockInstances = new List<BlockRenderer>();
    public BlockGridEntrance entrance;

    [NonSerialized]
    private LoadingProcces _instantiatingGroundBlocksProcess = new LoadingProcces("Instantiating ground blocks");
    [NonSerialized]
    private LoadingProcces _instantiatingNearToGroundProcess = new LoadingProcces("Instantiating near to ground blocks");
    [NonSerialized]
    private LoadingProcces _renderingBlocksProcess = new LoadingProcces("Rendering Blocks");

    public bool isRendered
    {
        get
        {
            return blockInstances.Count > 0;
        }
    }

    //TODO rename this
    public List<LoadingProcces> loadingSequenceProcesses
    {
        get
        {
            List<LoadingProcces> list = new List<LoadingProcces>
            {
                _instantiatingGroundBlocksProcess,
                _instantiatingNearToGroundProcess,
                _renderingBlocksProcess
            };

            return list;
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        blockPool = new GameObject("Blocks").transform;
        blockPool.parent = transform;
    }

    //TODO rename this
    public IEnumerator StartLoadSequence()
    {
        yield return StartCoroutine(InstantiateGroundBlocks());
        yield return StartCoroutine(InstantiateNearToGroundBlocks());
        yield return StartCoroutine(InstantiateAndRenderBlockGridEntrance());
        yield return StartCoroutine(RenderBlocks());
    }

    /// <summary>
    /// Instantiates wall block renderers that are behind given block renderer.
    /// </summary>
    /// <param name="blockRenderer"></param>
    public void InstantiateFarWallBlocks(BlockRenderer blockRenderer, bool renderImmideately = false)
    {
        List<BlockRenderer> wallBlocks = new List<BlockRenderer>();
        List<BlockRenderer> instantiatedBlockRenderers = new List<BlockRenderer>();
        wallBlocks.Add(blockRenderer);

        for (int i = 0; i < WALL_BLOCK_RENDER_DEPTH; i++)
        {
            if (i > 0)
            {
                wallBlocks = new List<BlockRenderer>();
                wallBlocks.AddRange(instantiatedBlockRenderers);
                instantiatedBlockRenderers.Clear();
            }

            foreach (BlockRenderer renderer in wallBlocks)
            {
                instantiatedBlockRenderers.AddRange(InstantiateNearbyWallBlockRenderers(renderer));

                if(renderImmideately)
                {
                    instantiatedBlockRenderers.ForEach(x => x.Render());
                }
            }

            wallBlocks = new List<BlockRenderer>();
            wallBlocks.AddRange(instantiatedBlockRenderers);
            instantiatedBlockRenderers.Clear();
        }
    }

    /// <summary>
    /// Gives all nearby block renderers. Instatiates new if some are missing.
    /// </summary>
    /// <param name="blockRenderer"></param>
    /// <returns></returns>
    public List<BlockRenderer> GetNearestBlockRenderers(BlockRenderer blockRenderer)
    {
        List<BlockRenderer> resultList = new List<BlockRenderer>();
        List<BlockRenderer> newWallBlockRenderers = new List<BlockRenderer>();

        List<Block> nearbyBlocks = BlockGridController.Singleton
            .grid.GetNearbyBlocks(blockRenderer.block.coordinates);

        nearbyBlocks.ForEach(x =>
        {
            if (x.renderer != null)
            {
                resultList.Add(x.renderer);
            }
            else
            {
                BlockRenderer newRenderer = InstantiateBlockRenderer(x);
                resultList.Add(newRenderer);
            }
        });

        return resultList;
    }

    public List<BlockRenderer> GetNearestGroundBlockRenderers(BlockRenderer blockRenderer)
    {
        return GetNearestBlockRenderers(blockRenderer)
            .FindAll(x => x.block.type == BlockType.Ground
                && x.block.coordinates.IsAdjacentTo(blockRenderer.block.coordinates)
            );
    }

    public List<BlockRenderer> GetGroundBlockRenderers()
    {
        return new List<BlockRenderer>(blockInstances.FindAll(x => x.block.type == BlockType.Ground));
    }

    public List<Block> GetBlockList(List<BlockRenderer> renderers)
    {
        List<Block> results = new List<Block>();
        renderers.ForEach(x => results.Add(x.block));
        return results;
    }

    public void Clear()
    {
        blockInstances.ForEach(x => Destroy(x.gameObject));
        blockInstances.Clear();
    }

    private List<BlockRenderer> InstantiateNearbyWallBlockRenderers(BlockRenderer blockRenderer)
    {
        List<BlockRenderer> result = new List<BlockRenderer>();
        BlockGridController.Singleton
                    .grid
                    .GetNearbyBlocks(blockRenderer.block.coordinates)
                    .FindAll(block => block.type != BlockType.Ground && block.renderer == null)
                    .ForEach(block =>
                    {
                        BlockRenderer newRenderer = InstantiateBlockRenderer(block);
                        result.Add(newRenderer);
                        blockInstances.Add(newRenderer);
                    });

        return result;
    }

    /// <summary>
    /// Instantiates block renderer for a given block. Also sets a reference to this renderer inside block instance for easy accessing through BlockGrid.
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    private BlockRenderer InstantiateBlockRenderer(Block block)
    {
        if (block.renderer != null)
        {
            Debug.LogError("Trying to instantiate renderer for a block that already has it:\t" + block);
            return block.renderer;
        }

        BlockRenderer blockInstance = new GameObject(block.ToString())
            .AddComponent<BlockRenderer>();

        block.renderer = blockInstance;

        blockInstance.block = block;
        blockInstance.transform.parent = blockPool;
        blockInstance.transform.localPosition = block.coordinates.GetPositionOffset();

        return blockInstance;
    }

    /// <summary>
    /// Instatiates block renderers for all ground blocks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstantiateGroundBlocks()
    {
        List<Block> groundBlocks = BlockGridController.Singleton.grid.GetGroundBlocks();
        int totalIterations = groundBlocks.Count;
        int iterationCounter = 0;

        Debug.Log("InstantiateGroundBlocks -> Found " + totalIterations + " ground block in block grid");

        foreach (Block block in groundBlocks)
        {
            iterationCounter++;
            blockInstances.Add(InstantiateBlockRenderer(block));

            if (iterationCounter % 100 == 0)
            {
                _instantiatingGroundBlocksProcess.progress = (float)iterationCounter / totalIterations;
                yield return null;
            }

            if (iterationCounter == totalIterations)
            {
                _instantiatingGroundBlocksProcess.progress = 1f;
                yield return null;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Instantiates block renderers for all blocks that has a ground nearby and are not instantiated yet.
    /// Also sets references to these renderers.
    /// Requires all ground block renderers to be already instantiated. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstantiateNearToGroundBlocks()
    {
        List<Block> groundBlocks = GetBlockList(GetGroundBlockRenderers());

        Debug.Log("InstantiateNearToGroundBlocks -> Found " + groundBlocks.Count + " ground blocks.");

        int totalIterations = groundBlocks.Count;
        int iterationCounter = 0;

        foreach (Block block in groundBlocks)
        {
            iterationCounter++;

            List<Block> nearbyBlocks = BlockGridController.Singleton.grid.GetNearbyBlocks(block.coordinates);
            nearbyBlocks.ForEach(x =>
            {
                if (x.renderer == null)
                {
                    blockInstances.Add(InstantiateBlockRenderer(x));

                    if (x.type == BlockType.RockShale)
                    {
                        InstantiateFarWallBlocks(x.renderer);
                    }
                }
            });

            if (iterationCounter % 100 == 0)
            {
                _instantiatingNearToGroundProcess.progress = (float)iterationCounter / totalIterations;
                yield return null;
            }

            if (iterationCounter == totalIterations)
            {
                _instantiatingNearToGroundProcess.progress = 1f;
                yield return null;
            }
        }

        yield return null;
    }

    private IEnumerator InstantiateAndRenderBlockGridEntrance(int index = 0)
    {
        GameObject entrancePlaceHolder = new GameObject("Entrance");
        entrancePlaceHolder.transform.parent = transform;
        entrancePlaceHolder.transform.localPosition = BlockGridController.Singleton.grid.startingPosition.GetPositionOffset();

        entrance = Instantiate(
            BlockGridController.Singleton.prefabs.entrancePrefabs[index],
            entrancePlaceHolder.transform
        );

        yield return null;
    }

    /// <summary>
    /// Renders all blocks that has renderer attached
    /// </summary>
    /// <returns></returns>
    private IEnumerator RenderBlocks()
    {
        int totalIterations = blockInstances.Count;
        int iterationCounter = 0;

        Debug.Log("InstantiateBlockRenderer -> Blocks to render: " + totalIterations);

        foreach (BlockRenderer blockRenderer in blockInstances)
        {
            blockRenderer.Render();

            iterationCounter++;
            if (iterationCounter % 100 == 0)
            {
                _renderingBlocksProcess.progress = (float)iterationCounter / totalIterations;
                yield return null;
            }

            if (iterationCounter == totalIterations)
            {
                _renderingBlocksProcess.progress = 1f;
                yield return null;
            }
        }
    }
}
