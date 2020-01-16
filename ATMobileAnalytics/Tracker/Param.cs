using System;
using System.Collections.Generic;

namespace ATInternet
{
    #region Param
    class Param
    {
        #region Members
        /// <summary>
        /// Param type
        /// </summary>
        internal enum Type
        {
            Integer, Double, Float, String, Boolean, Array, JSON, Closure, Unknown
        }

        /// <summary>
        /// Param key
        /// </summary>
        internal string key { get; set; }

        /// <summary>
        /// Param value
        /// </summary>
        internal Func<string> value;

        /// <summary>
        /// Param options
        /// </summary>
        internal ParamOption paramOptions { get; set; }

        /// <summary>
        /// Param type
        /// </summary>
        internal Type type { get; set; }

        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        internal Param()
        {
            type = Type.Unknown;
            key = string.Empty;
            value = null;
            paramOptions = null;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        internal Param(string key, Func<string> value, Type type) : this()
        {
            this.key = key;
            this.type = type;
            this.value = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        internal Param(string key, Func<string> value, Type type, ParamOption options) : this(key,value,type)
        {
            this.paramOptions = options;
        }

        #endregion
    }
    #endregion

    #region ParamOption
    #region RelativePosition enum
    /// <summary>
    /// Enum for relative position
    /// </summary>
    public enum RelativePosition
    {
        None, First, Last, Before, After
    }

    #endregion
    public class ParamOption
    {
        #region Members
        /// <summary>
        /// Relative position param
        /// </summary>
        public RelativePosition RelativePosition { get; set; }

        /// <summary>
        /// Relative position param key
        /// </summary>
        public string RelativeParameterKey { get; set; }

        /// <summary>
        /// Encoding option
        /// </summary>
        public bool Encode { get; set; }

        /// <summary>
        /// Separator for list
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Persistent or volatile
        /// </summary>
        public bool Persistent { get; set; }

        /// <summary>
        /// Option to append data into the same parameter
        /// </summary>
        public bool Append { get; set; }
        #endregion

        #region Constructor

        public ParamOption()
        {
            Separator = ",";
            Encode = false;
            RelativePosition = RelativePosition.None;
            Persistent = false;
            RelativeParameterKey = string.Empty;
        }

        #endregion
    }
    #endregion

    #region SliceReadyParam

    class SliceReadyParam
    {
        /// <summary>
        /// Get the param list ready to slice
        /// </summary>
        /// <returns></returns>
        internal static HashSet<string> list
        { get
            {
                return new HashSet<string> {
                    "stc",
                    "ati",
                    "atc",
                    "pdtl"};
            }
        }
    }

    #endregion

    #region ReadOnlyParam

    class ReadOnlyParam
    {
        /// <summary>
        /// Get the param list in read only mode
        /// </summary>
        /// <returns></returns>
        internal static HashSet<string> list
        {
            get
            {
                return new HashSet<string> {
                    "vtag",
                    "lng",
                    "mfmd",
                    "manufacturer",
                    "model",
                    "os",
                    "apvr",
                    "hl",
                    "car",
                    "cn",
                    "ts",
                    "olt"};
            }
        }
    }

    #endregion
}
