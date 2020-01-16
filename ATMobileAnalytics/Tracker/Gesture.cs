namespace ATInternet
{
    #region GestureAction
    public enum GestureAction
    {
        Touch, Navigate, Download, Exit, InternalSearch
    }

    #endregion

    #region Gesture
    public class Gesture : BusinessObject
    {
        #region Members
        public string Name { get; set; }

        public string Chapter1 { get; set; }

        public string Chapter2 { get; set; }

        public string Chapter3 { get; set; }

        public GestureAction Action { get; set; }

        public int Level2 { get; set; }

        #endregion

        #region Constructor

        internal Gesture(Tracker tracker) : base(tracker)
        {
            Action = GestureAction.Touch;
            Level2 = 0;
            Name = string.Empty;
        }

        #endregion

        #region Methods

        public void SendNavigation()
        {
            Action = GestureAction.Navigate;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendExit()
        {
            Action = GestureAction.Exit;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendDownload()
        {
            Action = GestureAction.Download;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendTouch()
        {
            Action = GestureAction.Touch;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendSearch()
        {
            Action = GestureAction.InternalSearch;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            if (!string.IsNullOrEmpty(TechnicalContext.ScreenName))
            {
                tracker.SetParam("pclick", TechnicalContext.ScreenName, new ParamOption() { Encode = true});
            }

            if (TechnicalContext.Level2 > 0)
            {
                tracker.SetParam("s2click", TechnicalContext.Level2);
            }

            if(Level2 > 0)
            {
                tracker.SetParam("s2", Level2);
            }

            string value = Chapter1 == null ? string.Empty : Chapter1 + "::";
            value = Chapter2 == null ? value : value + Chapter2 + "::";
            value = Chapter3 == null ? value : value + Chapter3 + "::";
            value += Name;

            string result;
            switch (Action)
            {
                case GestureAction.Download:
                    result = "T";
                    break;
                case GestureAction.Exit:
                    result = "S";
                    break;
                case GestureAction.InternalSearch:
                    result = "IS";
                    break;
                case GestureAction.Touch:
                    result = "A";
                    break;
                default:
                    result = "N";
                    break;
            }

            tracker.SetParam("click", result).Event.Set("click", result, value);
        }

        #endregion

    }

    #endregion

    #region Gestures

    public class Gestures
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Gestures(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public Gesture Add()
        {
            Gesture gest = new Gesture(tracker);
            tracker.businessObjects.Add(gest.id, gest);
            tracker.objectIndex++;

            return gest;
        }

        public Gesture Add(string name)
        {
            Gesture gest = Add();
            gest.Name = name;

            return gest;
        }

        public Gesture Add(string name, string chapter1)
        {
            Gesture gest = Add(name);
            gest.Chapter1 = chapter1;

            return gest;
        }

        public Gesture Add(string name, string chapter1, string chapter2)
        {
            Gesture gest = Add(name,chapter1);
            gest.Chapter2 = chapter2;

            return gest;
        }

        public Gesture Add(string name, string chapter1, string chapter2, string chapter3)
        {
            Gesture gest = Add(name, chapter1, chapter2);
            gest.Chapter3 = chapter3;

            return gest;
        }

        #endregion
    }

    #endregion
}
