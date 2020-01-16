using System;

namespace ATInternet
{
    #region IdentifiedVisitor
    public class IdentifiedVisitor
    {
        #region Members

        private Tracker tracker;

        private bool persistIdentifiedVisitor;

        #endregion

        #region Constructor

        internal IdentifiedVisitor(Tracker tracker)
        {
            this.tracker = tracker;
            persistIdentifiedVisitor = Boolean.Parse(tracker.configuration.parameters["persistIdentifiedVisitor"]);
        }

        #endregion

        #region Methods

        public Tracker Set(int visitorId)
        {
            Unset();
            return Save("an", "ATVisitorNumeric", visitorId.ToString());
        }

        public Tracker Set(int visitorId, int visitorCategory)
        {
            Set(visitorId);
            return Save("ac", "ATVisitorCategory", visitorCategory.ToString());
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Tracker Set(string visitorId)
        {
            Unset();
            return Save("at", "ATVisitorText", visitorId);
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Tracker Set(string visitorId, int visitorCategory)
        {
            Set(visitorId);
            return Save("ac", "ATVisitorCategory", visitorCategory.ToString());
        }

        public void Unset()
        {
            tracker.UnsetParam("an")
                .UnsetParam("ac")
                .UnsetParam("at");
            Tracker.LocalSettings.Values["ATVisitorNumeric"] = null;
            Tracker.LocalSettings.Values["ATVisitorText"] = null;
            Tracker.LocalSettings.Values["ATVisitorCategory"] = null;
        }

        internal Tracker Save(string key, string localSettingsKey, string value)
        {
            if (persistIdentifiedVisitor)
            {
                Tracker.LocalSettings.Values[localSettingsKey] = value;
            }
            else
            {
                tracker.SetParam(key, value, new ParamOption() { Persistent = true });
            }

            return tracker;
        }

        #endregion

    }

    #endregion
}
