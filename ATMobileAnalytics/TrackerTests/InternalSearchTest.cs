using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class InternalSearchTest : AbstractTest
    {
        InternalSearch internalS;
        InternalSearches iss;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            internalS = new InternalSearch(tracker);
            iss = new InternalSearches(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.IsNull(internalS.KeywordLabel);
            Assert.AreEqual(1, internalS.ResultScreenNumber);
            Assert.AreEqual(-1, internalS.ResultPosition);
        }

        [TestMethod]
        public void setEventTest()
        {
            internalS.KeywordLabel = "t)o..tù o_^007";
            internalS.ResultScreenNumber = 5;
            internalS.ResultPosition = 1;
            internalS.SetEvent();

            Assert.AreEqual(3, tracker.buffer.volatileParameters.Count);
            Assert.AreEqual("mc", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("toto007", tracker.buffer.volatileParameters[0].value());

            Assert.AreEqual("np", tracker.buffer.volatileParameters[1].key);
            Assert.AreEqual("5", tracker.buffer.volatileParameters[1].value());

            Assert.AreEqual("mcrg", tracker.buffer.volatileParameters[2].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[2].value());

        }

        [TestMethod]
        public void addTest()
        {
            internalS = iss.Add("toto", 1, 2);

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("toto", (tracker.businessObjects[internalS.id] as InternalSearch).KeywordLabel);
            Assert.AreEqual(1, (tracker.businessObjects[internalS.id] as InternalSearch).ResultScreenNumber);
            Assert.AreEqual(2, (tracker.businessObjects[internalS.id] as InternalSearch).ResultPosition);
        }
    }
}
