using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class AisleTest : AbstractTest
    {
        Aisle ai;
        Aisles ais;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            ai = new Aisle(tracker);
            ais = new Aisles(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.IsNull(ai.Level1);
            Assert.IsNull(ai.Level2);
            Assert.IsNull(ai.Level3);
            Assert.IsNull(ai.Level4);
            Assert.IsNull(ai.Level5);
            Assert.IsNull(ai.Level6);
        }

        [TestMethod]
        public void setEventTest()
        {
            ai.Level1 = "1";
            ai.Level2 = "2";
            ai.Level4 = "4";
            ai.Level6 = "6";
            ai.SetEvent();

            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("aisl", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("1::2::4::6", tracker.buffer.volatileParameters[0].value());

        }

        [TestMethod]
        public void addTest()
        {
            ai = ais.Add("toto");

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("toto", (tracker.businessObjects[ai.id] as Aisle).Level1);
        }
    }
}
