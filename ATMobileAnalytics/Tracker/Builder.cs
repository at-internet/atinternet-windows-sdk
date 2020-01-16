using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Json;

namespace ATInternet
{
    class Builder : Runnable
    {
        #region Constants

        /// <summary>
        /// Configuration parts count
        /// </summary>
        private const int REFCONFIGCHUNKS = 4;

        /// <summary>
        /// Constant for secure key
        /// </summary>
        private const string SSL_KEY = "secure";

        /// <summary>
        /// Constant for log key
        /// </summary>
        private const string LOG_KEY = "log";

        /// <summary>
        /// Constant for secured log key
        /// </summary>
        private const string SSL_LOG_KEY = "logSSL";

        /// <summary>
        /// Constant for site key
        /// </summary>
        private const string SITE_KEY = "site";

        /// <summary>
        /// Constant for pixel path key
        /// </summary>
        private const string PIXEL_PATH_KEY = "pixelPath";

        /// <summary>
        /// Constant for domain key
        /// </summary>
        private const string DOMAIN_KEY = "domain";

        /// <summary>
        /// Hit max length
        /// </summary>
        private const int HITMAXLENGTH = 1600;

        /// <summary>
        /// MHID length
        /// </summary>
        private const int MHIDLENGTH = 30;

        /// <summary>
        /// Hits max count
        /// </summary>
        private const int HITMAXCOUNT = 999;

        #endregion

        #region Members

        /// <summary>
        /// Tracker instance
        /// </summary>
        private Tracker tracker;

        /// <summary>
        /// Volatile parameters
        /// </summary>
        private List<Param> volatileParameters;

        /// <summary>
        /// Persistent parameters
        /// </summary>
        private List<Param> persistentParameters;

        #endregion

        #region Constructor

