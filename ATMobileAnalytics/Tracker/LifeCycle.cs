using System;
using System.Collections.Generic;
using System.Globalization;

namespace ATInternet
{
    #region LifeCycle
    internal class LifeCycle
    {
        #region Keys

        internal static string LAST_USE_DATE = "ATLastUseDate";

        internal static string FIRST_SESSION = "ATFirstLaunch";
        internal static string FIRST_SESSION_DATE = "ATFirstLaunchDate";
        internal static string FIRST_SESSION_AFTER_UPDATE = "ATFirstLaunchAfterUpdate";
        internal static string FIRST_SESSION_DATE_AFTER_UPDATE = "ATUpdateLaunchDate";

        internal static string SESSION_COUNT = "ATLaunchCount";
        internal static string SESSION_COUNT_SINCE_UPDATE = "ATLaunchCountSinceUpdate";

        internal static string DAYS_SINCE_FIRST_SESSION = "ATDaysSinceFirstLaunch";
        internal static string DAYS_SINCE_UPDATE = "ATDaysSinceUpdate";
        internal static string DAYS_SINCE_LAST_SESSION = "ATDaysSinceLastUse";

        internal static string LAST_APPLICATION_VERSION = "ATLastApplicationVersion";

        #endregion

        #region Members

        /// <summary>
        /// Boolean to know if lifecycle is initialized
        /// </summary>
        internal static bool IsInitialized = false;

        /// <summary>
        /// Session identifier
        /// </summary>
        private static string sessionId;

        #endregion

        #region Methods

        /// <summary>
        /// Init lifecycle metrics
        /// </summary>
        internal static void InitLifeCycle()
        {
            // Not first launch
            if (Tracker.LocalSettings.Values.ContainsKey(FIRST_SESSION))
            {
                NewLaunchInit();
            }
            else
            {
                FirstSessionInit();
            }
            IsInitialized = true;
        }

        /// <summary>
        /// First session init
        /// </summary>
        internal static void FirstSessionInit()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            var currentCulture = CultureInfo.CurrentCulture;
            int weekOnYear = currentCulture.Calendar.GetWeekOfYear(
                DateTime.Now,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);

            Tracker.LocalSettings.Values[FIRST_SESSION] = true;
            Tracker.LocalSettings.Values[FIRST_SESSION_AFTER_UPDATE] = false;

            Tracker.LocalSettings.Values[SESSION_COUNT] = 1;
            Tracker.LocalSettings.Values[SESSION_COUNT_SINCE_UPDATE] = 1;
            Tracker.LocalSettings.Values[DAYS_SINCE_FIRST_SESSION] = 0;
            Tracker.LocalSettings.Values[DAYS_SINCE_LAST_SESSION] = 0;
            Tracker.LocalSettings.Values[FIRST_SESSION_DATE] = now.ToString("yyyyMMdd");

            Tracker.LocalSettings.Values[LAST_APPLICATION_VERSION] = TechnicalContext.Apvr();
            sessionId = Guid.NewGuid().ToString();
        }

        internal static void updateFirstLaunch()
        {
            Tracker.LocalSettings.Values[FIRST_SESSION] = false;
            Tracker.LocalSettings.Values[FIRST_SESSION_AFTER_UPDATE] = false;
        }

