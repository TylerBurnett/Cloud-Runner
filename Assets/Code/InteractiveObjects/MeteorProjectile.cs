using Game.Common;
using Game.Global;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.InteractiveObjects
{



    public class MeteorProjectile : MonoBehaviour
    {
        private bool _hasExploded = false;

        private const float ExplosionForce = 30;
        private const float ExplosionForceUpwardsModifier = 1;

        private const float SpawnDistance = 5;

        private const float DebrisSizeMin = 1;
        private const float DebrisSizeMax = 3;

        private const float DebrisWeightFactor = 0.1f;

        protected void Awake()
        {
            _ = gameObject.AddComponent<DeathObject>();
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (!_hasExploded)
            {
                Destroy(gameObject.GetComponent<DeathObject>());
                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.GetComponent<Collider>());
                Destroy(gameObject.GetComponent<MeshRenderer>());

                _ = StartCoroutine(SpawnDebris(30));

                EventService<CameraShakeEventWithIntensity>.Trigger(0.3f, 7);
                _hasExploded = true;
            }
        }

        private IEnumerator SpawnDebris(int count)
        {
            List<Rigidbody> debris = new();

            for (int i = 0; i < count; i++)
            {
                GameObject chunk = GameObject.CreatePrimitive(PrimitiveType.Cube);

                chunk.AddComponent<DeathObject>();
                Rigidbody rigidbody = chunk.GetComponent<Rigidbody>();

                float projectileSize = Random.Range(DebrisSizeMin, DebrisSizeMax);
                rigidbody.transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);
                rigidbody.mass += projectileSize * DebrisWeightFactor;

                rigidbody.transform.position = new Vector3()
                {
                    x = Random.Range(transform.position.x - SpawnDistance, transform.position.x + SpawnDistance),
                    y = Random.Range(transform.position.y, transform.position.y + SpawnDistance),
                    z = Random.Range(transform.position.z - SpawnDistance, transform.position.z + SpawnDistance)
                };

                debris.Add(rigidbody);
            }

            yield return Enumerators.WaitForFrames(4); // This is required because the phyisics system hasn't registered the change in location yet,
                                                       // by waiting a whole 2 frames we are allowing time for it to register the change before trying to calc the explosion

            foreach (Rigidbody rigidbody in debris)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, 50, ExplosionForceUpwardsModifier, ForceMode.Impulse);
            }
        }
    }
}