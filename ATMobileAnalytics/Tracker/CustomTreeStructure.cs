namespace ATInternet
{
    #region CustomTreeStructure
    public class CustomTreeStructure : ScreenInfo
    {
        #region Members
        public int Category1 { get; set; }

        public int Category2 { get; set; }

        public int Category3 { get; set; }

        #endregion

        #region Constructor

        internal CustomTreeStructure(Tracker tracker) : base(tracker)
        {
            Category1 = 0;
            Category2 = 0;
            Category3 = 0;
        }

        internal override void SetEvent()
        {
            tracker.SetParam("ptype", string.Format("{0}-{1}-{2}", Category1, Category2, Category3));
        }

        #endregion
    }

    #endregion

    #region CustomTreeStructures

    public class CustomTreeStructures
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal CustomTreeStructures(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public CustomTreeStructure Add(int category1)
        {
            CustomTreeStructure cts = new CustomTreeStructure(tracker);
            cts.Category1 = category1;
            tracker.businessObjects.Add(cts.id, cts);
            tracker.objectIndex++;

            return cts;
        }

        public CustomTreeStructure Add(int category1, int category2)
        {
            CustomTreeStructure cts = Add(category1);
            cts.Category2 = category2;
            return cts;
        }

        public CustomTreeStructure Add(int category1, int category2, int category3)
        {
            CustomTreeStructure cts = Add(category1, category2);
            cts.Category3 = category3;
            return cts;
        }

        #endregion
    }

    #endregion
}
