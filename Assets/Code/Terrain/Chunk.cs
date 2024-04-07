using System.Collections;
using UnityEngine;

namespace Game.Terrain
{
    public sealed class Chunk
    {
        public enum Axis
        {
            X, Y, Z
        }

        public Vector3 Size { get; internal set; }

        public Vector3 Location { get; internal set; }

        public GameObject Parent { get; internal set; }

        private readonly int _gridSize;
        private readonly Material _material;
        private GameObject[,,] _descendants;

        private readonly MutatorFactory _runtimeMutator;

        public Chunk(Vector3 location, Vector3 size, int gridSize, float seed,Material material, MutatorCollection mutators)
        {
            Size = size;
            Location = location;
            _gridSize = gridSize;
            _material = material;
            _runtimeMutator = mutators.RuntimeMutator;

            // Establish the chunk
            BuildChunk();

            // Apply all intializers
            foreach (MutatorFactory mutator in mutators.InitializerMutators)
            {
                mutator.Mutate(seed, ref _descendants);
            }
        }

        public void StepMutator(float seed)
        {
            _runtimeMutator.Mutate(seed, ref _descendants);
        }

        public IEnumerator StepMutatorAsync(float seed)
        {
            _runtimeMutator.Mutate(seed, ref _descendants);

            yield return null;
        }

        public float GetEndPosition(Axis axis)
        {
            return axis switch
            {
                Axis.X => Location.x + (Size.x * _gridSize),
                Axis.Y => Location.y + (Size.y * _gridSize),
                Axis.Z => Location.z + (Size.z * _gridSize),
                _ => 0,
            };
        }

        private void BuildChunk()
        {
            // Create the terrain chunk parent
            Parent = new GameObject($"Terrain Chunk");
            Parent.transform.position = Location;

            _descendants = new GameObject[(int)Size.x, (int)Size.y, (int)Size.z];

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    for (int z = 0; z < Size.z; z++)
                    {
                        Vector3 voxelPhysicalLocation = new(Location.x + (x * _gridSize),
                                                            Location.y + (y * _gridSize),
                                                            Location.z + (z * _gridSize));

                        _descendants[x, y, z] = CreateVoxel(voxelPhysicalLocation);
                        _descendants[x, y, z].transform.SetParent(Parent.transform);
                    }
                }
            }
        }

        private GameObject CreateVoxel(Vector3 location)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = location;
            cube.transform.localScale = new Vector3(_gridSize, _gridSize, _gridSize);

            cube.GetComponent<Renderer>().material = _material;

            return cube;
        }
    }

}
