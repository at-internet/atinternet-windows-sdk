using System;

namespace ATInternet
{
    #region Campaign
    public class Campaign : ScreenInfo
    {
        #region Members

        public string campaignId { get; set; }

        #endregion

        #region Constructor

        internal Campaign(Tracker tracker) : base(tracker) { }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            ParamOption encoding = new ParamOption() { Encode = true };
            tracker.SetParam("xto", campaignId, encoding);
            Tracker.LocalSettings.Values["ATCampaignAdded"] = true;

            string remanentCampaign = null;
            if (Tracker.LocalSettings.Values.ContainsKey("ATMarketingCampaignSaved"))
            {
                object mrktCampaignSaved = Tracker.LocalSettings.Values["ATMarketingCampaignSaved"];
                remanentCampaign = mrktCampaignSaved != null ? mrktCampaignSaved.ToString() : null;
            }

            DateTimeOffset campaignDate;
            if (Tracker.LocalSettings.Values.ContainsKey("ATLastMarketingCampaignDate"))
            {
                object lastCampaignDate = Tracker.LocalSettings.Values["ATLastMarketingCampaignDate"];
                campaignDate = lastCampaignDate != null ? DateTimeOffset.ParseExact(lastCampaignDate.ToString(), "yyyyMMdd", null) : DateTimeOffset.Now;
            }

            if (remanentCampaign != null)
            {
                if ((DateTimeOffset.Now - campaignDate).TotalDays > int.Parse(tracker.configuration.parameters["campaignLifetime"]))
                {
                    Tracker.LocalSettings.Values["ATMarketingCampaignSaved"] = null;
                    remanentCampaign = null;
                }
                else
                {
                    tracker.SetParam("xtor", remanentCampaign, encoding);
                }
            }

            if (bool.Parse(tracker.configuration.parameters["campaignLastPersistence"]) || remanentCampaign == null)
            {
                Tracker.LocalSettings.Values["ATMarketingCampaignSaved"] = campaignId;
                Tracker.LocalSettings.Values["ATLastMarketingCampaignDate"] = DateTimeOffset.Now.ToString("yyyyMMdd");
            }
        }

        #endregion
    }

    #endregion

    #region Campaigns

    public class Campaigns
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Campaigns(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public Campaign Add(string campaignId)
        {
            Campaign cp = new Campaign(tracker);
            cp.campaignId = campaignId;
            tracker.businessObjects.Add(cp.id, cp);
            tracker.objectIndex++;

            return cp;
        }

        #endregion
    }

    #endregion
}
