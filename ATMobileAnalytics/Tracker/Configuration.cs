using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace ATInternet
{

    #region Configuration
    class Configuration
    {
        #region Members

        /// <summary>
        /// Dictionary parameters
        /// </summary>
        internal Dictionary<string, string> parameters { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default configuration
        /// </summary>
        internal Configuration()
        {
            try
            {
                string configXMLPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets/defaultConfiguration.xml");
                XDocument loadedData = XDocument.Load(configXMLPath);
                var configVars = from data in loadedData.Descendants("var")
                                 select new
                                 {
                                     Key = data.Element("key").Value,
                                     Value = data.Element("value").Value
                                 };
                parameters = new Dictionary<string, string>();
                foreach (var configVar in configVars)
                {
                    parameters[configVar.Key] = configVar.Value.ToString();
                }
            }
            catch(FileNotFoundException ex)
            {
                ex.ToString();
                parameters = new Dictionary<string, string>()
                {
                    {"log", ""},
                    {"logSSL", ""},
                    {"site", ""},
                    {"identifier", "guid"},
                    {"secure", "false"},
                    {"pixelPath", "/hit.xiti"},
                    {"domain", "xiti.com"},
                    {"hashUserId", "false"},
                    {"storage", "never"},
                    {"persistIdentifiedVisitor", "true"},
                    {"campaignLifetime", "30"},
                    {"campaignLastPersistence", "true"},
                    {"sessionBackgroundDuration", "60"},
                };
            }
            
        }

        /// <summary>
        /// Custom configuration
        /// </summary>
        /// <param name="config"></param>
        public Configuration(IDictionary<string, string> config) : this()
        {
            foreach (string key in config.Keys)
            {
                parameters[key] = config[key];
            }
        }
        #endregion
    }

    #endregion
}
