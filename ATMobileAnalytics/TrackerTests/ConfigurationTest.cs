using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace TrackerTests
{
    [TestClass]
    public class ConfigurationTest
    {

        [TestMethod]
        public void DefaultConfigTest()
        {
            Configuration conf = new Configuration();
            Assert.AreEqual(13, conf.parameters.Count);
            Assert.AreEqual("", conf.parameters[TrackerConfigurationKeys.LOG]);
            Assert.AreEqual("", conf.parameters[TrackerConfigurationKeys.LOG_SSL]);
            Assert.AreEqual("", conf.parameters[TrackerConfigurationKeys.SITE]);
            Assert.AreEqual("guid", conf.parameters[TrackerConfigurationKeys.IDENTIFIER]);
            Assert.AreEqual("false", conf.parameters[TrackerConfigurationKeys.SECURE]);
            Assert.AreEqual("/hit.xiti", conf.parameters[TrackerConfigurationKeys.PIXEL_PATH]);
            Assert.AreEqual("xiti.com", conf.parameters[TrackerConfigurationKeys.DOMAIN]);
            Assert.AreEqual("false", conf.parameters[TrackerConfigurationKeys.HASH_USER_ID]);
            Assert.AreEqual("never", conf.parameters[TrackerConfigurationKeys.OFFLINE_MODE]);
            Assert.AreEqual("true", conf.parameters[TrackerConfigurationKeys.PERSIST_IDENTIFIED_VISITOR]);
            Assert.AreEqual("30", conf.parameters[TrackerConfigurationKeys.CAMPAIGN_LIFETIME]);
            Assert.AreEqual("true", conf.parameters[TrackerConfigurationKeys.CAMPAIGN_LAST_PERSISTENCE]);
            Assert.AreEqual("60", conf.parameters[TrackerConfigurationKeys.SESSION_BACKGROUND_DURATION]);
        }

        [TestMethod]
        public void CustomConfigTest()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map[TrackerConfigurationKeys.LOG] = "logtest";
            map[TrackerConfigurationKeys.LOG_SSL] = "logstest";
            map[TrackerConfigurationKeys.SITE] = "123456";
            map[TrackerConfigurationKeys.OFFLINE_MODE] = "required";
            Configuration conf = new Configuration(map);

            Assert.AreEqual(13, conf.parameters.Count);
            Assert.AreEqual("logtest", conf.parameters[TrackerConfigurationKeys.LOG]);
            Assert.AreEqual("logstest", conf.parameters[TrackerConfigurationKeys.LOG_SSL]);
            Assert.AreEqual("123456", conf.parameters[TrackerConfigurationKeys.SITE]);
            Assert.AreEqual("guid", conf.parameters[TrackerConfigurationKeys.IDENTIFIER]);
            Assert.AreEqual("false", conf.parameters[TrackerConfigurationKeys.SECURE]);
            Assert.AreEqual("/hit.xiti", conf.parameters[TrackerConfigurationKeys.PIXEL_PATH]);
            Assert.AreEqual("xiti.com", conf.parameters[TrackerConfigurationKeys.DOMAIN]);
            Assert.AreEqual("false", conf.parameters[TrackerConfigurationKeys.HASH_USER_ID]);
            Assert.AreEqual("required", conf.parameters[TrackerConfigurationKeys.OFFLINE_MODE]);
            Assert.AreEqual("true", conf.parameters[TrackerConfigurationKeys.PERSIST_IDENTIFIED_VISITOR]);
            Assert.AreEqual("30", conf.parameters[TrackerConfigurationKeys.CAMPAIGN_LIFETIME]);
            Assert.AreEqual("true", conf.parameters[TrackerConfigurationKeys.CAMPAIGN_LAST_PERSISTENCE]);
            Assert.AreEqual("60", conf.parameters[TrackerConfigurationKeys.SESSION_BACKGROUND_DURATION]);
        }
    }
}
