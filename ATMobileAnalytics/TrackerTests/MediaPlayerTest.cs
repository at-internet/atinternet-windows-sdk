using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace TrackerTests
{
    [TestClass]
    public class MediaPlayerTest : AbstractTest
    {
        MediaPlayers mps;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            mps = new MediaPlayers(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(1, mp.PlayerId);
        }

        [TestMethod]
        public void audiosTest()
        {
            Audios a = mp.Audios;
            Assert.AreEqual(a, mp.Audios);
        }

        [TestMethod]
        public void videosTest()
        {
            Videos v = mp.Videos;
            Assert.AreEqual(v, mp.Videos);
        }

        [TestMethod]
        public void liveAudiosTest()
        {
            LiveAudios la = mp.LiveAudios;
            Assert.AreEqual(la, mp.LiveAudios);
        }

        [TestMethod]
        public void liveVideosTest()
        {
            LiveVideos lv = mp.LiveVideos;
            Assert.AreEqual(lv, mp.LiveVideos);
        }

        [TestMethod]
        public void initPlayersTest()
        {
            Assert.IsNotNull(mps.players);
        }

        [TestMethod]
        public void addDefaultTest()
        {
            Assert.AreEqual(1, mps.Add().PlayerId);
            Assert.AreEqual(2, mps.Add().PlayerId);
            Assert.AreEqual(3, mps.Add().PlayerId);

            Assert.AreEqual(3, mps.players.Count);
            Assert.IsNotNull(mps.players[1]);
            Assert.IsNotNull(mps.players[2]);
            Assert.IsNotNull(mps.players[2]);
            Assert.IsFalse(mps.players.ContainsKey(4));
        }

        [TestMethod]
        public void addTest()
        {
            Assert.AreEqual(15, mps.Add(15).PlayerId);
            Assert.AreEqual(16, mps.Add().PlayerId);
            Assert.AreEqual(15, mps.Add(15).PlayerId);

            Assert.AreEqual(2, mps.players.Count);
            Assert.IsNotNull(mps.players[15]);
            Assert.IsNotNull(mps.players[16]);
            Assert.IsFalse(mps.players.ContainsKey(4));
        }

        [TestMethod]
        public void removeTest()
        {
            Assert.AreEqual(1, mps.Add().PlayerId);
            Assert.AreEqual(2, mps.Add().PlayerId);
            Assert.AreEqual(3, mps.Add().PlayerId);

            Assert.AreEqual(3, mps.players.Count);

            mps.Remove(2);
            Assert.AreEqual(2, mps.players.Count);
            Assert.IsFalse(mps.players.ContainsKey(2));

            mps.Remove(2);
            Assert.AreEqual(2, mps.players.Count);
        }

        [TestMethod]
        public void removeAllTest()
        {
            Assert.AreEqual(1, mps.Add().PlayerId);
            Assert.AreEqual(2, mps.Add().PlayerId);
            Assert.AreEqual(3, mps.Add().PlayerId);

            Assert.AreEqual(3, mps.players.Count);

            mps.RemoveAll();
            Assert.AreEqual(0, mps.players.Count);
            Assert.IsFalse(mps.players.ContainsKey(1));
        }
    }
}
