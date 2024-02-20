using System;
using UnityEngine;

namespace Game.Terrain
{
    [Serializable]
    public class TerrainChunk
    {
        public Chunk Floor;
    }

    [Serializable]
    public class MapSettings
    {
        public Material VoxelMaterial;
        public MapChunkSettings MapChunkSettings = new();
    }

    [Serializable]
    public class MapChunkSettings
    {
        public int GridSize = 20;
        public Vector3 ChunkSize = new(20, 1, 50);
    }
}
