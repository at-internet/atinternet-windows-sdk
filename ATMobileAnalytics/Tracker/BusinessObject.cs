using System;

namespace ATInternet
{
    #region BusinessObject
    public class BusinessObject
    {
        #region Members

        /// <summary>
        /// Hit id
        /// </summary>
        internal string id { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        internal long timestamp { get; set; }

        /// <summary>
        /// Tracker instance
        /// </summary>
        internal Tracker tracker { get; set; }

        /// <summary>
        /// Object index
        /// </summary>
        /// <param name="tracker"></param>
        internal int index { get; set; }

        #endregion

        #region Constructor

        internal BusinessObject(Tracker tracker)
        {
            this.tracker = tracker;
            id = Guid.NewGuid().ToString();
            index = tracker.objectIndex;
            timestamp = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).Ticks;
        }

        #endregion

        #region Methods

        internal virtual void SetEvent() { }

        #endregion
    }

    #endregion
}
