using Game.Global;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gui
{
    public class MainMenuScreen : GuiScreen
    {
        public delegate void StartButtonPressedEvent();

        protected override ScreenArguements SetIntialArgs()
        {
            return new()
            {
                ScreenName = "Main Menu",
                InitialGameScreen = true,
            };
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            document.rootVisualElement.Q<Button>("StartGameButton").clicked += OnStartButtonPressed;
            document.rootVisualElement.Q<Button>("ExitGameButton").clicked += OnExitButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            EventService<StartButtonPressedEvent>.Trigger();
        }

        private void OnExitButtonPressed()
        {
            Application.Quit();
        }
    }
}