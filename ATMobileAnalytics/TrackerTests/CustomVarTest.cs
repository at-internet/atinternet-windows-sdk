using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{

    [TestClass]
    public class CustomVarTest : AbstractTest
    {
        CustomVar cv;
        CustomVars cvs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            cv = new CustomVar(tracker);
            cvs = new CustomVars(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(string.Empty, cv.Value);
            Assert.AreEqual(CustomVarType.App, cv.CustomVarType);
        }

        [TestMethod]
        public void setEventTest()
        {
            cv.Value = "test";
            cv.SetEvent();

            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("x1", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("test", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void addTest()
        {
            cv = cvs.Add(6, "toto", CustomVarType.Screen);

            Assert.AreEqual(1, tracker.businessObjects.Count);

            Assert.AreEqual(6, (tracker.businessObjects[cv.id] as CustomVar).VarId);
            Assert.AreEqual("toto", (tracker.businessObjects[cv.id] as CustomVar).Value);
        }
    }
}
