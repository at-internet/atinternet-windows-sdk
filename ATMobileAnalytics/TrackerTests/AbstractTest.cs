using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace TrackerTests
{
    [TestClass]
    public abstract class AbstractTest
    {
        protected Tracker tracker;
        protected MediaPlayer mp;

        [TestInitialize]
        public void setUp()
        {
            tracker = new Tracker(new Dictionary<string, string>() {
                {"secure","false" },
                {"log","logp" },
                {"logSSL","logs" },
                {"site","564360" },
                { "domain", "xiti.com"},
                { "pixelPath", "/hit.xiti"},
                { "plugins", ""},
                { "identifier", "deviceId"},
                { "hashUserId", "false"},
                { "persistIdentifiedVisitor", "false"},
                { "campaignLastPersistence", "true"},
                { "campaignLifetime", "30"},
                { "storage", "always"}

            });
            mp = new MediaPlayer(tracker);
        }
    }
}
