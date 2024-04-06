using Game.Common;
using Game.Global;
using Game.Player;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gui
{
    public class DeathMenuScreeen : GuiScreen
    {
        public delegate void FinishButtonPressedEvent();

        public Color BackgroundColor;

        private float _distanceTravelled;

        private Label _distanceLabel;

        private readonly string _distanceLabelText;

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

            document.rootVisualElement.Q<Button>("FinishGameButton").clicked += OnFinishButtonPressed;

            _distanceLabel = document.rootVisualElement.Q<Label>("DistanceLabel");
        }

        protected override void OnScreenShown()
        {
            Vector3 playerPosition = EventService<GetPlayerPositionEvent>.Trigger();

            _distanceTravelled = Mathf.Round(Vector3.Distance(new Vector3(0, 0, 0), playerPosition));
            StartCoroutine(Enumerators.MoveTowards(0, _distanceTravelled, 1, (float distance) => _distanceLabel.text = $"{_distanceLabelText} {Math.Round(distance)} meters"));
        }

        private void OnFinishButtonPressed()
        {
            EventService<FinishButtonPressedEvent>.Trigger();
        }
    }
}