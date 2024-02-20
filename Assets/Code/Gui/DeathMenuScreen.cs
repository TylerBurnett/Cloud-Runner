using Game.Global;
using Game.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gui
{
    public class DeathMenuScreeen : GuiScreen
    {
        public delegate void RestartGameButtonPressedEvent();

        private float _distanceTravelled;

        private Label _distanceLabel;

        private string _distanceLabelText;

        protected override ScreenArguements SetIntialArgs()
        {
            return new()
            {
                ScreenName = "Death Menu",
                InitialGameScreen = false,
            };
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            document.rootVisualElement.Q<Button>("FinishGameButton").clicked += OnStartButtonPressed;

            _distanceLabel = document.rootVisualElement.Q<Label>("DistanceLabel");
            _distanceLabelText = _distanceLabel.text;
        }

        protected override void OnScreenShown()
        {
            Vector3 playerPosition = EventService<GetPlayerPositionEvent>.Trigger();

            _distanceTravelled = Mathf.Round(Vector3.Distance(new Vector3(0, 0, 0), playerPosition));

            _ = StartCoroutine(RunDeathCounterAnimation());
        }

        private void OnStartButtonPressed()
        {

        }

        private IEnumerator RunDeathCounterAnimation(int rate = 1)
        {
            float currentDistance = 0;

            for (int i = 0; i > _distanceTravelled; i += rate)
            {
                yield return 0;

                currentDistance = Mathf.Max(currentDistance += rate, _distanceTravelled);
                _distanceLabel.text = $"{_distanceLabelText} {currentDistance} meters";
            }
        }
    }
}