        /// <summary>
        /// New launch init
        /// </summary>
        internal static void NewLaunchInit()
        {
            updateFirstLaunch();
            DateTimeOffset now = DateTimeOffset.Now;

            var currentCulture = CultureInfo.CurrentCulture;
            int weekOnYear = currentCulture.Calendar.GetWeekOfYear(
                DateTime.Now,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);

            // dsfs
            string savedFld = (string)Tracker.LocalSettings.Values[FIRST_SESSION_DATE];
            if (!string.IsNullOrEmpty(savedFld))
            {
                DateTimeOffset firstLaunchDate = DateTimeOffset.ParseExact(savedFld, "yyyyMMdd", CultureInfo.InvariantCulture);
                Tracker.LocalSettings.Values[DAYS_SINCE_FIRST_SESSION] = (int)(now - firstLaunchDate).TotalDays;
            }

            // dsu
            string saveduld = (string)Tracker.LocalSettings.Values[FIRST_SESSION_DATE_AFTER_UPDATE];
            if (!string.IsNullOrEmpty(saveduld))
            {
                DateTimeOffset updateLaunchDate = DateTimeOffset.ParseExact(saveduld, "yyyyMMdd", CultureInfo.InvariantCulture);
                Tracker.LocalSettings.Values[DAYS_SINCE_UPDATE] = (int)(now - updateLaunchDate).TotalDays;
            }

            // dsls
            string savedlud = (string)Tracker.LocalSettings.Values[LAST_USE_DATE];
            if (!string.IsNullOrEmpty(savedlud))
            {
                DateTimeOffset lastUseDate = DateTimeOffset.ParseExact(savedlud, "yyyyMMdd", CultureInfo.InvariantCulture);
                Tracker.LocalSettings.Values[DAYS_SINCE_LAST_SESSION] = (int)(now - lastUseDate).TotalDays;
            }

            // sc
            Tracker.LocalSettings.Values[SESSION_COUNT] = (int)Tracker.LocalSettings.Values[SESSION_COUNT] + 1;

            //scsu
            Tracker.LocalSettings.Values[SESSION_COUNT_SINCE_UPDATE] = (int)Tracker.LocalSettings.Values[SESSION_COUNT_SINCE_UPDATE] + 1;

            // Application version changed
            string savedApvr = Tracker.LocalSettings.Values[LAST_APPLICATION_VERSION] as string;
            string apvr = TechnicalContext.Apvr();
            // Update detected
            if (!apvr.Equals(savedApvr))
            {
                Tracker.LocalSettings.Values[FIRST_SESSION_DATE_AFTER_UPDATE] = now.ToString("yyyyMMdd");
                Tracker.LocalSettings.Values[LAST_APPLICATION_VERSION] = apvr;
                Tracker.LocalSettings.Values[SESSION_COUNT_SINCE_UPDATE] = 1;
                Tracker.LocalSettings.Values[DAYS_SINCE_UPDATE] = 0;
                Tracker.LocalSettings.Values[FIRST_SESSION_AFTER_UPDATE] = true;
            }

            Tracker.LocalSettings.Values[LAST_USE_DATE] = now.ToString("yyyyMMdd");
            sessionId = Guid.NewGuid().ToString();

        }

        /// <summary>
        /// Get lifecycles metrics
        /// </summary>
        /// <returns></returns>
        internal static Func<string> GetMetrics()
        {
            return (() =>
            {
                Dictionary<string, object> map = new Dictionary<string, object>();

                // fs
                map.Add("fs", (bool)Tracker.LocalSettings.Values[FIRST_SESSION] ? 1 : 0);

                // fsau
                map.Add("fsau", (bool)Tracker.LocalSettings.Values[FIRST_SESSION_AFTER_UPDATE] ? 1 : 0);

                if (Tracker.LocalSettings.Values.ContainsKey(FIRST_SESSION_DATE_AFTER_UPDATE))
                {
                    map.Add("scsu", Tracker.LocalSettings.Values[SESSION_COUNT_SINCE_UPDATE] as int?);
                    map.Add("fsdau", int.Parse(Tracker.LocalSettings.Values[FIRST_SESSION_DATE_AFTER_UPDATE].ToString()));
                    map.Add("dsu", Tracker.LocalSettings.Values[DAYS_SINCE_UPDATE] as int?);
                }

                map.Add("sc", Tracker.LocalSettings.Values[SESSION_COUNT] as int?);
                map.Add("fsd", int.Parse(Tracker.LocalSettings.Values[FIRST_SESSION_DATE].ToString()));
                map.Add("dsls", Tracker.LocalSettings.Values[DAYS_SINCE_LAST_SESSION] as int?);
                map.Add("dsfs", Tracker.LocalSettings.Values[DAYS_SINCE_FIRST_SESSION] as int?);

                map.Add("sessionId", sessionId);

                Dictionary<string, object> lifecycle = new Dictionary<string, object>();
                lifecycle.Add("lifecycle", map);
                return Tool.ConvertToString(lifecycle);
            });
        }

        #endregion
    }

    #endregion
}
