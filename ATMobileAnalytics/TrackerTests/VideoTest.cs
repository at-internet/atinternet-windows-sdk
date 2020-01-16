using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class VideoTest : AbstractTest
    {
        Video video;
        Videos videos;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            video = new Video(mp);
            videos = new Videos(mp);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(-1, video.Level2);
            Assert.AreEqual(0, video.Duration);
            Assert.IsNull(video.WebDomain);
            Assert.AreEqual(string.Empty, video.Name);
            Assert.AreEqual("video", video.type);
            Assert.AreEqual(RichMediaAction.Play, video.Action);
            Assert.AreEqual(BroadcastMode.Clip, video.broadcastMode);
            Assert.IsFalse(video.IsBuffering);
            Assert.IsFalse(video.IsEmbedded);
        }

        [TestMethod]
        public void setEventPlayTest()
        {
            video.Duration = 56;
            video.Action = RichMediaAction.Play;
            video.Level2 = 8;
            video.Name = "star";
            video.Chapter1 = "sing";
            video.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("play", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("clip", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventPauseTest()
        {
            video.Duration = 56;
            video.Action = RichMediaAction.Pause;
            video.Level2 = 8;
            video.Name = "star";
            video.Chapter1 = "sing";
            video.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("pause", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("clip", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventStopTest()
        {
            video.Duration = 56;
            video.Action = RichMediaAction.Stop;
            video.Level2 = 8;
            video.Name = "star";
            video.Chapter1 = "sing";
            video.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("stop", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("clip", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void setEventRefreshTest()
        {
            video.Duration = 56;
            video.Action = RichMediaAction.Refresh;
            video.Level2 = 8;
            video.Name = "star";
            video.Chapter1 = "sing";
            video.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("refresh", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("clip", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[index++].value());
        }
        [TestMethod]
        public void setEventMoveTest()
        {
            video.Duration = 56;
            video.Action = RichMediaAction.Move;
            video.Level2 = 8;
            video.Name = "star";
            video.Chapter1 = "sing";
            video.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("video", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("sing::star", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("a", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("move", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m6", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("clip", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("plyr", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m5", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("int", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("m1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("56", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void addTest()
        {
            videos.Add("name", 59);
            Assert.AreEqual(1, videos.list.Count);
            Assert.AreEqual(59, videos.list[0].Duration);
            Assert.AreEqual("name", videos.list[0].Name);
        }

        [TestMethod]
        public void removeTest()
        {
            Video v1 = videos.Add("name", 80);
            Video v2 = videos.Add("toto", 10);
            videos.Add("titi", 40);
            Assert.AreEqual(3, videos.list.Count);

            videos.Remove("name");
            Assert.AreEqual(2, videos.list.Count);
            Assert.IsFalse(videos.list.Contains(v1));
            Assert.IsTrue(videos.list.Contains(v2));
        }

        [TestMethod]
        public void removeAllTest()
        {
            videos.Add("name", 80);
            videos.Add("toto", 10);
            videos.Add("titi", 40);
            Assert.AreEqual(3, videos.list.Count);

            videos.RemoveAll();
            Assert.AreEqual(0, videos.list.Count);
        }
    }
}
