using Cinemachine;
using Game.Common;
using Game.Gameplay;
using Game.Global;
using StarterAssets;
using UnityEngine;

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
        private Transform _respawnTransform;

        private FirstPersonController _firstPersonController;
        private CharacterController _characterController;
        private StarterAssetsInputs _input;

        private Vector3 _lastPosition;

        protected void Start()
        {
            _camera = GameObject.FindGameObjectWithTag("PlayerCamera");
            _respawnTransform = GameObject.FindGameObjectWithTag("Respawn_Player").transform;

            _firstPersonController = GetComponent<FirstPersonController>();
            _characterController = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            _lastPosition = gameObject.transform.position;

            LockPlayer();
            UnlockCursor();

            EventService<GameEnterEvent>.Register(UnlockPlayer);
            EventService<RespawnPlayerEvent>.Register(() => Respawn());

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
            Respawn(true);
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

        private void Respawn(bool locked = false)
        {
            LockPlayer();

            gameObject.transform.SetPositionAndRotation(_respawnTransform.position, _respawnTransform.rotation);
            if (!locked) UnlockPlayer();
        }

        private void UnlockPlayer()
        {
            _characterController.enabled = true;
            _firstPersonController.enabled = true;
            _input.enabled = true;

            LockCursor();
        }

        private void LockPlayer()
        {
            _characterController.enabled = false;
            _firstPersonController.enabled = false;
            _input.enabled = false;

            UnlockCursor();
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
