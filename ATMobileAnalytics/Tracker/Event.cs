namespace ATInternet
{
    #region Event
    internal class Event
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Event(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        internal Tracker Set(string category, string action, string label)
        {
            return Set(category, action, label, "{}");
        }

        internal Tracker Set(string category, string action, string label, string value)
        {
            return tracker.SetParam("type", category)
                .SetParam("action", action)
                .SetParam("p", label, new ParamOption() { RelativeParameterKey = "idclient", RelativePosition = RelativePosition.After, Encode = true })
                .SetParam("stc", value, new ParamOption() { Encode = true, Append = true});
        }

        #endregion
    }

    #endregion
}
