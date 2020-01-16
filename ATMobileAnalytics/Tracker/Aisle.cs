namespace ATInternet
{
    #region Aisle
    public class Aisle : ScreenInfo
    {
        #region Members

        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public string Level5 { get; set; }
        public string Level6 { get; set; }

        #endregion

        #region Constructor

        internal Aisle(Tracker tracker) : base(tracker) { }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            string value = Level1 == null ? string.Empty : Level1;
            value += Level2 == null ? string.Empty : "::" + Level2;
            value += Level3 == null ? string.Empty : "::" + Level3;
            value += Level4 == null ? string.Empty : "::" + Level4;
            value += Level5 == null ? string.Empty : "::" + Level5;
            value += Level6 == null ? string.Empty : "::" + Level6;

            tracker.SetParam("aisl", value, new ParamOption() { Encode = true });
        }

        #endregion
    }

    #endregion

    #region Aisles

    public class Aisles
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Aisles(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods 

        public Aisle Add(string level1)
        {
            Aisle aisle = new Aisle(tracker);
            aisle.Level1 = level1;
            tracker.businessObjects.Add(aisle.id, aisle);
            tracker.objectIndex++;
            return aisle;
        }

        public Aisle Add(string level1, string level2)
        {
            Aisle aisle = Add(level1);
            aisle.Level2 = level2;
            return aisle;
        }

        public Aisle Add(string level1, string level2, string level3)
        {
            Aisle aisle = Add(level1, level2);
            aisle.Level3 = level3;
            return aisle;
        }

        public Aisle Add(string level1, string level2, string level3, string level4)
        {
            Aisle aisle = Add(level1, level2, level3);
            aisle.Level4 = level4;
            return aisle;
        }

        public Aisle Add(string level1, string level2, string level3, string level4, string level5)
        {
            Aisle aisle = Add(level1, level2, level3, level4);
            aisle.Level5 = level5;
            return aisle;
        }

        public Aisle Add(string level1, string level2, string level3, string level4, string level5, string level6)
        {
            Aisle aisle = Add(level1, level2, level3, level4, level5);
            aisle.Level6 = level6;
            return aisle;
        }

        #endregion
    }

    #endregion
}
