using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameplayEvents
{
    public abstract class GameplayEvent : IDisposable
    {
        protected List<GameObject> DisposableObjects;

        public GameplayEvent()
        {
            DisposableObjects = new List<GameObject>();
        }

        public abstract IEnumerator ShowWarning();

        public abstract void Trigger(Vector3 playerPosition);

        public void Dispose()
        {
            foreach (GameObject gameObject in DisposableObjects)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}