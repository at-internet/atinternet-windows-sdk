using Windows.Data.Json;

namespace ATInternet
{
    #region Plugin
    abstract class Plugin
    {

        #region Members
        /// <summary>
        /// Response returned by the execute function
        /// </summary>
        internal string response = (new JsonObject()).Stringify();

        /// <summary>
        /// Response type
        /// </summary>
        internal Param.Type responseType;

        /// <summary>
        /// Tracker instance
        /// </summary>
        internal Tracker tracker;

        /// <summary>
        /// Parameter key
        /// </summary>
        internal string paramKey;

        #endregion

        #region Methods

        /// <summary>
        /// Execute call
        /// </summary>
        /// <param name="tracker"></param>
        internal abstract void Execute(Tracker tracker);

        #endregion

    }

    #endregion
}
