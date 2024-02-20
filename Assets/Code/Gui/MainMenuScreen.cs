using Game.Global;
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
        }

        private void OnStartButtonPressed()
        {
            EventService<StartButtonPressedEvent>.Trigger();
        }
    }
}