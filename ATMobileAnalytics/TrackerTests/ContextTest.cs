using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ATInternet;

namespace TrackerTests
{
    [TestClass]
    public class ContextTest : AbstractTest
    {
        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            tracker.buffer.persistentParameters.Clear();
        }

        [TestMethod]
        public void setBackgroundModeTest()
        {
            tracker.Context.BackgroundMode = BackgroundMode.Task;
            Assert.AreEqual(1, tracker.buffer.persistentParameters.Count);
            Assert.AreEqual("bg", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("task", tracker.buffer.persistentParameters[0].value());
            tracker.Context.BackgroundMode = BackgroundMode.Normal;
            Assert.AreEqual(0, tracker.buffer.persistentParameters.Count);
        }

        [TestMethod]
        public void setLevel2Test()
        {
            tracker.Context.Level2 = 44;
            Assert.AreEqual(1, tracker.buffer.persistentParameters.Count);
            Assert.AreEqual("s2", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("44", tracker.buffer.persistentParameters[0].value());
            tracker.Context.Level2 = 0;
            Assert.AreEqual(0, tracker.buffer.persistentParameters.Count);
        }
    }
}
