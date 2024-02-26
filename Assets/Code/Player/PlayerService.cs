using Cinemachine;
using Game.Common;
using Game.Gameplay;
using Game.Global;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public delegate Vector3 GetPlayerPositionEvent();

    public delegate void PlayerMoveEvent(Vector3 position);

    /// <summary>
    /// Manages the Player and the MainCamera, needs to be attached to the player object.
    /// </summary>
    internal class PlayerService : GameServiceBase
    {
        public float MaxFov = 100;
        public float MinFov = 80;

        private GameObject _camera;
        private GameObject _respawn;

        private StarterAssetsInputs _input;

        private Vector3 _lastPosition;

        protected void Start()
        {
            _camera = GameObject.FindGameObjectWithTag("PlayerCamera");
            _respawn = GameObject.FindGameObjectWithTag("Respawn_Player");

            _input = GetComponent<StarterAssetsInputs>();
            _lastPosition = gameObject.transform.position;

            LockPlayer();
            UnlockCursor();

            EventService<GameEnterEvent>.Register(UnlockPlayer);
            EventService<RespawnPlayerEvent>.Register(Respawn);

            EventService<GetPlayerPositionEvent>.Register(() => gameObject.transform.position);
        }

        protected void Update()
        {
            if (!IsPaused)
            {
                // Trigger the player move event for any vector3 modification.
                if (gameObject.transform.position != _lastPosition)
                {
                    _lastPosition = gameObject.transform.position;
                    EventService<PlayerMoveEvent>.Trigger(gameObject.transform.position);
                }

                UpdateFov();
            }
        }

        protected override void OnGameplayEnd()
        {
            Respawn();
        }

        protected override void OnGameplayPause()
        {
            LockPlayer();
        }

        protected override void OnGameplayResume()
        {
            UnlockPlayer();
        }

        private void UpdateFov()
        {
            CinemachineVirtualCamera virtualCamera = _camera.GetComponent<CinemachineVirtualCamera>();

            float fovTarget = _input.move == Vector2.zero ? MinFov : MaxFov;
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, fovTarget, Time.deltaTime * 5);
        }

        private void Respawn()
        {
            LockPlayer();

            gameObject.transform.position = _respawn.transform.position;

            UnlockPlayer();
        }

        private void UnlockPlayer()
        {
            GetComponent<CharacterController>().enabled = true;
            GetComponent<FirstPersonController>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;

            UnlockCursor();
        }

        private void LockPlayer()
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<PlayerInput>().enabled = false;

            LockCursor();
        }

        private static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private static void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
