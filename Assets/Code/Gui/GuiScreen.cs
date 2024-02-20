using Game.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Gui
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class GuiScreen : GameServiceBase
    {
        public delegate void ShowScreenEvent(string ScreenName);

        public delegate void HideScreenEvent(string ScreenName);

        public class ScreenArguements
        {
            public string ScreenName;

            public bool InitialGameScreen = false;
        }

        protected string ScreenName { get; private set; }

        protected UIDocument document { get; private set; }

        /// <summary>
        /// Functions as a sudo constructor params since we cant construct in unity
        /// </summary>
        protected abstract ScreenArguements SetIntialArgs();

        protected override void OnAwake()
        {
            ScreenArguements screenArguements = SetIntialArgs();

            ScreenName = screenArguements.ScreenName;
            document = gameObject.GetComponent<UIDocument>();

            // Setting intial state for game entry (There should only be one screen set to true).
            document.rootVisualElement.visible = screenArguements.InitialGameScreen;

            EventService<ShowScreenEvent>.Register((name) =>
            {
                if (name == ScreenName)
                {
                    document.rootVisualElement.visible = true;
                    OnScreenShown();
                }
            });

            EventService<HideScreenEvent>.Register((name) =>
            {
                if (name == ScreenName)
                {
                    document.rootVisualElement.visible = false;
                    OnScreenHidden();
                }
            });
        }

        protected virtual void OnScreenShown()
        {
        }

        protected virtual void OnScreenHidden()
        {
        }
    }
}