using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class OnAppAdTest : AbstractTest
    {
        Publisher pub;
        Publisher pub1;
        SelfPromotion selfP;
        SelfPromotion selfP1;
        Publishers pubs;
        SelfPromotions selfPs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            pub = new Publisher(tracker);
            pub1 = new Publisher(tracker);
            selfP = new SelfPromotion(tracker);
            selfP1 = new SelfPromotion(tracker);
            pubs = new Publishers(tracker);
            selfPs = new SelfPromotions(tracker);
        }

        [TestMethod]
        public void initPublisherTest()
        {
            Assert.AreEqual(string.Empty, pub.CampaignId);
            Assert.AreEqual(OnAppAdAction.View, pub.Action);
        }

        [TestMethod]
        public void setEventPubInScreenTest()
        {
            pub.CampaignId = "test";
            pub.SetEvent();
            int index = 0;

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("AT", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("ati", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("PUB-test-------", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventPubTest()
        {
            tracker.SetParam("clic", "toto");
            pub.CampaignId = "test";
            pub.SetEvent();
            int index = 1;

            Assert.AreEqual(3, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("AT", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("ati", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("PUB-test-------", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventFullPubTest()
        {
            pub.CampaignId = "1";
            pub.Creation = "2";
            pub.Variant = "3";
            pub.Format = "4";
            pub.GeneralPlacement = "5";
            pub.DetailedPlacement = "6";
            pub.AdvertiserId = "7";
            pub.Url = "8";
            pub.Action = OnAppAdAction.Touch;
            pub.SetEvent();
            int index = 0;

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("AT", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("atc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("PUB-1-2-3-4-5-6-7-8", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventPubsTest()
        {
            pub.CampaignId = "test";
            pub.SetEvent();
            pub1.SetEvent();
            List<Tuple<Param,string>> paramS = new Builder(tracker).PrepareQuery();

            Assert.AreEqual("ati", paramS.Last().Item1.key);
            Assert.AreEqual("&ati=PUB-test-------,PUB--------", paramS.Last().Item2);
        }

        [TestMethod]
        public void initSelfPromotionTest()
        {
            Assert.AreEqual(0, selfP.AdId);
            Assert.AreEqual(OnAppAdAction.View, selfP.Action);
        }

        [TestMethod]
        public void setEventSelfPInScreenTest()
        {
            selfP.AdId = 20;
            selfP.SetEvent();
            int index = 0;

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("AT", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("ati", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("INT-20-||", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventSelfPTest()
        {
            tracker.SetParam("clic", "toto");
            selfP.AdId = 20;
            selfP.SetEvent();

            Assert.AreEqual(3, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("ati", tracker.buffer.volatileParameters[2].key);
            Assert.AreEqual("INT-20-||", tracker.buffer.volatileParameters[2].value());
        }

        [TestMethod]
        public void setEventFullSelfPTest()
        {
            selfP.AdId = 1;
            selfP.Format = "2";
            selfP.ProductId = "3";
            selfP.Action = OnAppAdAction.Touch;
            selfP.SetEvent();
            int index = 0;

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("AT", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("atc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("INT-1-2||3", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventSelfPsTest()
        {
            selfP.AdId = 2;
            selfP.SetEvent();
            selfP1.SetEvent();
            List<Tuple<Param, string>> paramS = new Builder(tracker).PrepareQuery();

            Assert.AreEqual("ati", paramS.Last().Item1.key);
            Assert.AreEqual("&ati=INT-2-%7C%7C,INT-0-%7C%7C", paramS.Last().Item2);
        }

        [TestMethod]
        public void addPub()
        {
            Publisher pub = pubs.Add("test");
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("test", (tracker.businessObjects[pub.id] as Publisher).CampaignId);
        }

        [TestMethod]
        public void addSelfP()
        {
            SelfPromotion selfP = selfPs.Add(9);
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual(9, (tracker.businessObjects[selfP.id] as SelfPromotion).AdId);
        }
    }
}
