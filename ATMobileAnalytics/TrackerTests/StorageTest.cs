using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace TrackerTests
{
    [TestClass]
    public class StorageTest
    {
        Storage storage = Storage.Instance;

        [TestInitialize]
        public void setUp()
        {
            storage.RemoveAllOfflineHits();
        }

        [TestMethod]
        public void saveHitTest()
        {
            Assert.IsTrue(storage.GetOfflineHits().Count == 0);
            storage.SaveHit("toto", "oltParam");
            Assert.IsTrue(storage.GetOfflineHits().Count == 1);
            Assert.IsTrue(storage.GetOfflineHits()[0].Url.Contains("toto"));
        }

        [TestMethod]
        public void deleteTest()
        {
            for (int i = 0; i < 5; i++)
            {
                storage.SaveHit(i.ToString(), "oltParam");
            }
            
            Assert.IsTrue(storage.GetOfflineHits().Count == 5);
            storage.DeleteHit("4");
            Assert.IsTrue(storage.GetOfflineHits().Count == 4);
        }

        [TestMethod]
        public void removeAllOfflineHits()
        {
            for (int i = 0; i < 5; i++)
            {
                storage.SaveHit(i.ToString(), "oltParam");
            }

            Assert.IsTrue(storage.GetOfflineHits().Count == 5);
            storage.RemoveAllOfflineHits();
            Assert.IsTrue(storage.GetOfflineHits().Count == 0);
        }

    }
}
