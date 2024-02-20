using Game.Common;
using Game.Global;
using Game.Player;
using Game.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameplayEvents
{
    public class GameplayEventService : GameServiceBase
    {
        private int _chunkIndex;

        private readonly List<(int, GameplayEvent)> EventHistory = new();

        protected void Start()
        {

            EventService<ChunkEnteredEvent>.Register(ChunkEntered);

            EventService<ChunkDestroyedEvent>.Register(ChunkDestroyed);
        }

        private void ChunkDestroyed(int chunkIndex)
        {        }

        protected override void OnGameplayStart()
        {
            _ = StartCoroutine(ProcessEvents());
        }

        protected override void OnGameplayEnd()
        {
            foreach ((int, GameplayEvent) gameEvent in EventHistory)
            {
                gameEvent.Item2.Dispose();
            }

            EventHistory.Clear();
        }

        private void ChunkEntered(int chunkIndex)
        {
            _chunkIndex = chunkIndex;
        }

        private IEnumerator ProcessEvents()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5, 10));

                if (IsPlaying)
                {
                    (int, GameplayEvent) gameEvent = new(_chunkIndex, new MeteorGameplayEvent());

                    yield return gameEvent.Item2.ShowWarning();

                    Vector3 playerPosition = EventService<GetPlayerPositionEvent>.Trigger();
                    gameEvent.Item2.Trigger(playerPosition);

                    EventHistory.Add(gameEvent);
                }
                else
                {
                    break;
                }
            }        }
    }
}