using UnityEngine;

namespace Game.Terrain.Mutators.Runtime
{
    public sealed class CosineScale : MutatorFactory
    {
        private readonly float _resolution;
        private readonly float _scale;

        /// <summary>
        /// Factory function for VoxelGrid Mutator which modifies the Z magnitude of scale based on provided seed.
        /// </summary>
        /// <param name="resolution">This parameter dictates the resolution of the noise. High values will create very gradual sloping terrain, low values will create erratic jagged terrain.</param>
        /// <param name="scale">The scale of the noise dictates the height at which it will be modified at, low values = low height, high values = high height</param>
        public CosineScale(float resolution, float scale)
        {
            _scale = scale;
            _resolution = resolution;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed">The seed that will be appended to each axis (X,Z) at time of perlin computation. Allows for deterministic terrain generation between chunks</param>
        /// <param name="descendants">The terrain chunk that is to be modified</param>
        public override void Mutate(float seed, ref GameObject[,,] descendants)
        {
            for (int x = 0; x < descendants.GetLength(0); x++)
            {
                for (int z = 0; z < descendants.GetLength(2); z++)
                {
                    if (descendants[x, 0, z] != null)
                    {
                        Vector3 voxel = descendants[x, 0, z].transform.transform.localScale;
                        voxel.y = Mathf.Max(Mathf.Cos(z + (seed / _resolution)) * _scale, 0);

                        descendants[x, 0, z].transform.localScale = voxel;
                    }
                }
            }
        }
    }
}


