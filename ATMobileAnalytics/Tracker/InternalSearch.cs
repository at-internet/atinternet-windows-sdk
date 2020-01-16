using System.Text.RegularExpressions;

namespace ATInternet
{
    #region InternalSearch
    public class InternalSearch : BusinessObject
    {

        #region Members

        public string KeywordLabel { get; set; }

        public int ResultScreenNumber { get; set; }

        public int ResultPosition { get; set; }

        #endregion

        #region Constructor

        internal InternalSearch(Tracker tracker) : base(tracker)
        {
            ResultScreenNumber = 1;
            ResultPosition = -1;
        }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            tracker.SetParam("mc", KeywordLabel == null ? string.Empty : rgx.Replace(KeywordLabel, ""))
                .SetParam("np", ResultScreenNumber);

            if(ResultPosition > -1)
            {
                tracker.SetParam("mcrg", ResultPosition);
            }
        }

        #endregion
    }

    #endregion

    #region InternalSearches

    public class InternalSearches
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal InternalSearches(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public InternalSearch Add(string keywordLabel, int resultScreenNumber)
        {
            InternalSearch internalSearch = new InternalSearch(tracker);
            internalSearch.KeywordLabel = keywordLabel;
            internalSearch.ResultScreenNumber = resultScreenNumber;

            tracker.businessObjects.Add(internalSearch.id, internalSearch);
            tracker.objectIndex++;

            return internalSearch;
        }

        public InternalSearch Add(string keywordLabel, int resultScreenNumber, int resultPosition)
        {
            InternalSearch internalSearch = Add(keywordLabel, resultScreenNumber);
            internalSearch.ResultPosition = resultPosition;
            return internalSearch;
        }

        #endregion
    }

    #endregion
}
