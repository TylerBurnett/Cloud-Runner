using Game.Global;
using Game.Player;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gui
{
    public class GameHudScreen : GuiScreen
    {
        public delegate void MeteorIndicatorEvent(bool visible);

        private VisualElement _meteorIndicator;

        private Label _distanceLabel;

        protected override ScreenArguements SetIntialArgs()
        {
            return new()
            {
                ScreenName = "Game Hud",
                InitialGameScreen = false,
            };
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            EventService<PlayerMoveEvent>.Register(OnPlayerMoved);

            _meteorIndicator = document.rootVisualElement.Q<VisualElement>("MeteorIndicator");
            _distanceLabel = document.rootVisualElement.Q<Label>("DistanceLabel");

            EventService<MeteorIndicatorEvent>.Register((bool visible) => _meteorIndicator.visible = visible);
        }

        private void OnPlayerMoved(Vector3 position)
        {
            if (IsPlaying)
            {
                _distanceLabel.text = Math.Round(Vector3.Distance(Vector3.zero, position)).ToString();
            }
        }
    }
}