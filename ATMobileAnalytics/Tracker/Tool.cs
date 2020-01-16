using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Json;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ATInternet
{
    #region Tool
    internal class Tool
    {
        /// <summary>
        ///  Find parameter positions
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal static List<int[]> FindParameterPositions(string searchKey, params List<Param>[] parameters)
        {
            List<int[]> indexes = new List<int[]>();
            int indexValue = 0;
            int idArray = 0;

            foreach(List<Param> array in parameters)
            {
                foreach(Param p in array)
                {
                    if (p.key.Equals(searchKey))
                    {
                        indexes.Add(new int[] { idArray, indexValue });
                    }
                    indexValue++;
                }
                idArray++;
                indexValue = 0;
            }

            return indexes;
        }

        /// <summary>
        /// Apply sha 256 on value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string SHA_256(string value)
        {
            IBuffer buffUtf8Str = CryptographicBuffer.ConvertStringToBinary("AT" + value, BinaryStringEncoding.Utf8);
            IBuffer buffHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256).HashData(buffUtf8Str);
            return CryptographicBuffer.EncodeToHexString(buffHash);
        }

        /// <summary>
        /// Check if value is a valid json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsValidJSON(string value)
        {
            return ParseJSON(value) != null;
        }

        /// <summary>
        /// Parse json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static JsonObject ParseJSON(string value)
        {
            try
            {
                return JsonObject.Parse(value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        internal static string ConvertToString(object value, string separator = ",")
        {
            string result = string.Empty;

            if(value != null)
            {
                if (value is IEnumerable<object>)
                {
                    IEnumerable<object> listResult = (IEnumerable<object>)value;
                    bool isFirst = true;
                    foreach (object obj in listResult)
                    {
                        value = obj;
                        if (isFirst)
                        {
                            result = ConvertToString(value, separator);
                            isFirst = false;
                        }
                        else
                        {
                            result += separator + ConvertToString(value, separator);
                        }
                    }
                }
                else if (value is Dictionary<string, object>)
                {
                    var entries = ((Dictionary<string, object>)value).Select(d =>
                        string.Format("\"{0}\":{1}", d.Key, d.Value is string ? "\"" + ConvertToString(d.Value) + "\"" : ConvertToString(d.Value)));
                    result =  "{" + string.Join(",", entries) + "}";
                }
                else 
                {
                    result = value.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Get the timestamp when the hit was created
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        internal static string GetTimeStampFromHit(string hit)
        {
            return hit.Split(new string[] { "&ts=" }, StringSplitOptions.None)[1].Split('&')[0];
        }

        /// <summary>
        /// Append values parameter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="volatileParameters"></param>
        /// <param name="persistentParameters"></param>
        /// <returns></returns>
        internal static string AppendParameterValues(string key, List<Param> volatileParameters, List<Param> persistentParameters)
        {
            List<int[]> indexPositions = Tool.FindParameterPositions(key, volatileParameters, persistentParameters);
            bool isFirst = true;
            string result = string.Empty;

            foreach(int[] index in indexPositions)
            {
                Param p = index[0] == 0 ? volatileParameters[index[1]] : persistentParameters[index[1]];
                if (isFirst)
                {
                    result = p.value();
                    isFirst = false;
                }
                else if (p.paramOptions != null)
                {
                    result += p.paramOptions.Separator + p.value();
                }
                else
                {
                    result += "," + p.value();
                }
            }

            return result;
        }
    }

    #endregion
}
