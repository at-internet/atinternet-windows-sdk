using System;
using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{

    #region Offline
    public class Offline
    {
        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        #endregion

        #region Constructor

        internal Offline(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Get the offline hits
        /// </summary>
        /// <returns></returns>
        public IList<Hit> Get()
        {
            return Storage.Instance.GetOfflineHits();
        }

        /// <summary>
        /// Get the offline hits count
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Storage.Instance.GetOfflineHits().Count();
        }

        /// <summary>
        /// Remove all offline hits
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            Storage.Instance.RemoveAllOfflineHits();
        }

        /// <summary>
        /// Remove all offline hits older than date
        /// </summary>
        /// <returns></returns>
        public void Delete(DateTimeOffset time)
        {
            Storage.Instance.RemoveOldOfflineHits(time);
        }

        /// <summary>
        /// Get the oldest offline hit
        /// </summary>
        /// <returns></returns>
        public Hit Oldest()
        {
            return Storage.Instance.GetOldestOfflineHit();
        }

        /// <summary>
        /// Get the oldest offline hit
        /// </summary>
        /// <returns></returns>
        public Hit Latest()
        {
            return Storage.Instance.GetLatestOfflineHit();
        }

        /// <summary>
        /// Send offline hits
        /// </summary>
        /// <returns></returns>
        public void Dispatch()
        {
            Sender.SendOfflineHits(tracker, Storage.Instance, true, true);
        }

        #endregion
    }
    #endregion
}
