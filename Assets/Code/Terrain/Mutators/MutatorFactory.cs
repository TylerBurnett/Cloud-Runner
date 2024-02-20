using UnityEngine;

namespace Game.Terrain
{
    public abstract class MutatorFactory
    {
        public abstract void Mutate(float seed, ref GameObject[,,] descendants);
    }
}
