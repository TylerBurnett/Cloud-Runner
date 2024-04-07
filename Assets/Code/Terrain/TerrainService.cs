using Game.Common;
using Game.Gameplay;
using Game.Global;
using Game.Player;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Game.Terrain
{
    public delegate void ChunkEnteredEvent(int chunkIndex);

    public delegate void ChunkDestroyedEvent(int chunkIndex);


    [Serializable]
    public class TerrainService : GameServiceBase
    {
        #region Public Properties

        public float CullingDistance = 150;

        public MapSettings Settings = new();

        public Collection<Chunk> Chunks = new();

        public int CurrentChunkIndex
        {
            get;
            private set;
        } = 0;

        public Chunk CurrentChunk => Chunks[CurrentChunkIndex];

        #endregion Public Properties

        #region Private Properties

        private float _seed = 0.001f;

        private Vector3 _playerPosition;

        #endregion Private Properties

        protected void Start()
        {
            EventService<GameEnterEvent>.Register(BeginGeneration);
            EventService<PlayerMoveEvent>.Register(PlayerMoved);
        }

        protected void Update()
        {
            if (!IsPaused)
            {
                UpdateTerrain();
            }
        }

        protected override void OnGameplayEnd()
        {
            ResetChunks();
        }

        private void UpdateTerrain()
        {
            if (Chunks.Count > 0)
            {
                _seed += 0.001f;

                // Cut rendering a bit by only modifying terrain every second frame
                if (Time.frameCount % 2 == 0)
                {
                    CurrentChunk.StepMutator(_seed);

                    // If either of the two chunks are too far out of view don't bother modifying the terrain.
                    if (IsChunkInRange(_playerPosition, Chunks[CurrentChunkIndex + 1], CullingDistance))
                    {
                        Chunks[CurrentChunkIndex + 1].StepMutator(_seed);
                    }

                    if (CurrentChunkIndex > 0 && IsChunkInRange(_playerPosition, Chunks[CurrentChunkIndex - 1], CullingDistance))
                    {
                        Chunks[CurrentChunkIndex - 1].StepMutator(_seed);
                    }
                }
            }
        }

        private void PlayerMoved(Vector3 position)
        {
            if (IsPlaying && Chunks.Count > 0)
            {
                if (position.z > CurrentChunk.GetEndPosition(Chunk.Axis.Z))
                {
                    AppendChunk();
                    CurrentChunkIndex++;

                    EventService<ChunkEnteredEvent>.Trigger(CurrentChunkIndex);

                    OptimiseChunks();
                }
            }

            _playerPosition = position;
        }

        /// <summary>
        /// As the name implies, this will initialize terrain generation
        /// By generating the first chunk, you are in turn creating the pathway which of course will trigger subsequent chunk generations.
        /// </summary>
        private void BeginGeneration()
        {
            if (Chunks.Count == 0)
            {
                AppendChunk(2);
            }
        }

        /// <summary>
        /// Appends another chunk to the current collection for use in game.
        /// </summary>
        /// <param name="count"></param>
        private void AppendChunk(int count = 1)
        {
            Vector3 chunkLocation = new(0, 0, 0);

            // This is ensure that each chunk is spawned center aligned to 0
            chunkLocation.x -= Settings.MapChunkSettings.ChunkSize.x * Settings.MapChunkSettings.GridSize / 2;

            for (int i = 0; i < count; i++)
            {
                if (Chunks.Count > 0)
                {
                    chunkLocation.z = Chunks.Last().GetEndPosition(Chunk.Axis.Z);
                }

                MutatorCollection mutators = MutatorGenerator.Generate();
                Chunks.Add(new Chunk(chunkLocation, Settings.MapChunkSettings.ChunkSize, Settings.MapChunkSettings.GridSize, _seed, Settings.VoxelMaterial, mutators));
            }
        }

        /// <summary>
        /// Optimises the game environment by removing old, un-used game chunks once they are out of view
        /// </summary>
        private void OptimiseChunks()
        {
            for (int i = 0; i < Chunks.Count - 1; i++)
            {
                if (Chunks[i] != null && CurrentChunkIndex - i > 2)
                {
                    Destroy(Chunks[i].Parent);
                    Chunks[i] = null;

                    EventService<ChunkDestroyedEvent>.Trigger(i);
                }
            }
        }

        /// <summary>
        /// Resets all game chunks and begins anew
        /// </summary>
        private void ResetChunks()
        {
            foreach (Chunk chunk in Chunks)
            {
                if (chunk != null)
                {
                    Destroy(chunk.Parent);
                }
            }

            Chunks.Clear();
            CurrentChunkIndex = 0;

            BeginGeneration();
        }

        public static bool IsChunkInRange(Vector3 playerPosition, Chunk chunk, float maxDist)
        {
            static float getDistance(float a, float b)
            {
                return Math.Abs(a - b);
            }

            float fromStart = getDistance(playerPosition.z, chunk.Location.z);
            float fromEnd = getDistance(playerPosition.z, chunk.GetEndPosition(Chunk.Axis.Z));

            return fromStart < maxDist || fromEnd < maxDist;
        }
    }
}
