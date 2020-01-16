using System;
using System.Collections.Generic;

namespace ATInternet
{
    #region HitType

    public enum HitType
    {
        Unknown,
        Screen,
        Touch,
        Audio,
        Video,
        Animation,
        Podcast,
        RSS,
        Email,
        Advertising,
        AdTracking,
        ProductDisplay,
        Weborama,
        MVTesting
    }

    #endregion

    #region ProcessedHitType
    internal class ProcessedHitType
    {
        /// <summary>
        /// Get the param list ready to slice
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, HitType> list
        {
            get
            {
                return new Dictionary<string, HitType>
                {
                    {"audio",HitType.Audio },
                    {"video",HitType.Video },
                    {"vpre",HitType.Video },
                    {"vmid",HitType.Video },
                    {"vpost",HitType.Video },
                    {"animation",HitType.Animation },
                    {"anim",HitType.Animation },
                    {"podcast",HitType.Podcast },
                    {"rss",HitType.RSS },
                    {"email",HitType.Email },
                    {"pub",HitType.Advertising },
                    {"ad",HitType.Advertising },
                    {"click",HitType.Touch },
                    {"AT",HitType.AdTracking },
                    {"pdt",HitType.ProductDisplay },
                    {"wbo",HitType.Weborama },
                    {"mvt",HitType.MVTesting },
                    {"screen",HitType.Screen }
                };
            }
        }
    }

    #endregion

    #region Hit
    public class Hit
    {

        #region Members

        /// <summary>
        /// Url querystring
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        /// Indicates wheter the hit comes from storage
        /// </summary>
        public bool IsOffline { get; set; }

        /// <summary>
        /// Hit type
        /// </summary>
        public HitType Type {
            get {
                return GetHitType();
            }
        }

        #endregion

        #region Constructors 
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="url"></param>
        public Hit(string url)
        {
            Url = url;
            if (url.Contains("&ts="))
            {
                CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(double.Parse(Tool.GetTimeStampFromHit(url)));
            }
            IsOffline = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get hit type
        /// </summary>
        /// <param name="volatileParams"></param>
        /// <param name="persistentParams"></param>
        /// <returns></returns>
        internal static HitType GetHitType(List<Param> volatileParams, List<Param> persistentParams)
        {
            List<Param> buffer = new List<Param>();
            buffer.AddRange(volatileParams);
            buffer.AddRange(persistentParams);

            HitType type = HitType.Screen;

            foreach(Param p in buffer)
            {
                if(p.key.Equals("clic") || p.key.Equals("click") || (p.key.Equals("type") && ProcessedHitType.list.ContainsKey(p.value())))
                {
                    if (p.key.Equals("type"))
                    {
                        type = ProcessedHitType.list[p.value()];
                        break;
                    }

                    if(p.key.Equals("clic") || p.key.Equals("click"))
                    {
                        type = HitType.Touch;
                    }
                }
            }

            return type;
        }

        /// <summary>
        /// Get the hit type for offline hit
        /// </summary>
        /// <returns></returns>
        public HitType GetHitType()
        {
            HitType type = HitType.Screen;

            if(Url != null)
            {
                string[] hitComponents = Url.Split('&');

                foreach(string component in hitComponents)
                {
                    string[] parameterComponents = component.Split('=');

                    if (parameterComponents[0].Equals("type"))
                    {
                        type = ProcessedHitType.list[parameterComponents[1]];
                        break;
                    }

                    if (parameterComponents[0].Equals("clic") || parameterComponents[0].Equals("click"))
                    {
                        type = HitType.Touch;
                    }
                }
            }

            return type;
        }

        #endregion
    }

    #endregion
}
