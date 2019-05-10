using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGridPrefabs : MonoBehaviour {
    [Serializable]
    public class PrefabByType
    {
        public GameObject prefab;
        public BlockType type;
    }

    public List<PrefabByType> blockPrefabs = new List<PrefabByType>();
    public List<BlockGridEntrance> entrancePrefabs = new List<BlockGridEntrance>();

    public GameObject GetBlockPrefab(BlockType type)
    {
        PrefabByType prefabByType = blockPrefabs.Find(item => item.type == type);
        return prefabByType != null ? prefabByType.prefab : null;
    }
}