        internal Builder(Tracker tracker)
        {
            this.tracker = tracker;
            this.volatileParameters = new List<Param>(tracker.buffer.volatileParameters);
            this.persistentParameters = new List<Param>(tracker.buffer.persistentParameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the hit configuration
        /// </summary>
        /// <returns></returns>
        internal string BuildConfiguration()
        {
            string hitConfig = string.Empty;
            int hitConfigChunks = 0;

            bool isSecure = bool.Parse(tracker.configuration.parameters[SSL_KEY]);
            string log = tracker.configuration.parameters[LOG_KEY];
            string securedLog = tracker.configuration.parameters[SSL_LOG_KEY];
            string domain = tracker.configuration.parameters[DOMAIN_KEY];
            string pixelPath = tracker.configuration.parameters[PIXEL_PATH_KEY];
            int siteId = int.Parse(tracker.configuration.parameters[SITE_KEY]);

            if (isSecure)
            {
                if (!string.IsNullOrEmpty(securedLog))
                {
                    hitConfig += "https://" + securedLog + ".";
                    hitConfigChunks++;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(log))
                {
                    hitConfig += "http://" + log + ".";
                    hitConfigChunks++;
                }
            }

            if (!string.IsNullOrEmpty(domain))
            {
                hitConfig += domain;
                hitConfigChunks++;
            }
            if (!string.IsNullOrEmpty(pixelPath))
            {
                hitConfig += pixelPath;
                hitConfigChunks++;
            }

            hitConfig += "?s=" + siteId;
            hitConfigChunks++;

            if (hitConfigChunks != REFCONFIGCHUNKS)
            {
                hitConfig = string.Empty;
                if (tracker.Delegate != null)
                {
                    tracker.Delegate.WarningDidOccur("There is something wrong with configuration: " + hitConfig + ". Expected 4 configuration keys, found " + hitConfigChunks);
                }
            }

            return hitConfig;
        }

        /// <summary>
        /// Builds hit(s)
        /// </summary>
        /// <returns></returns>
        internal object[] Build()
        {
            string hit = string.Empty;
            int hitsCount = 1;
            List<string> hits = new List<string>();
            bool error = false;

            string idClient = TechnicalContext.UserId(tracker.configuration.parameters["identifier"]);
            string config = BuildConfiguration();
            string oltParam = TechnicalContext.Timestamp();

            int MAX_LENGTH_AVAILABLE = HITMAXLENGTH - (config.Length + oltParam.Length + idClient.Length + MHIDLENGTH);

            List<Tuple<Param, string>> formattedParams;
            if (config.Length != 0)
            {
                formattedParams = PrepareQuery();

                foreach (Tuple<Param, string> queryString in formattedParams)
                {
                    /// If the querystring is too long
                    if (queryString.Item2.Length > MAX_LENGTH_AVAILABLE)
                    {
                        /// Check if the concerned parameter value in the queryString is allowed to be sliced
                        if (SliceReadyParam.list.Contains(queryString.Item1.key))
                        {
                            string separator;
                            if (queryString.Item1.paramOptions != null)
                            {
                                separator = queryString.Item1.paramOptions.Separator;
                            }
                            else
                            {
                                separator = ",";
                            }

                            /// We slice the parameter value on the parameter separator
                            string[] components = queryString.Item2.Split('=')[1].Split(separator.ToCharArray()[0]);

                            /// Parameter key to re-add on each chunks where the value is spread
                            bool keyAdded = false;
                            string keySplit = MakeSubQuery(queryString.Item1.key, string.Empty);

                            /// For each sliced value we check if we can add it to current hit, else we create and add a new hit
                            foreach (string component in components)
                            {
                                if (!keyAdded && (hit + keySplit + component).Length <= MAX_LENGTH_AVAILABLE)
                                {
                                    hit += keySplit + component;
                                    keyAdded = true;
                                }
                                else if (keyAdded && (hit + separator + component).Length < MAX_LENGTH_AVAILABLE)
                                {
                                    hit += separator + component;
                                }
                                else
                                {
                                    if (hit != string.Empty)
                                    {
                                        hits.Add(hit + separator);
                                    }
                                    hit = keySplit + component;
                                    if (hitsCount >= HITMAXCOUNT)
                                    {
                                        // Too much hits
                                        error = true;
                                        break;
                                    }
                                    else if (hit.Length > MAX_LENGTH_AVAILABLE)
                                    {
                                        /// Value still too long
                                        if (tracker.Delegate != null)
                                        {
                                            tracker.Delegate.WarningDidOccur("Multihits: value still too long after slicing");
                                        }

                                        /// Truncate the value
                                        hit = hit.Substring(0, MAX_LENGTH_AVAILABLE - MakeSubQuery("mherr", "1").Length);

                                        ///  Check if in the last 5 characters there is misencoded character, if so we truncate again
                                        string lastChars = hit.Substring(hit.Length - 5);
                                        int percentIndex = lastChars.IndexOf("%");
                                        if (percentIndex != -1)
                                        {
                                            hit = hit.Substring(0, hit.Length - 5);
                                        }
                                        hit += MakeSubQuery("mherr", "1");
                                        error = true;
                                        break;
                                    }
                                    hitsCount++;
                                }
                            }

                            if (error)
                            {
                                break;
                            }
                        }
                        else
                        {
                            /// Value can't be sliced
                            if (tracker.Delegate != null)
                            {
                                tracker.Delegate.WarningDidOccur("Multihits: parameter value not allowed to be sliced");
                            }
                            hit += MakeSubQuery("mherr", "1");
                            error = true;
                            break;
                        }
                    }
                    /// Else if the current hit + queryString length is not too long, we add it to the current hit
                    else if ((hit + queryString.Item2).Length <= MAX_LENGTH_AVAILABLE)
                    {
                        hit += queryString.Item2;
                    }
                    /// Else, we add a new hit
                    else
                    {
                        hitsCount++;
                        hits.Add(hit);
                        hit = queryString.Item2;

                        /// Too much hits
                        if (hitsCount >= HITMAXCOUNT)
                        {
                            break;
                        }
                    }
                }
                /// We add the current working hit
                hits.Add(hit);

                /// if hits count > 1, we have sliced a hit
                if (hitsCount > 1)
                {
                    string mhIdSuffix = MhIdSuffixGenerator();

                    for (int i = 0; i < hitsCount; i++)
                    {
                        if (i == (HITMAXCOUNT - 1))
                        {
                            // Too much hits count
                            if (tracker.Delegate != null)
                            {
                                tracker.Delegate.WarningDidOccur("Multihits: too much hit parts");
                            }
                            hits[i] = config + MakeSubQuery("mherr", "1");
                        }
                        else
                        {
                            hits[i] = config + MakeSubQuery("mh", String.Format("{0}-{1}-{2}", i + 1, hitsCount, mhIdSuffix)) + MakeSubQuery("idclient", idClient) + hits[i];
                        }
                    }
                }
                /// Only one hit
                else
                {
                    hits[0] = config + hits[0];
                }

                if (tracker.Delegate != null)
                {
                    string message = string.Empty;
                    foreach (string s in hits)
                    {
                        message += s + "\n";
                    }
                    tracker.Delegate.BuildDidEnd(HitStatus.Success, message);
                }
            }

            return new object[] { hits, oltParam };
        }

        /// <summary>
        /// Organize the parameters
        /// </summary>
        /// <param name="completeBuffer"></param>
        /// <returns></returns>
        internal List<Param> OrganizeParameters(List<Param> completeBuffer)
        {
            List<Param> parms = new List<Param>();
            bool findLast = false;
            bool findFirst = false;
            Param lastParam = null;

            Param referrer = null;
            List<int[]> refParamPositions = Tool.FindParameterPositions("ref", completeBuffer);
            int indexRef = refParamPositions.Count() == 0 ? -1 : refParamPositions[refParamPositions.Count() - 1][1];

            if (indexRef != -1)
            {
                referrer = completeBuffer[indexRef];
                completeBuffer.RemoveAt(indexRef);
                if (referrer.paramOptions != null &&
                        referrer.paramOptions.RelativePosition != RelativePosition.Last &&
                       referrer.paramOptions.RelativePosition != RelativePosition.None)
                {
                    if (tracker.Delegate != null)
                    {
                        tracker.Delegate.WarningDidOccur("ref= parameter will be put in last position");
                    }
                }
            }

            List<int[]> indexes;
            int position;

            foreach (Param p in completeBuffer)
            {
                ParamOption options = p.paramOptions;

                if (options != null)
                {
                    switch (options.RelativePosition)
                    {
                        case RelativePosition.First:
                            if (!findFirst)
                            {
                                parms.Insert(0, p);
                                findFirst = true;
                            }
                            else
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Found more than one parameter with relative position set to first");
                                }
                                parms.Add(p);
                            }
                            break;
                        case RelativePosition.Last:
                            if (!findLast)
                            {
                                lastParam = p;
                                findLast = true;
                            }
                            else
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Found more than one parameter with relative position set to last");
                                }
                                parms.Add(p);
                            }
                            break;
                        case RelativePosition.Before:
                            indexes = Tool.FindParameterPositions(options.RelativeParameterKey, parms);
                            position = indexes.Count() == 0 ? -1 : indexes[indexes.Count() - 1][1];
                            if (position == -1)
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Relative param with key " + options.RelativeParameterKey + " could not be found. Param will be appended");
                                }
                                parms.Add(p);
                            }
                            else
                            {
                                parms.Insert(position, p);
                            }
                            break;
                        case RelativePosition.After:
                            indexes = Tool.FindParameterPositions(options.RelativeParameterKey, parms);
                            position = indexes.Count() == 0 ? -1 : indexes[indexes.Count() - 1][1];
                            if (position == -1)
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Relative param with key " + options.RelativeParameterKey + " could not be found. Param will be appended");
                                }
                                parms.Add(p);
                            }
                            else
                            {
                                parms.Insert(position + 1, p);
                            }
                            break;
                        default:
                            parms.Add(p);
                            break;
                    }
                }
                else
                {
                    parms.Add(p);
                }
            }

            if (lastParam != null)
            {
                parms.Add(lastParam);
            }

            if (referrer != null)
            {
                parms.Add(referrer);
            }

            return parms;
        }

        /// <summary>
        /// Prepare the hit
        /// </summary>
        /// <returns></returns>
        internal List<Tuple<Param, string>> PrepareQuery()
        {
            List<Tuple<Param, string>> paramList = new List<Tuple<Param, string>>();

            List<Param> completeBuffer = new List<Param>();
            completeBuffer.AddRange(persistentParameters);
            completeBuffer.AddRange(volatileParameters);

            completeBuffer = OrganizeParameters(completeBuffer);

            foreach (Param p in completeBuffer)
            {
                string value = p.value();
                string key = p.key;

                if (key.Equals("idclient"))
                {
                    if (TechnicalContext.DoNotTrack)
                    {
                        value = "opt-out";
                    }
                    else if (Boolean.Parse(tracker.configuration.parameters["hashUserId"]))
                    {
                        value = Tool.SHA_256(value);
                    }
                }

                // Referrer processing
                if (key.Equals("ref"))
                {
                    value = value.Replace("&", "$")
                            .Replace("<", "")
                            .Replace(">", "");
                }

                if (p.type == Param.Type.Closure && Tool.IsValidJSON(value))
                {
                    p.type = Param.Type.JSON;
                }

                if (p.paramOptions != null && p.paramOptions.Encode)
                {
                    value = Uri.EscapeDataString(value);
                    p.paramOptions.Separator = Uri.UnescapeDataString(p.paramOptions.Separator);
                }

                int duplicateParamIndex = -1;

                for (int i = 0; i < paramList.Count(); i++)
                {
                    if (paramList[i].Item1.key.Equals(key))
                    {
                        duplicateParamIndex = i;
                        break;
                    }
                }

                if (duplicateParamIndex > -1)
                {
                    Tuple<Param, string> duplicateParam = paramList[duplicateParamIndex];

                    /// If parameter's value is JSON 
                    if (p.type == Param.Type.JSON)
                    {
                        /// Parse string to JSON Object
                        object json = Tool.ParseJSON(Uri.UnescapeDataString(duplicateParam.Item2.Split('=')[1]));
                        object newJson = Tool.ParseJSON(Uri.UnescapeDataString(value));

                        if (json != null && json is JsonObject)
                        {
                            if (newJson != null && newJson is JsonObject)
                            {
                                JsonObject firstData = json as JsonObject;
                                JsonObject secondData = newJson as JsonObject;

                                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                                foreach (string jsonKey in firstData.Keys)
                                {
                                    dictionary.Add(jsonKey, firstData[jsonKey]);
                                }
                                foreach (string jsonKey in secondData.Keys)
                                {
                                    dictionary.Add(jsonKey, secondData[jsonKey]);
                                }
                                string strJsonData = Tool.ConvertToString(dictionary);
                                paramList[duplicateParamIndex] = Tuple.Create(duplicateParam.Item1, MakeSubQuery(key, Uri.EscapeUriString(strJsonData)));
                            }
                            else
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Couldn't append value to a dictionnary");
                                }
                            }
                        }
                        else if (json != null && json is JsonArray)
                        {
                            if (newJson != null && newJson is JsonArray)
                            {
                                List<object> array = new List<object>();
                                array.AddRange((JsonArray)json);
                                array.AddRange((JsonArray)newJson);

                                string strJsonData = Tool.ConvertToString(array);
                                paramList[duplicateParamIndex] = Tuple.Create(duplicateParam.Item1, MakeSubQuery(key, Uri.EscapeUriString(strJsonData)));
                            }
                            else
                            {
                                if (tracker.Delegate != null)
                                {
                                    tracker.Delegate.WarningDidOccur("Couldn't append value to an array");
                                }
                            }
                        }
                        else
                        {
                            if (tracker.Delegate != null)
                            {
                                tracker.Delegate.WarningDidOccur("Couldn't append value a JSON");
                            }
                        }
                    }
                    else
                    {
                        if (duplicateParam.Item1.type == Param.Type.JSON)
                        {
                            if (tracker.Delegate != null)
                            {
                                tracker.Delegate.WarningDidOccur("Couldn't append a JSON object");
                            }
                        }
                        else
                        {
                            string separator = ",";

                            if (p.paramOptions != null)
                            {
                                separator = p.paramOptions.Separator;
                            }

                            paramList[duplicateParamIndex] = Tuple.Create(duplicateParam.Item1, duplicateParam.Item2 + separator + value);
                        }
                    }
                }
                else
                {
                    paramList.Add(Tuple.Create(p, MakeSubQuery(key, value)));
                }
            }


            return paramList;
        }

        /// <summary>
        /// Generate mhId suffix
        /// </summary>
        /// <returns></returns>
        private string MhIdSuffixGenerator()
        {
            Random r = new Random();
            int randId = r.Next(9000000) + 1000000;

            DateTimeOffset time = DateTimeOffset.Now;
            string hour = time.Hour.ToString("00");
            string min = time.Minute.ToString("00");
            string secs = time.Second.ToString("00");

            return hour + min + secs + randId;
        }

        /// <summary>
        /// Builds querystring parameters
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string MakeSubQuery(string key, string value)
        {
            return "&" + key + "=" + value;
        }

        void Runnable.Run()
        {
            object[] buildResult = Build();
            List<string> hits = (List<string>)buildResult[0];

            foreach (string hit in hits)
            {
                new Sender(tracker, new Hit(hit), false, (string)buildResult[1]).Send(true);
            }
        }

        #endregion
    }
}
