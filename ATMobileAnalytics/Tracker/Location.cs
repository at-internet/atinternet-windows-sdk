namespace ATInternet
{
    #region Location
    public class Location : ScreenInfo
    {
        #region Members

        public double Latitude { get; set;}

        public double Longitude { get; set; }
        #endregion

        #region Constructor

        internal Location(Tracker tracker) : base(tracker) { }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            tracker.SetParam("gy", string.Format("{0:0.00}", Latitude))
                .SetParam("gx", string.Format("{0:0.00}", Longitude));
        }

        #endregion
    }

    #endregion

    #region Locations

    public class Locations
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Locations(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public Location Add(double latitude, double longitude)
        {
            Location loc = new Location(tracker);
            loc.Latitude = latitude;
            loc.Longitude = longitude;

            tracker.businessObjects.Add(loc.id, loc);
            tracker.objectIndex++;

            return loc;
        }

        #endregion
    }

    #endregion
}
