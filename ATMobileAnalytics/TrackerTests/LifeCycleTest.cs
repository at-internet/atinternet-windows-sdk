using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.Data.Json;

namespace TrackerTests
{
    [TestClass]
    public class LifeCycleTest : AbstractTest
    {
        DateTime now = DateTime.Now;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            Tracker.LocalSettings.Values.Clear();
        }

        [TestMethod]
        public void firstLaunchInitTest()
        {
            LifeCycle.FirstSessionInit();
            Assert.IsTrue(Tracker.LocalSettings.Values[LifeCycle.FIRST_SESSION] as bool? ?? false);
            Assert.IsFalse(Tracker.LocalSettings.Values[LifeCycle.FIRST_SESSION_AFTER_UPDATE] as bool? ?? true);
            Assert.AreEqual(now.ToString("yyyyMMdd"), Tracker.LocalSettings.Values[LifeCycle.FIRST_SESSION_DATE]);

            Assert.AreEqual(1, Tracker.LocalSettings.Values[LifeCycle.SESSION_COUNT]);
            Assert.AreEqual(1, Tracker.LocalSettings.Values[LifeCycle.SESSION_COUNT_SINCE_UPDATE]);
            Assert.AreEqual(0, Tracker.LocalSettings.Values[LifeCycle.DAYS_SINCE_FIRST_SESSION]);
            Assert.AreEqual(0, Tracker.LocalSettings.Values[LifeCycle.DAYS_SINCE_LAST_SESSION]);
            Assert.AreEqual(TechnicalContext.Apvr(), Tracker.LocalSettings.Values[LifeCycle.LAST_APPLICATION_VERSION]);
        }

        [TestMethod]
        public void newLaunchInitTest()
        {
            LifeCycle.FirstSessionInit();
            JsonObject obj = Tool.ParseJSON(LifeCycle.GetMetrics()());
            JsonObject life = Tool.ParseJSON(obj["lifecycle"].ToString());
            string sesssionId = life["sessionId"].ToString();
            LifeCycle.NewLaunchInit();
            obj = Tool.ParseJSON(LifeCycle.GetMetrics()());
            life = Tool.ParseJSON(obj["lifecycle"].ToString());
            Assert.AreEqual(2, Tracker.LocalSettings.Values[LifeCycle.SESSION_COUNT]);
            Assert.AreEqual(2, Tracker.LocalSettings.Values[LifeCycle.SESSION_COUNT_SINCE_UPDATE]);
            Assert.AreNotEqual(sesssionId, life["sessionId"].ToString());
        }

        [TestMethod]
        public void firstLaunchFirstHitTest()
        {
            LifeCycle.FirstSessionInit();
            JsonObject obj = Tool.ParseJSON(LifeCycle.GetMetrics()());
            JsonObject life = Tool.ParseJSON(obj["lifecycle"].ToString());
            Assert.AreEqual(1, life["fs"].GetNumber());
            Assert.AreEqual(0, life["fsau"].GetNumber());
            Assert.AreEqual(1, life["sc"].GetNumber());
            Assert.AreEqual(int.Parse(now.ToString("yyyyMMdd")), life["fsd"].GetNumber());
            Assert.AreEqual(0, life["dsls"].GetNumber());
            Assert.AreEqual(0, life["dsfs"].GetNumber());

            Assert.IsFalse(life.ContainsKey("dsu"));
            Assert.IsFalse(life.ContainsKey("scsu"));
            Assert.IsFalse(life.ContainsKey("fsdau"));
        }

        [TestMethod]
        public void firstLaunchAfterUpdateFirstHitTest()
        {
            LifeCycle.FirstSessionInit();
            Tracker.LocalSettings.Values[LifeCycle.LAST_APPLICATION_VERSION] = "toto";
            LifeCycle.IsInitialized = false;
            LifeCycle.updateFirstLaunch();
            LifeCycle.NewLaunchInit();
            
            JsonObject obj = Tool.ParseJSON(LifeCycle.GetMetrics()());
            JsonObject life = Tool.ParseJSON(obj["lifecycle"].ToString());

            Assert.AreEqual(0, life["fs"].GetNumber());
            Assert.AreEqual(1, life["fsau"].GetNumber());
            Assert.AreEqual(2, life["sc"].GetNumber());
            Assert.AreEqual(int.Parse(now.ToString("yyyyMMdd")), life["fsd"].GetNumber());
            Assert.AreEqual(0, life["dsls"].GetNumber());
            Assert.AreEqual(0, life["dsfs"].GetNumber());
            Assert.AreEqual(0, life["dsu"].GetNumber());
            Assert.AreEqual(1, life["scsu"].GetNumber());
            Assert.AreEqual(int.Parse(now.ToString("yyyyMMdd")), life["fsdau"].GetNumber());
        }
    }
}
