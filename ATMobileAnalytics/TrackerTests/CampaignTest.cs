using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class CampaignTest : AbstractTest
    {
        Campaign cp;
        Campaigns cps;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            cp = new Campaign(tracker);
            cps = new Campaigns(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.IsNull(cp.campaignId);
        }

        [TestMethod]
        public void setEventTest()
        {
            cp.campaignId = "campaignID";
            cp.SetEvent();

            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("xto", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("campaignID", tracker.buffer.volatileParameters[0].value());
            Assert.AreEqual("campaignID", Tracker.LocalSettings.Values["ATMarketingCampaignSaved"].ToString());

            tracker.buffer.volatileParameters.Clear();
            cp.campaignId = "campaignID2";
            cp.SetEvent();

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("xto", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("campaignID2", tracker.buffer.volatileParameters[0].value());
            Assert.AreEqual("xtor", tracker.buffer.volatileParameters[1].key);
            Assert.AreEqual("campaignID", tracker.buffer.volatileParameters[1].value());
        }
    }
}
