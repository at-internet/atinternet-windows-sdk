using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ATInternet
{
    #region IdentifierType
    public enum IdentifierType
    {
        deviceId, guid
    }

    #endregion

    #region OfflineMode
    public enum OfflineMode
    {
        never, required, always
    }

    #endregion

    #region TrackerConfigurationKeys
    public class TrackerConfigurationKeys
    {
        public static readonly string LOG = "log";
        public static readonly string LOG_SSL = "logSSL";
        public static readonly string SITE = "site";
        public static readonly string PIXEL_PATH = "pixelPath";
        public static readonly string DOMAIN = "domain";
        public static readonly string SECURE = "secure";
        public static readonly string IDENTIFIER = "identifier";
        public static readonly string OFFLINE_MODE = "storage";
        public static readonly string HASH_USER_ID = "hashUserId";
        public static readonly string PERSIST_IDENTIFIED_VISITOR = "persistIdentifiedVisitor";
        public static readonly string CAMPAIGN_LAST_PERSISTENCE = "campaignLastPersistence";
        public static readonly string CAMPAIGN_LIFETIME = "campaignLifetime";
        public static readonly string SESSION_BACKGROUND_DURATION = "sessionBackgroundDuration";
    };

    #endregion

    #region Tracker
    public class Tracker
    {
        #region Members
        /// <summary>
        /// Configuration instance
        /// </summary>
        internal Configuration configuration { get; set; }

        /// <summary>
        /// Storage instance
        /// </summary>
        internal static Storage storage;

        /// <summary>
        /// Tracker queue instance
        /// </summary>
        internal static TrackerQueue trackerQueue;

        /// <summary>
        /// Tracker delegate instance
        /// </summary>
        public TrackerDelegate Delegate { get; set; }

        /// <summary>
        /// Buffer instance
        /// </summary>
        internal Buffer buffer;

        /// <summary>
        /// Dispatcher instance
        /// </summary>
        internal Dispatcher dispatcher;

        /// <summary>
        /// Dispatcher instance
        /// </summary>
        internal Dictionary<string, BusinessObject> businessObjects { get; set; }

        /// <summary>
        /// Shared preferences
        /// </summary>
        internal static ApplicationDataContainer LocalSettings
        {
            get
            {
                return ApplicationData.Current.LocalSettings.CreateContainer("ATLocalSettings", ApplicationDataCreateDisposition.Always);
            }
        }

        public Dictionary<string,string> Config {
            get
            {
                return configuration.parameters;
            }
        }

        /// <summary>
        /// Enable or disable tracking
        /// </summary>
        public static bool DoNotTrack
        {
            get
            {
                return TechnicalContext.DoNotTrack;
            }
            set
            {
                LocalSettings.Values["ATDoNotTrack"] = value;
            }
        }

        /// <summary>
        /// Time during the app was suspending
        /// </summary>
        static long timeInBackground = -1;

        /// <summary>
        /// Boolean to detect if view lifecycle tracking is enabled
        /// </summary>
        static bool isTrackerViewLifecycleEnabled = false;

        /// <summary>
        /// BusinessObject index
        /// </summary>
        internal int objectIndex = 0;

        #endregion

        #region Helpers

        /// <summary>
        /// Offline property
        /// </summary>
        private Offline _offline;
        public Offline Offline { get { return _offline ?? (_offline = new Offline(this)); } }

        /// <summary>
        /// Context property
        /// </summary>
        private Context _context;
        public Context Context { get { return _context ?? (_context = new Context(this)); } }

        /// <summary>
        /// Event property
        /// </summary>
        private Event _event;
        internal Event Event { get { return _event ?? (_event = new Event(this)); } }

        /// <summary>
        /// Screens property
        /// </summary>
        private Screens _screens;
        public Screens Screens { get { return _screens ?? (_screens = new Screens(this)); } }

        /// <summary>
        /// Dynamic Screens property
        /// </summary>
        private DynamicScreens _dynamicScreens;
        public DynamicScreens DynamicScreens { get { return _dynamicScreens ?? (_dynamicScreens = new DynamicScreens(this)); } }

        /// <summary>
        /// Publishers property
        /// </summary>
        private Publishers _publishers;
        public Publishers Publishers { get { return _publishers ?? (_publishers = new Publishers(this)); } }

        /// <summary>
        /// SelfPromotions property
        /// </summary>
        private SelfPromotions _selfPromotions;
        public SelfPromotions SelfPromotions { get { return _selfPromotions ?? (_selfPromotions = new SelfPromotions(this)); } }

        /// <summary>
        /// CustomObjects property
        /// </summary>
        private CustomObjects _customObjects;
        public CustomObjects CustomObjects { get { return _customObjects ?? (_customObjects = new CustomObjects(this)); } }

        /// <summary>
        /// CustomTreeStructures property
        /// </summary>
        private CustomTreeStructures _customTreeStructures;

        [Obsolete("customTreeStructures is deprecated, please use CustomTreeStructures instead.")]
        public CustomTreeStructures customTreeStructures { get { return _customTreeStructures ?? (_customTreeStructures = new CustomTreeStructures(this)); } }
        public CustomTreeStructures CustomTreeStructures { get { return _customTreeStructures ?? (_customTreeStructures = new CustomTreeStructures(this)); } }

        /// <summary>
        /// CustomVars property
        /// </summary>
        private CustomVars _customVars;
        public CustomVars CustomVars { get { return _customVars ?? (_customVars = new CustomVars(this)); } }

        /// <summary>
        /// Gestures property
        /// </summary>
        private Gestures _gestures;
        public Gestures Gestures { get { return _gestures ?? (_gestures = new Gestures(this)); } }

        /// <summary>
        /// IdentifiedVisitor property
        /// </summary>
        private IdentifiedVisitor _identifiedVisitor;
        public IdentifiedVisitor IdentifiedVisitor { get { return _identifiedVisitor ?? (_identifiedVisitor = new IdentifiedVisitor(this)); } }

        /// <summary>
        /// InternalSearches property
        /// </summary>
        private InternalSearches _internalSearches;
        public InternalSearches InternalSearches { get { return _internalSearches ?? (_internalSearches = new InternalSearches(this)); } }

        /// <summary>
        /// Locations property
        /// </summary>
        private Locations _locations;
        public Locations Locations { get { return _locations ?? (_locations = new Locations(this)); } }

        /// <summary>
        /// Products property
        /// </summary>
        private Products _products;
        public Products Products { get { return _products ?? (_products = new Products(this)); } }

        /// <summary>
        /// Cart property
        /// </summary>
        private Cart _cart;
        public Cart Cart { get { return _cart ?? (_cart = new Cart(this)); } }

        /// <summary>
        /// Aisles property
        /// </summary>
        private Aisles _aisles;
        public Aisles Aisles { get { return _aisles ?? (_aisles = new Aisles(this)); } }

        /// <summary>
        /// Orders property
        /// </summary>
        private Orders _orders;
        public Orders Orders { get { return _orders ?? (_orders = new Orders(this)); } }

        /// <summary>
        /// Campaigns property
        /// </summary>
        private Campaigns _campaigns;
        public Campaigns Campaigns { get { return _campaigns ?? (_campaigns = new Campaigns(this)); } }

        /// <summary>
        /// MediaPlayers property
        /// </summary>
        private MediaPlayers _mediaPlayers;
        public MediaPlayers MediaPlayers { get { return _mediaPlayers ?? (_mediaPlayers = new MediaPlayers(this)); } }

        #endregion

        #region Constructor

        public Tracker()
        {
            configuration = new Configuration();
            InitTracker();
        }

        public Tracker(IDictionary<string, string> config)
        {
            configuration = new Configuration(config);
            InitTracker();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize tracker members
        /// </summary>
        private void InitTracker()
        {
            trackerQueue = TrackerQueue.Instance;
            buffer = new Buffer(configuration);
            storage = Storage.Instance;
            dispatcher = new Dispatcher(this);
            businessObjects = new Dictionary<string, BusinessObject>();
            if (!LifeCycle.IsInitialized)
            {
                LifeCycle.InitLifeCycle();
            }

            if (!isTrackerViewLifecycleEnabled)
            {
                isTrackerViewLifecycleEnabled = true;
                Application.Current.Suspending += new SuspendingEventHandler(OnSuspending);
                Application.Current.Resuming += new EventHandler<object>(OnResuming);
            }
        }

        void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            timeInBackground = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        void OnResuming(object sender, object args)
        {
            if (timeInBackground > -1 && ((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - timeInBackground) > int.Parse(configuration.parameters["sessionBackgroundDuration"]))
            {
                timeInBackground = -1;
                LifeCycle.NewLaunchInit();
            }
        }

        /// <summary>
        /// Get the tracker configuration
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConfiguration()
        {
            return configuration.parameters;
        }

        /// <summary>
        /// Get the user id
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            if (!bool.Parse(configuration.parameters["hashUserId"]) || DoNotTrack)
            {
                return TechnicalContext.UserId(configuration.parameters["identifier"]);
            }
            else
            {
                return Tool.SHA_256(TechnicalContext.UserId(configuration.parameters["identifier"]));
            }
        }



        /// <summary>
        /// Set a new log
        /// </summary>
        /// <param name="log"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetLog(string log, TrackerReadyHandler trackerReadyHandler)
        {
            if (string.IsNullOrEmpty(log))
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for log, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.LOG, log, trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new secured log
        /// </summary>
        /// <param name="securedLog"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetSecuredLog(string securedLog, TrackerReadyHandler trackerReadyHandler)
        {
            if (string.IsNullOrEmpty(securedLog))
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for secured log, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.LOG_SSL, securedLog, trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new domain
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetDomain(string domain, TrackerReadyHandler trackerReadyHandler)
        {
            if (string.IsNullOrEmpty(domain))
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for domain, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.DOMAIN, domain, trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetSiteId(int siteId, TrackerReadyHandler trackerReadyHandler)
        {
            if (siteId <= 0)
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for site id, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.SITE, siteId.ToString(), trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new offline mode
        /// </summary>
        /// <param name="offlineMode"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetOfflineMode(OfflineMode offlineMode, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.OFFLINE_MODE, offlineMode.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Set a new identifierType
        /// </summary>
        /// <param name="identifierType"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetIdentifierType(IdentifierType identifierType, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.IDENTIFIER, identifierType.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Enable hash user id mode
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetHashUserIdEnabled(bool enabled, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.HASH_USER_ID, enabled.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Set a new pixel path
        /// </summary>
        /// <param name="pixelPath"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetPixelPath(string pixelPath, TrackerReadyHandler trackerReadyHandler)
        {
            if (string.IsNullOrEmpty(pixelPath))
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for pixel path, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.PIXEL_PATH, pixelPath, trackerReadyHandler);
            }
        }

        /// <summary>
        /// Enablesecure mode
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetSecureModeEnabled(bool enabled, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.SECURE, enabled.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Enable identified visitor persistence mode
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetPersistentIdentifiedVisitorEnabled(bool enabled, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.PERSIST_IDENTIFIED_VISITOR, enabled.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Enable campaign last persistence mode
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetCampaignLastPersistenceEnabled(bool enabled, TrackerReadyHandler trackerReadyHandler)
        {
            SetConfig(TrackerConfigurationKeys.CAMPAIGN_LAST_PERSISTENCE, enabled.ToString(), trackerReadyHandler);
        }

        /// <summary>
        /// Set a new campaign lifetime
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetCampaignLifetime(int lifetime, TrackerReadyHandler trackerReadyHandler)
        {
            if (lifetime <= 0)
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for campaign lifetime, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.CAMPAIGN_LIFETIME, lifetime.ToString(), trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new session background duration
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="trackerReadyHandler"></param>
        public void SetSessionBackgroundDuration(int duration, TrackerReadyHandler trackerReadyHandler)
        {
            if (duration <= 0)
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Bad value for session background duration, default value retained");
                }
            }
            else
            {
                SetConfig(TrackerConfigurationKeys.SESSION_BACKGROUND_DURATION, duration.ToString(), trackerReadyHandler);
            }
        }

        /// <summary>
        /// Set a new key/value config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="callback"></param>
        [Windows.Foundation.Metadata.DefaultOverload]
        public void SetConfig(string key, string value, TrackerReadyHandler handler)
        {
            trackerQueue.Add(new SetConfigRunnable(this, key, value, handler));
        }

        /// <summary>
        /// Set a new config
        /// </summary>
        /// <param name="newConfig"></param>
        /// <param name="overriding"></param>
        /// <param name="callback"></param>
        public void SetConfig(IDictionary<string, string> newConfig, bool overriding, TrackerReadyHandler handler)
        {
            trackerQueue.Add(new SetConfigRunnable(this, newConfig, overriding, handler));
        }


        /// <summary>
        /// Add a parameter in the hit querystring
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal Tracker SetParam(string key, Func<string> data, Param.Type type, ParamOption options)
        {
            if (!ReadOnlyParam.list.Contains(key))
            {
                Param p = new Param(key, data, type, options);
                List<int[]> positions = Tool.FindParameterPositions(key, buffer.persistentParameters, buffer.volatileParameters);

                if (options != null)
                {
                    if (options.Append)
                    {
                        foreach (int[] indexTab in positions)
                        {
                            int idArray = indexTab[0];
                            int position = indexTab[1];
                            if (options.Persistent && idArray == 1)
                            {
                                Param existingParam = buffer.volatileParameters[position];
                                buffer.volatileParameters.RemoveAt(position);
                                buffer.persistentParameters.Add(existingParam);
                            }
                            else if (idArray == 0)
                            {
                                Param existingParam = buffer.persistentParameters[position];
                                buffer.persistentParameters.RemoveAt(position);
                                buffer.volatileParameters.Add(existingParam);
                            }
                        }
                        if (options.Persistent)
                        {
                            buffer.persistentParameters.Add(p);
                        }
                        else
                        {
                            buffer.volatileParameters.Add(p);
                        }
                    }
                    else
                    {
                        if (positions.Count() > 0)
                        {
                            bool isFirst = true;
                            foreach (int[] indexTab in positions)
                            {
                                int idArray = indexTab[0];
                                int position = indexTab[1];
                                if (isFirst)
                                {
                                    isFirst = false;
                                    if (idArray == 0)
                                    {
                                        if (options.Persistent)
                                        {
                                            buffer.persistentParameters[position] = p;
                                        }
                                        else
                                        {
                                            buffer.persistentParameters.RemoveAt(position);
                                            buffer.volatileParameters.Add(p);
                                        }
                                    }
                                    else
                                    {
                                        if (options.Persistent)
                                        {
                                            buffer.volatileParameters.RemoveAt(position);
                                            buffer.persistentParameters.Add(p);
                                        }
                                        else
                                        {
                                            buffer.volatileParameters[position] = p;
                                        }
                                    }
                                }
                                else
                                {
                                    if (idArray == 0)
                                    {
                                        buffer.persistentParameters.RemoveAt(position);
                                    }
                                    else
                                    {
                                        buffer.volatileParameters.RemoveAt(position);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (options.Persistent)
                            {
                                buffer.persistentParameters.Add(p);
                            }
                            else
                            {
                                buffer.volatileParameters.Add(p);
                            }
                        }
                    }
                }
                else
                {
                    if (positions.Count() > 0)
                    {
                        bool isFirst = true;
                        foreach (int[] indexTab in positions)
                        {
                            int idArray = indexTab[0];
                            int position = indexTab[1];
                            if (isFirst)
                            {
                                isFirst = false;
                                if (idArray == 0)
                                {
                                    buffer.persistentParameters[position] = p;
                                }
                                else
                                {
                                    buffer.volatileParameters[position] = p;
                                }
                            }
                            else if (idArray == 0)
                            {
                                buffer.persistentParameters.RemoveAt(position);
                            }
                            else
                            {
                                buffer.volatileParameters.RemoveAt(position);
                            }
                        }
                    }
                    else
                    {
                        buffer.volatileParameters.Add(p);
                    }
                }
            }
            else
            {
                if (Delegate != null)
                {
                    Delegate.WarningDidOccur("Parameter " + key + " is read only. Value will not be updated");
                }
            }

            return this;
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Tracker SetParam(string key, string data)
        {
            if (data != null && !Tool.IsValidJSON(data))
                return HandleNotClosureStringParameterSetting(key, data, Param.Type.String, null);

            return HandleNotClosureStringParameterSetting(key, data, Param.Type.JSON, null);
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Tracker SetParam(string key, string data, ParamOption options)
        {
            if (data != null && !Tool.IsValidJSON(data))
                return HandleNotClosureStringParameterSetting(key, data, Param.Type.String, options);

            return HandleNotClosureStringParameterSetting(key, data, Param.Type.JSON, options);
        }

        public Tracker SetParam(string key, int data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Integer, null);
        }

        public Tracker SetParam(string key, int data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Integer, options);
        }

        public Tracker SetParam(string key, float data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Float, null);
        }

        public Tracker SetParam(string key, float data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Float, options);
        }

        public Tracker SetParam(string key, double data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Double, null);
        }

        public Tracker SetParam(string key, double data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Double, options);
        }

        public Tracker SetParam(string key, bool data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Boolean, null);
        }

        public Tracker SetParam(string key, bool data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Boolean, options);
        }

        public Tracker SetParam(string key, IEnumerable<object> data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Array, null);
        }

        public Tracker SetParam(string key, IEnumerable<object> data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.Array, options);
        }

        public Tracker SetParam(string key, IDictionary<string, object> data)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.JSON, null);
        }

        public Tracker SetParam(string key, IDictionary<string, object> data, ParamOption options)
        {
            return HandleNotClosureStringParameterSetting(key, data, Param.Type.JSON, options);
        }

        /// <summary>
        /// Remove parameter from buffer
        /// </summary>
        /// <param name="paramKey"></param>
        public Tracker UnsetParam(string paramKey)
        {
            List<int[]> positions = Tool.FindParameterPositions(paramKey, buffer.persistentParameters, buffer.volatileParameters);

            if (positions.Count() > 0)
            {
                foreach (int[] position in positions)
                {
                    if (position[0] == 0)
                    {
                        buffer.persistentParameters.RemoveAt(position[1]);
                    }
                    else
                    {
                        buffer.volatileParameters.RemoveAt(position[1]);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Convert value to lambda expression
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private Tracker HandleNotClosureStringParameterSetting(string key, object v, Param.Type type, ParamOption options)
        {
            return SetParam(key, (() => Tool.ConvertToString(v, options != null ? options.Separator : ",")), type, options);
        }

        public void Dispatch()
        {
            if (businessObjects.Count > 0)
            {
                List<BusinessObject> trackerObjects = new List<BusinessObject>();
                trackerObjects.AddRange(businessObjects.Values);

                List<BusinessObject> onAppAds = new List<BusinessObject>();
                List<BusinessObject> customObjects = new List<BusinessObject>();
                List<BusinessObject> screenObjects = new List<BusinessObject>();
                List<BusinessObject> salesTrackerObjects = new List<BusinessObject>();
                List<BusinessObject> internalSearchesObjects = new List<BusinessObject>();
                List<BusinessObject> productsObjects = new List<BusinessObject>();

                trackerObjects = trackerObjects.OrderBy(o => o.index).ToList();
                objectIndex = 0;
                foreach (BusinessObject businessObject in trackerObjects)
                {
                    if (!(businessObject is Product))
                    {
                        DispatchObjects(productsObjects, customObjects);
                    }

                    if (!(businessObject is OnAppAd || businessObject is ScreenInfo || businessObject is Screen || businessObject is DynamicScreen || businessObject is InternalSearch || businessObject is Cart || businessObject is Order)
                        || (businessObject is OnAppAd && (businessObject as OnAppAd).Action == OnAppAdAction.Touch))
                    {
                        DispatchObjects(onAppAds, customObjects);
                    }

                    if (businessObject is OnAppAd)
                    {
                        OnAppAd ad = businessObject as OnAppAd;
                        if (ad.Action == OnAppAdAction.View)
                        {
                            onAppAds.Add(ad);
                        }
                        else
                        {
                            customObjects.Add(businessObject);
                            dispatcher.Dispatch(customObjects.ToArray());
                            customObjects.Clear();
                        }
                    }
                    else if (businessObject is CustomObject)
                    {
                        customObjects.Add(businessObject);
                    }
                    else if (businessObject is ScreenInfo)
                    {
                        screenObjects.Add(businessObject);
                    }
                    else if (businessObject is InternalSearch)
                    {
                        internalSearchesObjects.Add(businessObject);
                    }
                    else if (businessObject is Product)
                    {
                        productsObjects.Add(businessObject);
                    }
                    else if (businessObject is Order || businessObject is Cart)
                    {
                        salesTrackerObjects.Add(businessObject);
                    }
                    else if (businessObject is Screen || businessObject is DynamicScreen)
                    {
                        onAppAds.AddRange(customObjects);
                        onAppAds.AddRange(screenObjects);
                        onAppAds.AddRange(internalSearchesObjects);

                        List<BusinessObject> orders = new List<BusinessObject>();
                        Cart cart = null;
                        foreach (BusinessObject obj in salesTrackerObjects)
                        {
                            if (obj is Cart)
                            {
                                cart = obj as Cart;
                            }
                            else
                            {
                                orders.Add(obj);
                            }
                        }

                        if (cart != null)
                        {
                            if (businessObject is Screen && ((businessObject as Screen).IsBasketScreen || orders.Count > 0))
                            {
                                onAppAds.Add(cart);
                            }
                            else if ((businessObject as DynamicScreen).IsBasketScreen || orders.Count > 0)
                            {
                                onAppAds.Add(cart);
                            }
                        }

                        onAppAds.AddRange(orders);
                        onAppAds.Add(businessObject);
                        dispatcher.Dispatch(onAppAds.ToArray());

                        salesTrackerObjects.Clear();
                        screenObjects.Clear();
                        internalSearchesObjects.Clear();
                        onAppAds.Clear();
                        customObjects.Clear();
                    }
                    else
                    {
                        if (businessObject is Gesture && (businessObject as Gesture).Action == GestureAction.InternalSearch)
                        {
                            onAppAds.AddRange(internalSearchesObjects);
                            internalSearchesObjects.Clear();
                        }
                        onAppAds.AddRange(customObjects);
                        onAppAds.Add(businessObject);

                        dispatcher.Dispatch(onAppAds.ToArray());

                        onAppAds.Clear();
                        customObjects.Clear();
                    }
                }
                DispatchObjects(onAppAds, customObjects);

                DispatchObjects(productsObjects, customObjects);

                if (customObjects.Count > 0 || screenObjects.Count > 0 || internalSearchesObjects.Count > 0)
                {
                    customObjects.AddRange(screenObjects);
                    customObjects.AddRange(internalSearchesObjects);
                    dispatcher.Dispatch(customObjects.ToArray());

                    customObjects.Clear();
                    screenObjects.Clear();
                    internalSearchesObjects.Clear();
                }
            }
            else
            {
                dispatcher.Dispatch();
            }
        }

        internal void DispatchObjects(List<BusinessObject> objects, List<BusinessObject> customObjects)
        {
            if (objects.Count > 0)
            {
                objects.AddRange(customObjects);
                dispatcher.Dispatch(objects.ToArray());
                customObjects.Clear();
                objects.Clear();
            }
        }

        #endregion
    }

    #endregion

    #region TrackerQueue

    internal class TrackerQueue : BlockingCollection<Runnable>
    {
        #region Members

        /// <summary>
        /// Singleton tracker queue
        /// </summary>
        static readonly TrackerQueue _instance = new TrackerQueue();
        internal static TrackerQueue Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Task executing other tasks
        /// </summary>
        internal Task executor { get; }

        #endregion

        #region Constructor

        TrackerQueue()
        {
            executor = Task.Run(() =>
            {
                while (true)
                {
                    Take().Run();
                }
            });
        }

        #endregion
    }

    #endregion

    #region TrackerDelegate

    #region HitStatus

    /// <summary>
    /// Enum for Tracker callbacks
    /// </summary>
    public enum HitStatus
    {
        Failed, Success
    }

    #endregion

    public interface TrackerDelegate
    {

        /// <summary>
        /// First launched approbation callback
        /// </summary>
        /// <param name="message"></param>
        void TrackerNeedsFirstLaunchApproval(string message);

        /// <summary>
        /// End hit construction callback
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        void BuildDidEnd(HitStatus status, string message);

        /// <summary>
        /// End sending hit callback
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        void SendDidEnd(HitStatus status, string message);

        /// <summary>
        /// Call partner callback
        /// </summary>
        /// <param name="response"></param>
        void DidCallPartner(string response);

        /// <summary>
        /// Warning callback
        /// </summary>
        /// <param name="message"></param>
        void WarningDidOccur(string message);

        /// <summary>
        /// Save did end callback
        /// </summary>
        /// <param name="message"></param>
        void SaveDidEnd(string message);

        /// <summary>
        /// Error callback
        /// </summary>
        /// <param name="message"></param>
        void ErrorDidOccur(string message);
    }

    #endregion

    #region Context

    public enum BackgroundMode
    {
        Normal, Task
    }

    public class Context
    {
        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        #region Level2

        private int _level2 = 0;
        public int Level2
        {
            get
            {
                return _level2;
            }
            set
            {
                _level2 = value;
                if (_level2 > 0)
                {
                    tracker.SetParam("s2", _level2, new ParamOption() { Persistent = true });
                }
                else
                {
                    tracker.UnsetParam("s2");
                }
            }
        }

        #endregion

        #region BackgroundMode

        private BackgroundMode _backgroundMode;
        public BackgroundMode BackgroundMode
        {
            get
            {
                return _backgroundMode;
            }
            set
            {
                _backgroundMode = value;
                switch (_backgroundMode)
                {
                    case BackgroundMode.Task:
                        tracker.SetParam("bg", "task", new ParamOption() { Persistent = true });
                        break;
                    default:
                        tracker.UnsetParam("bg");
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        internal Context(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion
    }

    #endregion
}
