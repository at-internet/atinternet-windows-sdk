using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class AudioTest : AbstractTest
    {
        Audio audio;
        Audios audios;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            audio = new Audio(mp);
            audios = new Audios(mp);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(-1, audio.Level2);
            Assert.AreEqual(0, audio.Duration);
            Assert.IsNull(audio.WebDomain);
            Assert.AreEqual(string.Empty, audio.Name);
            Assert.AreEqual("audio", audio.type);
            Assert.AreEqual(RichMediaAction.Play, audio.Action);
            Assert.AreEqual(BroadcastMode.Clip, audio.broadcastMode);
            Assert.IsFalse(audio.IsBuffering);
            Assert.IsFalse(audio.IsEmbedded);
        }

        [TestMethod]
        public void setEventPlayTest()
        {
            audio.Duration = 56;
            audio.Action = RichMediaAction.Play;
            audio.Level2 = 8;
            audio.Name = "star";
            audio.Chapter1 = "sing";
            audio.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            audio.Duration = 56;
            audio.Action = RichMediaAction.Pause;
            audio.Level2 = 8;
            audio.Name = "star";
            audio.Chapter1 = "sing";
            audio.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            audio.Duration = 56;
            audio.Action = RichMediaAction.Stop;
            audio.Level2 = 8;
            audio.Name = "star";
            audio.Chapter1 = "sing";
            audio.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            audio.Duration = 56;
            audio.Action = RichMediaAction.Refresh;
            audio.Level2 = 8;
            audio.Name = "star";
            audio.Chapter1 = "sing";
            audio.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            audio.Duration = 56;
            audio.Action = RichMediaAction.Move;
            audio.Level2 = 8;
            audio.Name = "star";
            audio.Chapter1 = "sing";
            audio.SetEvent();

            int index = 0;

            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("audio", tracker.buffer.volatileParameters[index++].value());

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
            audios.Add("name", 59);
            Assert.AreEqual(1, audios.list.Count);
            Assert.AreEqual(59, audios.list[0].Duration);
            Assert.AreEqual("name", audios.list[0].Name);
        }

        [TestMethod]
        public void removeTest()
        {
            Audio a1 = audios.Add("name", 80);
            Audio a2 = audios.Add("toto", 10);
            audios.Add("titi", 40);
            Assert.AreEqual(3, audios.list.Count);

            audios.Remove("name");
            Assert.AreEqual(2, audios.list.Count);
            Assert.IsFalse(audios.list.Contains(a1));
            Assert.IsTrue(audios.list.Contains(a2));
        }

        [TestMethod]
        public void removeAllTest()
        {
            audios.Add("name", 80);
            audios.Add("toto", 10);
            audios.Add("titi", 40);
            Assert.AreEqual(3, audios.list.Count);

            audios.RemoveAll();
            Assert.AreEqual(0, audios.list.Count);
        }
    }
}
