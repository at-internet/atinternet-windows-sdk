using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class GestureTest : AbstractTest
    {
        Gesture g;
        Gestures gs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            g = new Gesture(tracker);
            gs = new Gestures(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(string.Empty, g.Name);
            Assert.AreEqual(GestureAction.Touch, g.Action);
            Assert.AreEqual(0, g.Level2);
        }

        [TestMethod]
        public void setEventTest()
        {
            g.Name = "test";
            g.Level2 = 9;
            g.Action = GestureAction.Navigate;
            g.SetEvent();

            int index = 0;

            Assert.AreEqual(6, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("9", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("click", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("N", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("click", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("action", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("N", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("test", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("stc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("{}", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void addTest()
        {
            g = gs.Add();

            Assert.AreEqual(1, tracker.businessObjects.Count);

            Assert.AreEqual(string.Empty, (tracker.businessObjects[g.id] as Gesture).Name);
        }
    }
}
