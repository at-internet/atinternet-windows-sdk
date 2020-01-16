using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Windows.Security.ExchangeActiveSyncProvisioning;
using System;
using ATInternet;

namespace TrackerTests
{
    [TestClass]
    public class TrackerTest : AbstractTest
    {
        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            tracker.buffer.volatileParameters.Clear();
            tracker.buffer.persistentParameters.Clear();
        }

        [TestMethod]
        public void multiInstanceTest()
        {
            Assert.AreNotEqual(tracker, new Tracker(new Dictionary<string, string>()));
        }

        [TestMethod]
        public void stringifyEnumTest()
        {
            Assert.AreEqual("deviceId", IdentifierType.deviceId.ToString());
            Assert.AreEqual("guid", IdentifierType.guid.ToString());

            Assert.AreEqual("always", OfflineMode.always.ToString());
            Assert.AreEqual("required", OfflineMode.required.ToString());
            Assert.AreEqual("never", OfflineMode.never.ToString());
        }

        [TestMethod]
        public void offlineTest()
        {
            Offline offline = tracker.Offline;
            Assert.AreEqual(offline, tracker.Offline);
        }

        [TestMethod]
        public void contextTest()
        {
            Context context = tracker.Context;
            Assert.AreEqual(context, tracker.Context);
        }

        [TestMethod]
        public void publishersTest()
        {
            Publishers pubs = tracker.Publishers;
            Assert.AreEqual(pubs, tracker.Publishers);
        }

        [TestMethod]
        public void selfPromotionsTest()
        {
            SelfPromotions selfps = tracker.SelfPromotions;
            Assert.AreEqual(selfps, tracker.SelfPromotions);
        }

        [TestMethod]
        public void customObjectsTest()
        {
            CustomObjects cos = tracker.CustomObjects;
            Assert.AreEqual(cos, tracker.CustomObjects);
        }

        [TestMethod]
        public void customTreeStructuresTest()
        {
            CustomTreeStructures cts = tracker.CustomTreeStructures;
            Assert.AreEqual(cts, tracker.customTreeStructures);
            Assert.AreEqual(cts, tracker.CustomTreeStructures);
        }

        [TestMethod]
        public void customVarsTest()
        {
            CustomVars cvs = tracker.CustomVars;
            Assert.AreEqual(cvs, tracker.CustomVars);
        }

        [TestMethod]
        public void gesturesTest()
        {
            Gestures gest = tracker.Gestures;
            Assert.AreEqual(gest, tracker.Gestures);
        }

        [TestMethod]
        public void identifiedVisitorTest()
        {
            IdentifiedVisitor iv = tracker.IdentifiedVisitor;
            Assert.AreEqual(iv, tracker.IdentifiedVisitor);
        }

        [TestMethod]
        public void internalSearchesTest()
        {
            InternalSearches iss = tracker.InternalSearches;
            Assert.AreEqual(iss, tracker.InternalSearches);
        }

        [TestMethod]
        public void locationsTest()
        {
            Locations locs = tracker.Locations;
            Assert.AreEqual(locs, tracker.Locations);
        }

        [TestMethod]
        public void productsTest()
        {
            Products ps = tracker.Products;
            Assert.AreEqual(ps, tracker.Products);
        }

        [TestMethod]
        public void cartTest()
        {
            Cart cart = tracker.Cart;
            Assert.AreEqual(cart, tracker.Cart);
        }

        [TestMethod]
        public void aislesTest()
        {
            Aisles aisles = tracker.Aisles;
            Assert.AreEqual(aisles, tracker.Aisles);
        }

        [TestMethod]
        public void ordersTest()
        {
            Orders orders = tracker.Orders;
            Assert.AreEqual(orders, tracker.Orders);
        }

        [TestMethod]
        public void campaignsTest()
        {
            Campaigns campaigns = tracker.Campaigns;
            Assert.AreEqual(campaigns, tracker.Campaigns);
        }

        [TestMethod]
        public void playersTest()
        {
            MediaPlayers mp = tracker.MediaPlayers;
            Assert.AreEqual(mp, tracker.MediaPlayers);
        }

        [TestMethod]
        public void setParamIntTest()
        {
            tracker.SetParam("int", 56);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamFloatTest()
        {
            tracker.SetParam("float", (float)56.6);
            Assert.AreEqual("56.6", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamDoubleTest()
        {
            tracker.SetParam("double", 56.68);
            Assert.AreEqual("56.68", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamStringTest()
        {
            tracker.SetParam("str", "test");
            Assert.AreEqual("test", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamListTest()
        {
            tracker.SetParam("list", new List<object>() { 4, true, "okay" });
            Assert.AreEqual("4,True,okay", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamDicoTest()
        {
            tracker.SetParam("dico", new Dictionary<string, object>() { { "key1", "value1" } });
            Assert.AreEqual("{\"key1\":\"value1\"}", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void setParamPersistentListTest()
        {
            tracker.SetParam("list", new List<object>() { 4, true, "okay" }, new ParamOption() { Persistent = true });
            Assert.AreEqual("4,True,okay", tracker.buffer.persistentParameters[0].value());
        }

        [TestMethod]
        public void setParamListSeparatorTest()
        {
            tracker.SetParam("list", new List<object>() { 4, true, "okay" }, new ParamOption() { Separator = "-" });
            Assert.AreEqual("4-True-okay", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void unsetTest()
        {
            tracker.SetParam("list", new List<object>() { 4, true, "okay" }, new ParamOption() { Separator = "-" });
            tracker.SetParam("toto", "test");
            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            tracker.UnsetParam("list");
            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);

        }

        [TestMethod]
        public void getUserGUID()
        {
            tracker.configuration.parameters["identifier"] = "what";
            string id = Guid.NewGuid().ToString();
            Tracker.LocalSettings.Values["ATIdClient"] = id;

            Assert.AreEqual(id, tracker.GetUserId());
            tracker.configuration.parameters["hashUserId"] = "true";
            Assert.AreEqual(Tool.SHA_256(id), tracker.GetUserId());
        }

        [TestMethod]
        public void getUserDeviceId()
        {
            string id = new EasClientDeviceInformation().Id.ToString();

            Assert.AreEqual(id, tracker.GetUserId());
            tracker.configuration.parameters["hashUserId"] = "true";
            Assert.AreEqual(Tool.SHA_256(id), tracker.GetUserId());
        }

        [TestMethod]
        public void doNotTrackTest()
        {
            tracker.configuration.parameters["identifier"] = "what";
            Tracker.DoNotTrack = true;
            string id = Guid.NewGuid().ToString();
            Tracker.LocalSettings.Values["ATIdClient"] = id;

            Assert.AreEqual("opt-out", tracker.GetUserId());
            tracker.configuration.parameters["hashUserId"] = "true";
            Assert.AreEqual("opt-out", tracker.GetUserId());

        }
    }
}
