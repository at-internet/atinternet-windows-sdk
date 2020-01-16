using System.Collections.Generic;

namespace ATInternet
{
    #region CustomObject
    public class CustomObject : BusinessObject
    {
        #region Members

        public string Value { get; set; }

        #endregion

        #region Constructor
        internal CustomObject(Tracker tracker) : base(tracker)
        {
            Value = "{}";
        }

        #endregion

        #region Methods
        internal override void SetEvent()
        {
            tracker.SetParam("stc", Value, new ParamOption() { Encode = true, Append = true});
        }

        #endregion
    }

    #endregion

    #region CustomObjects
    public class CustomObjects
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal CustomObjects(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public CustomObject Add(string customObject)
        {
            CustomObject customObj = new CustomObject(tracker);
            customObj.Value = customObject;
            tracker.businessObjects.Add(customObj.id, customObj);
            tracker.objectIndex++;

            return customObj;
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public CustomObject Add(IDictionary<string,object> customObject)
        {
            return Add(Tool.ConvertToString(customObject));
        }

        #endregion

    }

    #endregion
}
