using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace TrackerTests
{
    [TestClass]
    public class TechnicalContextTest
    {
        [TestMethod]
        public void vtagTest()
        {
            Assert.IsNotNull(TechnicalContext.TagVersion);
            Assert.AreEqual("1.3.0", TechnicalContext.TagVersion());
        }

        [TestMethod]
        public void ptagTest()
        {
            Assert.IsNotNull(TechnicalContext.TagPlatform);
            Assert.AreEqual("Windows", TechnicalContext.TagPlatform());
        }

        [TestMethod]
        public void localHourTest()
        {
            Assert.IsNotNull(TechnicalContext.LocalHour);
            Assert.AreNotEqual(string.Empty, TechnicalContext.LocalHour());
        }

        [TestMethod]
        public void languageTest()
        {
            Assert.IsNotNull(TechnicalContext.Language);
            Assert.AreNotEqual(string.Empty, TechnicalContext.Language());
        }

        [TestMethod]
        public void connectionTest()
        {
            Assert.IsNotNull(TechnicalContext.ConnectionType);
            Assert.AreNotEqual(string.Empty, TechnicalContext.ConnectionType());
        }

        [TestMethod]
        public void deviceTest()
        {
            Assert.IsNotNull(TechnicalContext.Device);
            Assert.AreNotEqual(string.Empty, TechnicalContext.Device());
        }

        [TestMethod]
        public void manufacturerTest()
        {
            Assert.IsNotNull(TechnicalContext.Manufacturer);
            //Assert.AreNotEqual(string.Empty, TechnicalContext.Manufacturer());
        }

        [TestMethod]
        public void modelTest()
        {
            Assert.IsNotNull(TechnicalContext.Model);
            //Assert.AreNotEqual(string.Empty, TechnicalContext.Model());
        }

        [TestMethod]
        public void osTest()
        {
            Assert.IsNotNull(TechnicalContext.OS);
            Assert.AreNotEqual(string.Empty, TechnicalContext.OS());
        }

        [TestMethod]
        public void apvrTest()
        {
            Assert.IsNotNull(TechnicalContext.Apvr);
            Assert.AreNotEqual(string.Empty, TechnicalContext.Apvr());
        }

        [TestMethod]
        public void apidTest()
        {
            Assert.IsNotNull(TechnicalContext.ApplicationId);
            Assert.AreNotEqual(string.Empty, TechnicalContext.ApplicationId());
        }

        [TestMethod]
        public void guidTest()
        {
            string id = Guid.NewGuid().ToString();
            Tracker.LocalSettings.Values["ATIdClient"] = id;
            Assert.IsNotNull(id);
            Assert.AreEqual(id, TechnicalContext.UserId("what"));
        }

        [TestMethod]

        public void deviceIdTest()
        {
            string id = new EasClientDeviceInformation().Id.ToString();

            Assert.IsNotNull(id);
            Assert.AreEqual(id, TechnicalContext.UserId("deviceId"));
        }
    }
}
