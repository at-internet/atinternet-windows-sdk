using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace ATInternet
{
    #region Storage
    internal class Storage
    {
        #region Members

        /// <summary>
        /// Local storage
        /// </summary>
        ApplicationDataContainer container {
            get {
               return ApplicationData.Current.LocalSettings.CreateContainer("ATHitsStorage", ApplicationDataCreateDisposition.Always);
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Singleton storage
        /// </summary>
        static readonly Storage _instance = new Storage();
        internal static Storage Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save the hit
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="oltParam"></param>
        /// <returns></returns>
        internal string SaveHit(string hit, string oltParam)
        {
            hit = BuildHitToStore(hit, oltParam);
            container.Values[container.Values.Count().ToString()] = hit;
            return hit;
        }

        /// <summary>
        /// Formatting the hit to store
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="oltParam"></param>
        /// <returns></returns>
        private string BuildHitToStore(string hit,string oltParam)
        {
            string[] hitComponents = hit.Split('&');
            string newHit = hitComponents[0];

            for (int i = 1; i < hitComponents.Count(); i++)
            {
                string[] parameterComponents = hitComponents[i].Split('=');

                if (parameterComponents[0].Equals("cn"))
                {
                    newHit += "&cn=offline";
                }
                else
                {
                    newHit += "&" + hitComponents[i];
                }

                if (parameterComponents[0].Equals("ts") || parameterComponents[0].Equals("mh"))
                {
                    newHit += "&olt=" + oltParam;
                }
            }
            return newHit;
        }

        /// <summary>
        /// Delete the hit from storage
        /// </summary>
        /// <param name="hit"></param>
        internal void DeleteHit(string hit)
        {
            List<Hit> offlineHits = GetOfflineHits();
            offlineHits.RemoveAll(x => x.Url.Equals(hit));
            container.Values.Clear();
            for(int i = 0; i < offlineHits.Count(); i++)
            {
                container.Values[i.ToString()] = offlineHits[i].Url;
            }
        }

        /// <summary>
        /// Remove offline hits older than storage duration
        /// </summary>
        /// <param name="hit"></param>
        internal void RemoveOldOfflineHits(DateTimeOffset time)
        {
            double resultTime = (time - new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc)).TotalMilliseconds;
            List<Hit> offlineHits = GetOfflineHits();
            offlineHits.RemoveAll(hit => (hit.CreationDate.Millisecond < resultTime));
            container.Values.Clear();
            for (int i = 0; i < offlineHits.Count(); i++)
            {
                container.Values[i.ToString()] = offlineHits[i].Url;
            }
        }

        /// <summary>
        /// Delete all hits from storage
        /// </summary>
        /// <param name="hit"></param>
        internal void RemoveAllOfflineHits()
        {
            container.Values.Clear();
        }

        /// <summary>
        /// Get the offline hits
        /// </summary>
        /// <returns></returns>
        internal List<Hit> GetOfflineHits()
        {
            List<Hit> hits = new List<Hit>();
            for (int i = 0; i < container.Values.Count(); i++)
            {
               hits.Add(new Hit((string)container.Values[i.ToString()]) { IsOffline = true });
            }

            hits.OrderBy(hit => hit.CreationDate.Millisecond);
            return hits;
        }

        /// <summary>
        /// Get the latest hit
        /// </summary>
        /// <returns></returns>
        internal Hit GetLatestOfflineHit()
        {
            List<Hit> offlineHits = GetOfflineHits();
            if (offlineHits.Count() > 0)
            {
                return new Hit(offlineHits.Last().Url) { IsOffline = true };
            }
            return null;
        }

        /// <summary>
        /// Get the oldest hit
        /// </summary>
        /// <returns></returns>
        internal Hit GetOldestOfflineHit()
        {
            List<Hit> offlineHits = GetOfflineHits();
            if (offlineHits.Count() > 0)
            {
                return new Hit(offlineHits.First().Url) { IsOffline = true };
            }
            return null;
        }

        #endregion
    }

    #endregion
}
