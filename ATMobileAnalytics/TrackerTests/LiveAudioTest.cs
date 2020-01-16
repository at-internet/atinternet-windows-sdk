using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class LiveAudioTest : AbstractTest
    {
        LiveAudio la;
        LiveAudios las;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            la = new LiveAudio(mp);
            las = new LiveAudios(mp);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(-1, la.Level2);
            Assert.IsNull(la.WebDomain);
            Assert.AreEqual(string.Empty, la.Name);
            Assert.AreEqual("audio", la.type);
            Assert.AreEqual(RichMediaAction.Play, la.Action);
            Assert.AreEqual(BroadcastMode.Live, la.broadcastMode);
            Assert.IsFalse(la.IsBuffering);
            Assert.IsFalse(la.IsEmbedded);
        }

        [TestMethod]
        public void setEventPlayTest()
        {
            la.Action = RichMediaAction.Play;
            la.Level2 = 8;
            la.Name = "star";
            la.Chapter1 = "sing";
            la.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            la.Action = RichMediaAction.Pause;
            la.Level2 = 8;
            la.Name = "star";
            la.Chapter1 = "sing";
            la.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            la.Action = RichMediaAction.Stop;
            la.Level2 = 8;
            la.Name = "star";
            la.Chapter1 = "sing";
            la.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            la.Action = RichMediaAction.Refresh;
            la.Level2 = 8;
            la.Name = "star";
            la.Chapter1 = "sing";
            la.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            la.Action = RichMediaAction.Move;
            la.Level2 = 8;
            la.Name = "star";
            la.Chapter1 = "sing";
            la.SetEvent();

            int index = 0;

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            las.Add("name");
            Assert.AreEqual(1, las.list.Count);
            Assert.AreEqual("name", las.list[0].Name);
        }

        [TestMethod]
        public void removeTest()
        {
            LiveAudio la1 = las.Add("name");
            LiveAudio la2 = las.Add("toto");
            las.Add("titi");
            Assert.AreEqual(3, las.list.Count);

            las.Remove("name");
            Assert.AreEqual(2, las.list.Count);
            Assert.IsFalse(las.list.Contains(la1));
            Assert.IsTrue(las.list.Contains(la2));
        }

        [TestMethod]
        public void removeAllTest()
        {
            las.Add("name");
            las.Add("toto");
            las.Add("titi");
            Assert.AreEqual(3, las.list.Count);

            las.RemoveAll();
            Assert.AreEqual(0, las.list.Count);
        }
    }
}
