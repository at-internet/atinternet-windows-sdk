using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace ATInternet
{
    #region TechnicalContext
    internal class TechnicalContext
    {
        #region Members

        /// <summary>
        /// Static variable to save screen name
        /// </summary>
        internal static string ScreenName { get; set; }

        /// <summary>
        /// Static variable to save level 2
        /// </summary>
        internal static int Level2 { get; set; }

        /// <summary>
        /// Static variable to enable or disable tracking
        /// </summary>
        internal static bool DoNotTrack
        {
            get
            {
                return Tracker.LocalSettings.Values["ATDoNotTrack"] != null && (bool)Tracker.LocalSettings.Values["ATDoNotTrack"];
            }
            set
            {
                Tracker.LocalSettings.Values["ATDoNotTrack"] = value;
            }
        }

        /// <summary>
        /// Get tag version
        /// </summary>
        internal static Func<string> TagVersion = (() => "1.3.0");

        /// <summary>
        /// Get tag platform
        /// </summary>
        internal static Func<string> TagPlatform = (() => "Windows");

        /// <summary>
        /// Get connection type
        /// </summary>
        internal static Func<string> ConnectionType = (() => Reachability.GetConnectionString());

        // TODO get carrier

        /// <summary>
        /// Get language
        /// </summary>
        internal static Func<string> Language = (() => Windows.Globalization.Language.CurrentInputMethodLanguageTag);
        
        /// <summary>
        /// Get manufacturer
        /// </summary>
        internal static Func<string> Manufacturer = (() => new EasClientDeviceInformation().SystemManufacturer);

        /// <summary>
        /// Get model
        /// </summary>
        internal static Func<string> Model = (() => new EasClientDeviceInformation().SystemProductName);

        /// <summary>
        /// Get device
        /// </summary>        
        internal static Func<string> Device = (() =>
            {
                EasClientDeviceInformation device = new EasClientDeviceInformation();
                Regex reg = new Regex("(\\W)|(_)");
                return string.Format("[{0}]-[{1}]",
                    reg.Replace(device.SystemManufacturer, ""),
                    reg.Replace(device.SystemProductName, ""));
            }
        );

        /// <summary>
        /// Get os
        /// </summary>        
        internal static Func<string> OS = (() =>
            {
                EasClientDeviceInformation device = new EasClientDeviceInformation();
                Regex reg = new Regex("(\\W)");
                return string.Format("[{0}]-[10]",
                    reg.Replace(device.OperatingSystem, ""));
            }
        );

        /// <summary>
        /// Get application version
        /// </summary>
        internal static Func<string> Apvr = (() =>
            {
                PackageVersion version = Package.Current.Id.Version;
                return string.Format("[{0}.{1}.{2}.{3}]", version.Major, version.Minor, version.Build, version.Revision);
            }
        );

        /// <summary>
        /// Get timestamp
        /// </summary>
        internal static Func<string> Timestamp = (() =>
            {
                return ((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000).ToString();
            }
        );

        /// <summary>
        /// Get local hour
        /// </summary>
        internal static Func<string> LocalHour = (() =>
            {
                return DateTimeOffset.Now.ToString("HH'x'mm'x'ss");
            }
        );

        /// <summary>
        /// Get application identifier
        /// </summary>
        internal static Func<string> ApplicationId = (() =>
            {
                return Package.Current.Id.Name;
            }
        );

        /// <summary>
        /// Get userId
        /// </summary>

        internal static string UserId(string identifier)
        {
            if (Tracker.DoNotTrack)
            {
                return "opt-out";
            }
            else if (identifier.Equals("deviceId"))
            {
                return new EasClientDeviceInformation().Id.ToString();
            }
            else
            {
                string idClient = (string)Tracker.LocalSettings.Values["ATIdClient"];
                if (idClient == null)
                {
                    idClient = Guid.NewGuid().ToString();
                    Tracker.LocalSettings.Values["ATIdClient"] = idClient;
                }
                return idClient;
            }
        }

        #endregion

    }

    #endregion
}
