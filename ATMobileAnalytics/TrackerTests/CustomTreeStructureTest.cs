using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class CustomTreeStructureTest : AbstractTest
    {
        CustomTreeStructure cts;
        CustomTreeStructures ctsS;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            cts = new CustomTreeStructure(tracker);
            ctsS = new CustomTreeStructures(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.IsTrue(cts.Category1 == 0);
            Assert.IsTrue(cts.Category2 == 0);
            Assert.IsTrue(cts.Category3 == 0);
        }

        [TestMethod]
        public void setEventTest()
        {
            cts.Category1 = 4;
            cts.Category3 = 4;
            cts.SetEvent();

            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);
            Assert.AreEqual("ptype", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("4-0-4", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void addTest()
        {
            cts = ctsS.Add(9,4);
            cts.Category3 = 4;

            Assert.AreEqual(1, tracker.businessObjects.Count);

            Assert.AreEqual(9, (tracker.businessObjects[cts.id] as CustomTreeStructure).Category1);
            Assert.AreEqual(4, (tracker.businessObjects[cts.id] as CustomTreeStructure).Category2);
            Assert.AreEqual(4, (tracker.businessObjects[cts.id] as CustomTreeStructure).Category3);
        }
    }
}
