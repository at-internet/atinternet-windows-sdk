using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace TrackerTests
{
    [TestClass]
    public class ToolTest
    {
        [TestMethod]
        public void SHA256Test()
        {
            Assert.AreEqual("8652e6fddc89d1392129e8f5ade37e4288406503e5b73bad51619d6e4f3ce50c", Tool.SHA_256("test"));
        }

        [TestMethod]
        public void isValidJSONTest()
        {
            Assert.IsTrue(Tool.IsValidJSON("{\"key1\": \"value1\"}"));
        }

        [TestMethod]
        public void getTSFromHitTest()
        {
            string ts = TechnicalContext.Timestamp();
            string hit = "http://logp.xiti.com/hit.xiti?s=552987&cn=3g&ts=" + ts;
            Assert.AreEqual(ts, Tool.GetTimeStampFromHit(hit));
        }
    }
}
