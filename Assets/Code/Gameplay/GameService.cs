using Game.Common;
using Game.Global;
using Game.Gui;
using Game.InteractiveObjects;
using Game.Player;
using UnityEngine;

namespace Game.Gameplay
{
    public delegate void RespawnPlayerEvent();

    /// <summary>
    /// The Game Enter Event is triggered when the player is given access to the player controller and is capable of moving in the game world.
    /// </summary>
    public delegate void GameEnterEvent();

    /// <summary>
    /// The GameplayStartEvent is triggered when the player moves past the spawn threshold, triggering a run in the game.
    /// </summary>
    public delegate void GameplayStartEvent();

    public delegate void GameplayPauseEvent();

    public delegate void GameplayResumeEvent();

    public delegate void GameplayEndEvent();

    /// <summary>
    /// The Game Service acts mostly as the mediator for all gameplay related events
    /// </summary>
    public class GameService : GameServiceBase
    {
        public GameServiceSettings Settings = new();

        private GameObject _deathWall;
        private GameObject _respawn;

        private float _wallSpeed;

        protected void Start()
        {
            _deathWall = GameObject.FindGameObjectWithTag("DeathWall");
            _respawn = GameObject.FindGameObjectWithTag("Respawn_DeathWall");

            _deathWall.SetActive(false);

            EventService<PlayerMoveEvent>.Register(OnPlayerMoved);
            EventService<DeathObjectCollisionEvent>.Register(OnDeathObjectCollision);

            EventService<MainMenuScreen.StartButtonPressedEvent>.Register(OnGameEnter);
        }

        protected void Update()
        {
            if (IsPlaying)
            {
                MoveDeathWall();
            }
        }

        protected override void OnGameplayStart()
        {
            _wallSpeed = Settings.DeathWallInitialSpeed;
            _deathWall.SetActive(true);
        }

        protected override void OnGameplayEnd()
        {
            _deathWall.transform.position = _respawn.transform.position;
            _deathWall.SetActive(false);
        }

        private void OnGameEnter()
        {
            EventService<GuiScreen.ShowScreenEvent>.Trigger("Game Hud");
            EventService<GuiScreen.HideScreenEvent>.Trigger("Main Menu");
            EventService<GameEnterEvent>.Trigger();
        }

        private void OnPlayerMoved(Vector3 position)
        {
            if (IsPlaying)
            {
                if (position.z - _deathWall.transform.position.z <= 10 || position.y <= Settings.GameFloor)
                {
                    HandlePlayerDeath();
                }
            }
            else if (!IsPaused)
            {
                // If the player falls out of the world without starting game, respawn them
                if (position.y <= Settings.GameFloor)
                {
                    EventService<RespawnPlayerEvent>.Trigger();
                }
                // else if they have triggered the game start condition and aren't below floor start the game
                else if (position.z >= Settings.StartDistance)
                {
                    EventService<GameplayStartEvent>.Trigger();
                }
            }
        }

        private void OnDeathObjectCollision()
        {
            if (IsPlaying)
            {
                HandlePlayerDeath();
            }
        }

        private void MoveDeathWall()
        {
            Vector3 nextPosition = _deathWall.transform.position;
            nextPosition.z = Mathf.Lerp(nextPosition.z, nextPosition.z + _wallSpeed, Time.deltaTime); // TODO: this maths is wrong.

            _deathWall.transform.position = nextPosition;

            if (_wallSpeed <= Settings.DeathWallMaxSpeed)
            {
                _wallSpeed += Settings.DeathWallSpeedIncrement;
            }
        }

        private void HandlePlayerDeath()
        {
            PauseGame();

            EventService<GuiScreen.HideScreenEvent>.Trigger("Game Hud");
            EventService<GuiScreen.ShowScreenEvent>.Trigger("Death Menu");
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            EventService<GameplayPauseEvent>.Trigger();
        }
    }
}

