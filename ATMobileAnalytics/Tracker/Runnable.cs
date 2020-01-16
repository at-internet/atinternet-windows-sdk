using System.Collections.Generic;

namespace ATInternet
{
    #region Runnable
    interface Runnable
    {
        /// <summary>
        /// Method executed in queue
        /// </summary>
        void Run();
    }

    #endregion

    #region SetConfigRunnable

    class SetConfigRunnable : Runnable
    {
        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        internal Tracker tracker;

        /// <summary>
        /// boolean to know if all the config will be changed and remove the precedent
        /// </summary>
        bool changeAllConfig, overriding;

        /// <summary>
        /// Variable to change only one config
        /// </summary>
        string key, value;

        /// <summary>
        /// New Config
        /// </summary>
        IDictionary<string, string> newConfig;

        /// <summary>
        /// Callback called when the task is finished
        /// </summary>
        TrackerReadyHandler callback;

        #endregion

        #region Construtors

        private SetConfigRunnable(Tracker tracker, TrackerReadyHandler callback)
        {
            this.tracker = tracker;
            this.callback = callback;
        }

        internal SetConfigRunnable(Tracker tracker, string key, string value, TrackerReadyHandler callback) : this(tracker, callback)
        {
            changeAllConfig = false;
            this.key = key;
            this.value = value;
        }

        internal SetConfigRunnable(Tracker tracker, IDictionary<string, string> newConfig, bool overriding, TrackerReadyHandler callback) : this(tracker, callback)
        {
            changeAllConfig = true;
            this.overriding = overriding;
            this.newConfig = newConfig;
        }

        #endregion

        public void Run()
        {
            if (changeAllConfig)
            {
                if (overriding)
                {
                    tracker.configuration.parameters.Clear();
                }
                foreach (string key in newConfig.Keys)
                {
                    tracker.configuration.parameters[key] = newConfig[key];
                }
            }
            else
            {
                tracker.configuration.parameters[key] = value;
            }

            if (callback != null)
            {
                callback.TrackerReady();
            }
        }
    }

    #endregion
}
