using UnityEngine;

namespace Game.Terrain.Mutators.Runtime
{
    public sealed class Platforms : MutatorFactory
    {
        private readonly float _resolution;
        private readonly float _floor;

        /// <summary>
        /// Platform Mutator constructor
        /// </summary>
        /// <param name="percentage">This parameter dictates the resolution of the noise. High values will create very gradual sloping terrain, low values will create erratic jagged terrain.</param>
        /// <param name="magnitude">Dictates the minimum cut off for when terrain should be culled, value ranges are generally 0.0 to 1.0, respectively 0.0 no terrain loss, 1.0 all terrain loss.</param>
        public Platforms(float resolution, float floor)
        {
            _resolution = resolution;
            _floor = floor;
        }

        /// <summary>
        /// Mutates the ref chunk to create platforms.
        /// </summary>
        /// <param name="_">The seed that will be appended to each axis (X,Z) at time of perlin computation. Allows for deterministic terrain generation between chunks</param>
        /// <param name="descendants">The terrain chunk that is to be modified</param>
        public override void Mutate(float seed, ref GameObject[,,] descendants)
        {
            int lengthX = descendants.GetLength(0);
            int lengthZ = descendants.GetLength(2);

            for (int x = 0; x < lengthX; x++)
            {
                for (int z = 0; z < lengthZ; z++)
                {
                    if (descendants[x, 0, z] != null)
                    {
                        float height = Mathf.Max(Mathf.PerlinNoise((x + seed) / _resolution, (z + seed) / _resolution), 0);

                        if (height < _floor)
                        {
                            Object.Destroy(descendants[x, 0, z]);
                            descendants[x, 0, z] = null;
                        }
                    }
                }
            }
        }
    }
}
