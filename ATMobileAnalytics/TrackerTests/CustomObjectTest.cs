using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class CustomObjectTest : AbstractTest
    {
        CustomObject customObj;
        CustomObjects customObjs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            customObj = new CustomObject(tracker);
            customObjs = new CustomObjects(tracker);
        }

        [TestMethod]
        public void setEventTest()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>() { { "toto","titi" } };
            customObj.Value = Tool.ConvertToString(new Dictionary<string, object>() { {"test",dico } });
            customObj.SetEvent();

            Assert.AreEqual(1, tracker.buffer.volatileParameters.Count);
            Assert.AreEqual("stc", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("{\"test\":{\"toto\":\"titi\"}}", tracker.buffer.volatileParameters[0].value());
        }

        [TestMethod]
        public void addTest()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>() { { "toto", "titi" } };
            CustomObject co = customObjs.Add(Tool.ConvertToString(new Dictionary<string, object>() { { "test", dico } }));

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("{\"test\":{\"toto\":\"titi\"}}", (tracker.businessObjects[co.id] as CustomObject).Value);
        }
    }
}
