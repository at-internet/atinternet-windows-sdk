using System.Collections.Generic;

namespace ATInternet
{
    #region OnAppAdAction

    public enum OnAppAdAction
    {
        View, Touch
    }

    #endregion

    #region OnAppAd
    public class OnAppAd : BusinessObject
    {

        public OnAppAdAction Action { get; set; }

        internal OnAppAd(Tracker tracker) : base(tracker) { Action = OnAppAdAction.View; }

        internal override void SetEvent()
        {
            List<int[]> indexes = Tool.FindParameterPositions("type", tracker.buffer.volatileParameters, tracker.buffer.persistentParameters);
            string currentType = "";

            foreach (int[] index in indexes)
            {
                int arrayID = index[0];
                int itemPosition = index[1];
                if (arrayID == 0)
                {
                    currentType = tracker.buffer.volatileParameters[itemPosition].value();
                }
                else
                {
                    currentType = tracker.buffer.persistentParameters[itemPosition].value();
                }
            }
            if (currentType != "screen" && currentType != "AT")
            {
                tracker.SetParam("type", "AT");
            }

            if (Action == OnAppAdAction.Touch)
            {
                if (!string.IsNullOrEmpty(TechnicalContext.ScreenName))
                {
                    tracker.SetParam("patc", TechnicalContext.ScreenName, new ParamOption() { Encode = true });
                }

                if (TechnicalContext.Level2 > 0)
                {
                    tracker.SetParam("s2atc", TechnicalContext.Level2);
                }
            }
        }
    }
    #endregion

    #region Publisher

    public class Publisher : OnAppAd
    {
        #region Members

        public string CampaignId { get; set; }

        public string Creation { get; set; }

        public string Variant { get; set; }

        public string Format { get; set; }

        public string GeneralPlacement { get; set; }

        public string DetailedPlacement { get; set; }

        public string AdvertiserId { get; set; }

        public string Url { get; set; }

        #endregion

        #region Constructor
        internal Publisher(Tracker tracker) : base(tracker)
        {
            CampaignId = string.Empty;
        }
        #endregion

        #region Methods

        public void SendImpression()
        {
            Action = OnAppAdAction.View;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendTouch()
        {
            Action = OnAppAdAction.Touch;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            base.SetEvent();
            string pub = string.Format(
                "PUB-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}",
                CampaignId,
                Creation ?? string.Empty,
                Variant ?? string.Empty,
                Format ?? string.Empty,
                GeneralPlacement ?? string.Empty,
                DetailedPlacement ?? string.Empty,
                AdvertiserId ?? string.Empty,
                Url ?? string.Empty);

            tracker.SetParam(Action == OnAppAdAction.View ? "ati" : "atc", pub, new ParamOption() { Append = true, Encode = true});
        }

        #endregion
    }

    #endregion

    #region SelfPromotion

    public class SelfPromotion : OnAppAd
    {
        #region Members

        public int AdId { get; set; }

        public string ProductId { get; set; }

        public string Format { get; set; }

        #endregion

        #region Constructor
        internal SelfPromotion(Tracker tracker) : base(tracker)
        {
            AdId = 0;
        }
        #endregion

        #region Methods

        public void SendImpression()
        {
            Action = OnAppAdAction.View;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendTouch()
        {
            Action = OnAppAdAction.Touch;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            base.SetEvent();
            string selfP = string.Format(
                "INT-{0}-{1}||{2}",
                AdId,
                Format ?? string.Empty,
                ProductId ?? string.Empty);

            tracker.SetParam(Action == OnAppAdAction.View ? "ati" : "atc", selfP, new ParamOption() { Append = true, Encode = true });
        }

        #endregion
    }

    #endregion

    #region Publishers

    public class Publishers
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Publishers(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public Publisher Add(string campaignId)
        {
            Publisher pub = new Publisher(tracker);
            pub.CampaignId = campaignId;
            tracker.businessObjects.Add(pub.id, pub);
            tracker.objectIndex++;

            return pub;
        }

        public void SendImpressions()
        {
            List<BusinessObject> impressions = new List<BusinessObject>();

            foreach (string key in tracker.businessObjects.Keys)
            {
                BusinessObject obj = tracker.businessObjects[key];
                if(obj is Publisher && (obj as Publisher).Action == OnAppAdAction.View)
                {
                    impressions.Add(obj);
                }
            }

            tracker.dispatcher.Dispatch(impressions.ToArray());
        }

        #endregion
    }

    #endregion

    #region SelfPromotions

    public class SelfPromotions
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal SelfPromotions(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public SelfPromotion Add(int adId)
        {
            SelfPromotion selfP = new SelfPromotion(tracker);
            selfP.AdId = adId;
            tracker.businessObjects.Add(selfP.id, selfP);
            tracker.objectIndex++;

            return selfP;
        }

        public void SendImpressions()
        {
            List<BusinessObject> impressions = new List<BusinessObject>();

            foreach (string key in tracker.businessObjects.Keys)
            {
                BusinessObject obj = tracker.businessObjects[key];
                if (obj is SelfPromotion && (obj as SelfPromotion).Action == OnAppAdAction.View)
                {
                    impressions.Add(obj);
                }
            }

            tracker.dispatcher.Dispatch(impressions.ToArray());
        }

        #endregion
    }

    #endregion
}
