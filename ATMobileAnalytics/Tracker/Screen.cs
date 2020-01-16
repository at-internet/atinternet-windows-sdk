using System;

namespace ATInternet
{
    #region ScreenAction
    public enum ScreenAction
    {
        View
    }

    #endregion

    #region ScreenInfo

    public class ScreenInfo : BusinessObject
    {
        #region Constructor

        internal ScreenInfo(Tracker tracker) : base(tracker) { }

        #endregion
    }

    #endregion

    #region Screen
    public class Screen : BusinessObject
    {
        #region Members

        public string Name { get; set; }

        public string Chapter1 { get; set; }

        public string Chapter2 { get; set; }

        public string Chapter3 { get; set; }

        public ScreenAction Action { get; set; }

        public bool IsBasketScreen { get; set; }

        public int Level2 { get; set; }

        #endregion

        #region Constructor

        internal Screen(Tracker tracker) : base(tracker)
        {
            Action = ScreenAction.View;
            Level2 = 0;
            Name = string.Empty;
        }

        #endregion

        #region Methods

        public void SendView()
        {
            Action = ScreenAction.View;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            if (Level2 > 0)
            {
                tracker.SetParam("s2", Level2);
            }

            if (IsBasketScreen)
            {
                tracker.SetParam("tp", "cart");
            }

            string value = Chapter1 == null ? string.Empty : Chapter1 + "::";
            value = Chapter2 == null ? value : value + Chapter2 + "::";
            value = Chapter3 == null ? value : value + Chapter3 + "::";
            value += Name;

            tracker.Event.Set("screen", Action.ToString().ToLower(), value);
        }

        #endregion
    }

    #endregion

    #region DynamicScreen
    public class DynamicScreen : BusinessObject
    {
        #region Members

        public string Name { get; set; }

        public string Chapter1 { get; set; }

        public string Chapter2 { get; set; }

        public string Chapter3 { get; set; }

        public ScreenAction Action { get; set; }

        public bool IsBasketScreen { get; set; }

        public int Level2 { get; set; }

        public string ScreenId { get; set; }

        public DateTimeOffset Update { get; set; }

        #endregion

        #region Constructor

        internal DynamicScreen(Tracker tracker) : base(tracker)
        {
            Name = string.Empty;
            Action = ScreenAction.View;
            Level2 = 0;
            ScreenId = string.Empty;
            Update = DateTime.Now;
        }

        #endregion

        #region Methods

        public void SendView()
        {
            Action = ScreenAction.View;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            if (ScreenId.Length > 255)
            {
                ScreenId = string.Empty;
                if (tracker.Delegate != null)
                {
                    tracker.Delegate.WarningDidOccur("ScreenId too long, replaced by empty value");
                }
            }

            if (Level2 > 0)
            {
                tracker.SetParam("s2", Level2);
            }

            if (IsBasketScreen)
            {
                tracker.SetParam("tp", "cart");
            }

            string value = Chapter1;
            value += Chapter2 == null ? string.Empty : "::" + Chapter2;
            value += Chapter3 == null ? string.Empty : "::" + Chapter3;

            tracker.SetParam("pid", ScreenId)
                .SetParam("pchap", value, new ParamOption() { Encode = true })
                .SetParam("pidt", Update.ToString("yyyyMMddHHmm"))
                .Event.Set("screen", Action.ToString().ToLower(), Name);
        }

        #endregion
    }

    #endregion

    #region Screens

    public class Screens
    {
        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        #endregion

        #region Constructor

        internal Screens(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods 

        public Screen Add()
        {
            Screen screen = new Screen(tracker);
            tracker.businessObjects.Add(screen.id, screen);
            tracker.objectIndex++;

            return screen;
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Screen Add(string name)
        {
            Screen screen = new Screen(tracker) { Name = name };
            tracker.businessObjects.Add(screen.id, screen);
            tracker.objectIndex++;

            return screen;
        }

        public Screen Add(string name, string chapter1)
        {
            Screen screen = Add(name);
            screen.Chapter1 = chapter1;
            return screen;
        }

        public Screen Add(string name, string chapter1, string chapter2)
        {
            Screen screen = Add(name, chapter1);
            screen.Chapter2 = chapter2;
            return screen;
        }

        public Screen Add(string name, string chapter1, string chapter2, string chapter3)
        {
            Screen screen = Add(name, chapter1, chapter2);
            screen.Chapter3 = chapter3;
            return screen;
        }

        #endregion
    }

    #endregion

    #region DynamicScreens

    public class DynamicScreens
    {
        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        #endregion

        #region Constructor

        internal DynamicScreens(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods 

        [Obsolete("Add(int, string, DateTimeOffset) is deprecated, please use Add(string, string, DateTimeOffset) instead.")]
        public DynamicScreen Add(int screenId, string name, DateTimeOffset update)
        {
            DynamicScreen dynamicScreen = new DynamicScreen(tracker)
            {
                ScreenId = screenId.ToString(),
                Name = name,
                Update = update
            };
            tracker.businessObjects.Add(dynamicScreen.id, dynamicScreen);
            tracker.objectIndex++;

            return dynamicScreen;
        }

        public DynamicScreen Add(string screenId, string name, DateTimeOffset update)
        {
            DynamicScreen dynamicScreen = new DynamicScreen(tracker)
            {
                ScreenId = screenId,
                Name = name,
                Update = update
            };
            tracker.businessObjects.Add(dynamicScreen.id, dynamicScreen);
            tracker.objectIndex++;

            return dynamicScreen;
        }

        [Obsolete("Add(int, string, DateTimeOffset, string) is deprecated, please use Add(string, string, DateTimeOffset, string) instead.")]
        public DynamicScreen Add(int screenId, string name, DateTimeOffset update, string chapter1)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update);
            dynamicScreen.Chapter1 = chapter1;
            return dynamicScreen;
        }

        public DynamicScreen Add(string screenId, string name, DateTimeOffset update, string chapter1)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update);
            dynamicScreen.Chapter1 = chapter1;
            return dynamicScreen;
        }

        [Obsolete("Add(int, string, DateTimeOffset, string, string) is deprecated, please use Add(string, string, DateTimeOffset, string, string) instead.")]

        public DynamicScreen Add(int screenId, string name, DateTimeOffset update, string chapter1, string chapter2)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update, chapter1);
            dynamicScreen.Chapter2 = chapter2;
            return dynamicScreen;
        }

        public DynamicScreen Add(string screenId, string name, DateTimeOffset update, string chapter1, string chapter2)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update, chapter1);
            dynamicScreen.Chapter2 = chapter2;
            return dynamicScreen;
        }

        [Obsolete("Add(int, string, DateTimeOffset, string, string, string) is deprecated, please use Add(string, string, DateTimeOffset, string, string, string) instead.")]
        public DynamicScreen Add(int screenId, string name, DateTimeOffset update, string chapter1, string chapter2, string chapter3)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update, chapter1, chapter2);
            dynamicScreen.Chapter3 = chapter3;
            return dynamicScreen;
        }

        public DynamicScreen Add(string screenId, string name, DateTimeOffset update, string chapter1, string chapter2, string chapter3)
        {
            DynamicScreen dynamicScreen = Add(screenId, name, update, chapter1, chapter2);
            dynamicScreen.Chapter3 = chapter3;
            return dynamicScreen;
        }

        #endregion
    }

    #endregion
}
