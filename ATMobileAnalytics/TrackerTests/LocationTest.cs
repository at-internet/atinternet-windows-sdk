using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{

    [TestClass]
    public class LocationTest : AbstractTest
    {
        Location loc;
        Locations locs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            loc = new Location(tracker);
            locs = new Locations(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(0.0, loc.Latitude);
            Assert.AreEqual(0.0, loc.Longitude);
        }

        [TestMethod]
        public void setEventTest()
        {
            loc.Latitude = 125.54545;
            loc.Longitude = 98.6546541;
            loc.SetEvent();

            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("gy", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("125.55", tracker.buffer.volatileParameters[0].value());

            Assert.AreEqual("gx", tracker.buffer.volatileParameters[1].key);
            Assert.AreEqual("98.65", tracker.buffer.volatileParameters[1].value());
        }

        [TestMethod]
        public void addTest()
        {
            loc = locs.Add(12.7777, 14.89888);

            Assert.AreEqual(1, tracker.businessObjects.Count);

            Assert.AreEqual(12.7777, (tracker.businessObjects[loc.id] as Location).Latitude);
            Assert.AreEqual(14.89888, (tracker.businessObjects[loc.id] as Location).Longitude);
        }
    }
}
