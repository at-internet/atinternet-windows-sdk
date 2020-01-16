using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ATInternet;

namespace TrackerTests
{
    [TestClass]
    public class IdentifiedVisitorTest : AbstractTest
    {
        IdentifiedVisitor iv;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            tracker.buffer.persistentParameters.Clear();
            iv = new IdentifiedVisitor(tracker);
        }

        [TestMethod]
        public void setWithIntTest()
        {
            iv.Set(45);
            Assert.AreEqual("an", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("45", tracker.buffer.persistentParameters[0].value());
        }

        [TestMethod]
        public void setWithIntAndCategoryTest()
        {
            iv.Set(45, 5);
            Assert.AreEqual("an", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("45", tracker.buffer.persistentParameters[0].value());

            Assert.AreEqual("ac", tracker.buffer.persistentParameters[1].key);
            Assert.AreEqual("5", tracker.buffer.persistentParameters[1].value());
        }

        [TestMethod]
        public void setWithStringTest()
        {
            iv.Set("toto45");
            Assert.AreEqual("at", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("toto45", tracker.buffer.persistentParameters[0].value());
        }

        [TestMethod]
        public void setWithStringAndCategoryTest()
        {
            iv.Set("toto45", 5);
            Assert.AreEqual("at", tracker.buffer.persistentParameters[0].key);
            Assert.AreEqual("toto45", tracker.buffer.persistentParameters[0].value());

            Assert.AreEqual("ac", tracker.buffer.persistentParameters[1].key);
            Assert.AreEqual("5", tracker.buffer.persistentParameters[1].value());
        }
    }
}
