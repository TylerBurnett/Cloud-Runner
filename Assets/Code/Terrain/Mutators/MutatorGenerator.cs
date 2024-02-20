using Game.Terrain.Mutators.Initializers;
using Game.Terrain.Mutators.Runtime;
using System;

namespace Game.Terrain
{
    public static class MutatorGenerator
    {
        private static readonly MutatorFactory[] Initializers = { new Platforms(3, 0.4f), new Platforms(3, 0.5f) };
        private static readonly MutatorFactory[] Obstacles = { new DeathVoxels(0.2f), new DeathVoxels(0.3f), new DeathVoxels(0.1f) };
        private static readonly MutatorFactory[] RuntimeMutators = { new PerlinScale(3, 50), new PerlinScale(6, 50) };

        public static MutatorCollection Generate()
        {            Random random = new();

            return new()            {
                InitializerMutators = new MutatorFactory[] { Initializers[random.Next(Initializers.Length)], Obstacles[random.Next(Obstacles.Length)] },
                RuntimeMutator = RuntimeMutators[random.Next(RuntimeMutators.Length)],
            };
        }
    }

    public class MutatorCollection
    {
        public MutatorFactory[] InitializerMutators { get; set; }

        public MutatorFactory RuntimeMutator { get; set; }
    }
}