namespace ATInternet
{
    #region CustomVarType
    public enum CustomVarType
    {
        App, Screen
    }

    #endregion

    #region CustomVar
    public class CustomVar : ScreenInfo
    {
        #region Members

		public int VarId { get; set; }

		public string Value { get; set; }

        public CustomVarType CustomVarType { get; set; }

        #endregion

        #region Constructor

        internal CustomVar(Tracker tracker) : base(tracker)
        {
            VarId = 0;
            CustomVarType = CustomVarType.App;
            Value = string.Empty;
        }

        #endregion

        #region Methods

		internal override void SetEvent()
        {
            VarId = VarId < 1 ? 1 : VarId;
            string result = CustomVarType == CustomVarType.App ? "x" : "f";
            tracker.SetParam(result+VarId, Value, new ParamOption() { Encode = true });
        }

        #endregion
    }

    #endregion

    #region CustomVars

	public class CustomVars
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal CustomVars(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

		public CustomVar Add(int varId, string customValue, CustomVarType type)
        {
            CustomVar cv = new CustomVar(tracker);
            cv.VarId = varId;
            cv.Value = customValue;
            cv.CustomVarType = type;
            tracker.businessObjects.Add(cv.id, cv);
            tracker.objectIndex++;

            return cv;
        }

        #endregion
    }

    #endregion
}
