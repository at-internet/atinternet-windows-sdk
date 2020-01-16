using System.Collections.Generic;

namespace ATInternet
{
    #region SmartTag
    public class SmartTag
    {
        #region Members

        /// <summary>
        /// Collection trackers
        /// </summary>
        private Dictionary<string, Tracker> trackers;

        public Tracker defaultTracker {
            get
            {
                return GetTracker("defaultTracker");
            }
        }

        /// <summary>
        /// Singleton tracker queue
        /// </summary>
        static readonly SmartTag _instance = new SmartTag();
        public static SmartTag Instance
        {
            get
            {
                return _instance;
            }
        }

        #region Constructor

        private SmartTag()
        {
            trackers = new Dictionary<string, Tracker>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create or get a tracker instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tracker GetTracker(string name)
        {
            if (trackers.ContainsKey(name))
            {
                return trackers[name];
            }
            else
            {
                trackers[name] = new Tracker();
                return trackers[name];
            }
        }

        /// <summary>
        /// Create or get a tracker instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tracker GetTracker(string name, IDictionary<string, string> config)
        {
            if (trackers.ContainsKey(name))
            {
                return trackers[name];
            }
            else
            {
                trackers[name] = new Tracker(config);
                return trackers[name];
            }
        }

        #endregion

        #endregion
    }

    #endregion
}
