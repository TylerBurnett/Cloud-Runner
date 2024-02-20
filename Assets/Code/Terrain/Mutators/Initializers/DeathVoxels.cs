using Game.InteractiveObjects;
using UnityEngine;


namespace Game.Terrain.Mutators.Initializers
{
    public sealed class DeathVoxels : MutatorFactory
    {
        private readonly float _percentage;

        public DeathVoxels(float percentage)
        {
            _percentage = percentage;
        }

        public override void Mutate(float _, ref GameObject[,,] descendants)
        {
            int maxX = descendants.GetLength(0);
            int maxZ = descendants.GetLength(2);

            int totalSpikes = Mathf.RoundToInt(maxX * maxZ * _percentage);

            for (int i = 0; i < totalSpikes; i++)
            {
                int pointX = Random.Range(0, maxX);
                int pointZ = Random.Range(0, maxZ);

                GameObject voxel = descendants[pointX, 0, pointZ];

                if (voxel != null && voxel.GetComponent<StaticDeathObject>() == null)
                {
                    voxel.AddComponent<StaticDeathObject>();
                }
            }
        }
    }
}
