using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ATInternet
{
    internal class Sender : Runnable
    {
        #region Constants
        /// <summary>
        /// Number of retry
        /// </summary>
        private const int RETRY_COUNT = 3;

        /// <summary>
        /// Number of retry
        /// </summary>
        private const int TIMEOUT = 15000;
        #endregion

        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        /// <summary>
        /// Storage instance
        /// </summary>
        private Storage storage;

        /// <summary>
        /// Hit
        /// </summary>
        private Hit hit;

        /// <summary>
        /// Olt Param built by the builder
        /// </summary>
        private string oltParameter;

        /// <summary>
        /// Offline hit processing boolean
        /// </summary>
        private static bool OfflineHitProcessing = false;

        /// <summary>
        /// Force offline hits to be sent
        /// </summary>
        private bool forceSendOfflineHits;

        #endregion

        #region Constructor

        internal Sender(Tracker tracker, Hit hit, bool forceSendOfflineHits, string oltParameter = "")
        {
            this.tracker = tracker;
            this.hit = hit;
            this.oltParameter = oltParameter;
            this.storage = Storage.Instance;
            this.forceSendOfflineHits = forceSendOfflineHits;
        }

        #endregion

        #region Methods

        private void Send(Hit hit)
        {
            if (tracker.configuration.parameters["storage"].Equals("always") && !forceSendOfflineHits)
            {
                storage.SaveHit(hit.Url, oltParameter);
                if (tracker.Delegate != null)
                {
                    tracker.Delegate.SaveDidEnd("Hit saved : " + hit.Url);
                }
            }
            else if (Reachability.GetActiveNetworkType() == Reachability.NetworkType.OFFLINE || (!hit.IsOffline && storage.GetOfflineHits().Count() > 0))
            {
                if (!tracker.configuration.parameters["storage"].Equals("never") && !hit.IsOffline)
                {
                    storage.SaveHit(hit.Url, oltParameter);
                    if (tracker.Delegate != null)
                    {
                        tracker.Delegate.SaveDidEnd("Hit saved : " + hit.Url);
                    }
                }
            }
            else
            {
                try
                {
                    HttpClient client = new HttpClient();
                    Uri uri = new Uri(hit.Url);
                    HttpResponseMessage response = client.GetAsync(uri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        if (hit.IsOffline)
                        {
                            storage.DeleteHit(hit.Url);
                        }

                        if(tracker.Delegate != null)
                        {
                            tracker.Delegate.SendDidEnd(HitStatus.Success, hit.Url);
                        }
                    }
                    else
                    {
                        if (!tracker.configuration.parameters["storage"].Equals("never"))
                        {
                            if (!hit.IsOffline)
                            {
                                storage.SaveHit(hit.Url, oltParameter);
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.SaveDidEnd("Hit saved : " + hit.Url);
                                }
                            }
                            else
                            {
                                storage.DeleteHit(hit.Url);
                            }
                        }
                        if (tracker.Delegate != null)
                        {
                            tracker.Delegate.SendDidEnd(HitStatus.Failed, response.ReasonPhrase);
                        }
                    }

                } catch(Exception e)
                {
                    e.ToString();
                    if(!tracker.configuration.parameters["storage"].Equals("never"))
                    {
                        if (!hit.IsOffline)
                        {
                            storage.SaveHit(hit.Url, oltParameter);
                            if (tracker.Delegate != null)
                            {
                                tracker.Delegate.SaveDidEnd("Hit saved : " + hit.Url);
                            }
                        }
                        else
                        {
                            storage.DeleteHit(hit.Url);
                        }
                    }
                    if (tracker.Delegate != null)
                    {
                        tracker.Delegate.SendDidEnd(HitStatus.Failed, "Hit could not be parsed and sent");
                    }
                }
            }
        }

        internal void Send(bool includeOfflineHits)
        {
            if (includeOfflineHits)
            {
                SendOfflineHits(tracker, storage, false, forceSendOfflineHits);
            }
            Send(hit);
        }

        internal static void SendOfflineHits(Tracker tracker, Storage storage, bool forceSendOfflineHits, bool async)
        {
            if((!tracker.configuration.parameters["storage"].Equals("always") || forceSendOfflineHits) && Reachability.GetActiveNetworkType() != Reachability.NetworkType.OFFLINE && !OfflineHitProcessing)
            {
                List<Hit> offlineHits = storage.GetOfflineHits();

                if (TrackerQueue.Instance.Where(task => task is Sender && (task as Sender).hit.IsOffline).Count() == 0 && offlineHits.Count() > 0)
                {
                    if (async)
                    {
                        foreach(Hit hit in offlineHits)
                        {
                            TrackerQueue.Instance.Add(new Sender(tracker, hit, forceSendOfflineHits));
                        }
                    }
                    else
                    {
                        OfflineHitProcessing = true;

                        foreach(Hit hit in offlineHits)
                        {
                            Sender sender = new Sender(tracker, hit, forceSendOfflineHits);
                            sender.Send(false);
                        }

                        OfflineHitProcessing = false;
                    }
                }
            }
        }

        void Runnable.Run()
        {
            Send(false);
        }
        #endregion
    }
}
