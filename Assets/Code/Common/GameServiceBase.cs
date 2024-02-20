using Game.Gameplay;
using Game.Global;
using UnityEngine;

namespace Game.Common
{
    /// <summary>
    /// Utility parent class which abstracts the complexity away from managing current gameplay state
    /// WARNING: Do not use on non-persistent behaviours otherwise you will get consistent issues with mis-matched state.
    /// </summary>
    public abstract class GameServiceBase : MonoBehaviour
    {
        protected bool IsPlaying => _isPlaying && !IsPaused;

        protected bool IsPaused { get; private set; }

        private bool _isPlaying = false;

        protected void Awake()
        {
            EventService<GameplayStartEvent>.Register(HandleStart);
            EventService<GameplayEndEvent>.Register(HandleEnd);

            EventService<GameplayPauseEvent>.Register(HandlePause);
            EventService<GameplayResumeEvent>.Register(HandleResume);

            OnAwake();
        }

        private void HandleStart()
        {
            _isPlaying = true;
            OnGameplayStart();
        }

        private void HandleEnd()
        {
            _isPlaying = false;
            OnGameplayEnd();
        }

        private void HandlePause()
        {
            IsPaused = true;
            OnGameplayPause();
        }

        private void HandleResume()
        {
            IsPaused = false;
            OnGameplayResume();
        }

        protected virtual void OnAwake()
        {        }

        protected virtual void OnGameplayStart()
        {

        }

        protected virtual void OnGameplayEnd()
        {

        }

        protected virtual void OnGameplayPause()
        {

        }

        protected virtual void OnGameplayResume()
        {

        }
    }
}
