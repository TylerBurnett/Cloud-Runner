using Game.Global;
using Game.Gui;
using Game.InteractiveObjects;
using System.Collections;
using UnityEngine;

namespace Game.GameplayEvents
{
    public class MeteorGameplayEvent : GameplayEvent
    {
        private const float SpawnDistanceX = 200;
        private const float SpawnDistanceY = 300;
        private const float SpawnDistanceZ = 200;

        private const float TargetPositionOffsetZ = 100;

        private const float ProjectileSize = 10;

        private const float ProjectileVelocity = 50;
        private const float ProjectileTorque = 100000;

        public override void Trigger(Vector3 playerPosition)
        {
            Vector3 targetPosition = SelectTargetPosition(playerPosition);
            Vector3 spawnPosition = SelectSpawnPosition(targetPosition);

            DisposableObjects.Add(BuildProjectile(spawnPosition, targetPosition));
        }

        public override IEnumerator ShowWarning()
        {
            for (int i = 0; i < 3; i++)
            {
                EventService<GameHudScreen.MeteorIndicatorEvent>.Trigger(true);

                yield return new WaitForSeconds(0.2f);

                EventService<GameHudScreen.MeteorIndicatorEvent>.Trigger(false);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private static Vector3 SelectTargetPosition(Vector3 playerPosition)
        {
            Vector3 targetPosition = playerPosition;
            targetPosition.z += TargetPositionOffsetZ;

            return targetPosition;
        }

        private static Vector3 SelectSpawnPosition(Vector3 target)
        {
            return new Vector3()
            {
                x = Random.Range(target.x + SpawnDistanceX, target.x - SpawnDistanceX),
                y = SpawnDistanceY, // We fix the height since variations in height could ruin immersion.
                z = Random.Range(target.z + SpawnDistanceZ, target.z - SpawnDistanceZ),            };
        }

        private GameObject BuildProjectile(Vector3 spawn, Vector3 target)
        {
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Cube);

            projectile.transform.position = spawn;
            projectile.transform.localScale = new Vector3(ProjectileSize, ProjectileSize, ProjectileSize);

            projectile.AddComponent<MeteorProjectile>();

            Vector3 appliedForce = (target - projectile.transform.position) * ProjectileVelocity;

            Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
            rigidBody.AddForce(appliedForce);

            rigidBody.AddTorque(Vector3.up * ProjectileTorque);
            rigidBody.AddTorque(Vector3.left * ProjectileTorque);            return projectile;
        }
    }
}