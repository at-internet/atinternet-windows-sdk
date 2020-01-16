using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class LiveVideoTest : AbstractTest
    {
        LiveVideo lv;
        LiveVideos lvs;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            lv = new LiveVideo(mp);
            lvs = new LiveVideos(mp);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(-1, lv.Level2);
            Assert.IsNull(lv.WebDomain);
            Assert.AreEqual(string.Empty, lv.Name);
            Assert.AreEqual("video", lv.type);
            Assert.AreEqual(RichMediaAction.Play, lv.Action);
            Assert.AreEqual(BroadcastMode.Live, lv.broadcastMode);
            Assert.IsFalse(lv.IsBuffering);
            Assert.IsFalse(lv.IsEmbedded);
        }

        [TestMethod]
        public void setEventPlayTest()
        {
            lv.Action = RichMediaAction.Play;
            lv.Level2 = 8;
            lv.Name = "star";
            lv.Chapter1 = "sing";
            lv.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("play", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("live", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventPauseTest()
        {
            lv.Action = RichMediaAction.Pause;
            lv.Level2 = 8;
            lv.Name = "star";
            lv.Chapter1 = "sing";
            lv.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("pause", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("live", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventStopTest()
        {
            lv.Action = RichMediaAction.Stop;
            lv.Level2 = 8;
            lv.Name = "star";
            lv.Chapter1 = "sing";
            lv.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("stop", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("live", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventRefreshTest()
        {
            lv.Action = RichMediaAction.Refresh;
            lv.Level2 = 8;
            lv.Name = "star";
            lv.Chapter1 = "sing";
            lv.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("refresh", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("live", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
        }
        [TestMethod]
        public void setEventMoveTest()
        {
            lv.Action = RichMediaAction.Move;
            lv.Level2 = 8;
            lv.Name = "star";
            lv.Chapter1 = "sing";
            lv.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("move", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("live", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void addTest()
        {
            lvs.Add("name");
            Assert.AreEqual(1, lvs.list.Count);
            Assert.AreEqual("name", lvs.list[0].Name);
        }

        [TestMethod]
        public void removeTest()
        {
            LiveVideo lv1 = lvs.Add("name");
            LiveVideo lv2 = lvs.Add("toto");
            lvs.Add("titi");
            Assert.AreEqual(3, lvs.list.Count);

            lvs.Remove("name");
            Assert.AreEqual(2, lvs.list.Count);
            Assert.IsFalse(lvs.list.Contains(lv1));
            Assert.IsTrue(lvs.list.Contains(lv2));
        }

        [TestMethod]
        public void removeAllTest()
        {
            lvs.Add("name");
            lvs.Add("toto");
            lvs.Add("titi");
            Assert.AreEqual(3, lvs.list.Count);

            lvs.RemoveAll();
            Assert.AreEqual(0, lvs.list.Count);
        }
    }
}
