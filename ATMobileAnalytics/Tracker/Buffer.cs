using System.Collections.Generic;

namespace ATInternet
{
    #region Buffer
    class Buffer
    {
        #region Members
        /// <summary>
        /// Persistent parameters list
        /// </summary>
        internal List<Param> persistentParameters { get; }

        /// <summary>
        /// Volatile parameters list
        /// </summary>
        internal List<Param> volatileParameters { get; }

        #endregion

        #region Constructor

        internal Buffer(Configuration configuration)
        {
            persistentParameters = new List<Param>();
            volatileParameters = new List<Param>();

            AddContextVariables(configuration);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add context variables to the hit 
        /// </summary>
        private void AddContextVariables(Configuration configuration)
        {
            ParamOption persistentOption = new ParamOption() { Persistent = true };
            ParamOption persistentOptionWithEncoding = new ParamOption() { Persistent = true , Encode = true};

            // Add sdk version
            persistentParameters.Add(new Param("vtag", TechnicalContext.TagVersion, Param.Type.String, persistentOption));
            // Add platform tag
            persistentParameters.Add(new Param("ptag", TechnicalContext.TagPlatform, Param.Type.String, persistentOption));
            // Add device language
            persistentParameters.Add(new Param("lng", TechnicalContext.Language, Param.Type.String, persistentOption));
            // Add device info
            persistentParameters.Add(new Param("mfmd", TechnicalContext.Device, Param.Type.String, persistentOption));
            // Add device manufacturer
            persistentParameters.Add(new Param("manufacturer", TechnicalContext.Manufacturer, Param.Type.String, persistentOptionWithEncoding));
            // Add device model
            persistentParameters.Add(new Param("model", TechnicalContext.Model, Param.Type.String, persistentOptionWithEncoding));
            // Add os
            persistentParameters.Add(new Param("os", TechnicalContext.OS, Param.Type.String, persistentOption));
            // Add application identifier
            persistentParameters.Add(new Param("apid", TechnicalContext.ApplicationId, Param.Type.String, persistentOption));
            // Add application version
            persistentParameters.Add(new Param("apvr", TechnicalContext.Apvr, Param.Type.String, persistentOptionWithEncoding));
            // Add local hour
            persistentParameters.Add(new Param("hl", TechnicalContext.LocalHour, Param.Type.String, persistentOption));
            // Add connexion info
            persistentParameters.Add(new Param("cn", TechnicalContext.ConnectionType, Param.Type.String, persistentOptionWithEncoding));
            // Add timestamp
            persistentParameters.Add(new Param("ts", TechnicalContext.Timestamp, Param.Type.String, persistentOption));
            // Add id client
            persistentParameters.Add(new Param("idclient", (() => TechnicalContext.UserId(configuration.parameters["identifier"])), Param.Type.String, persistentOption));
        }

        #endregion

    }

    #endregion
}
