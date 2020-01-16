using System;
using Windows.Networking.Connectivity;

namespace ATInternet
{
    #region Reachability
    /// <summary>
    /// Class used to check network availability
    /// </summary>
    internal class Reachability
    {
        #region NetworkType
        /// <summary>
        /// Network type
        /// </summary>
        internal enum NetworkType
        {
            /// <summary>
            /// type wifi
            /// </summary>
            WIFI,
            /// <summary>
            /// type ethernet
            /// </summary>
            ETHERNET,
            /// <summary>
            /// offline
            /// </summary>
            OFFLINE,
            /// <summary>
            /// edge
            /// </summary>
            EDGE,
            /// <summary>
            /// gprs
            /// </summary>
            GPRS,
            /// <summary>
            /// 2g
            /// </summary>
            TWOG,
            /// <summary>
            /// 3g
            /// </summary>
            THREEG,
            /// <summary>
            /// 3g+
            /// </summary>
            THREEGPLUS,
            /// <summary>
            /// 4g
            /// </summary>
            FOURG,
            /// <summary>
            /// Other types (see http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib )
            /// </summary>
            UNKNOWN
        }

        #endregion


        #region Methods
        /// <summary>
        /// Get network type stringified
        /// </summary>
        /// <returns></returns>
        internal static string GetConnectionString()
        {
            switch (GetActiveNetworkType())
            {
                case Reachability.NetworkType.EDGE: return "edge";
                case Reachability.NetworkType.GPRS: return "gprs";
                case Reachability.NetworkType.WIFI: return "wifi";
                case Reachability.NetworkType.TWOG: return "2g";
                case Reachability.NetworkType.THREEG: return "3g";
                case Reachability.NetworkType.THREEGPLUS: return "3g+";
                case Reachability.NetworkType.FOURG: return "4g";
                case Reachability.NetworkType.ETHERNET: return "ethernet";
                case Reachability.NetworkType.UNKNOWN: return "unknown";
                default: return "offline";
            }
        }

        /// <summary>
        /// get active network type
        /// </summary>
        internal static NetworkType GetActiveNetworkType()
        {
            try
            {
                ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile != null)
                {
					//if Ethernet
					if(profile.NetworkAdapter.IanaInterfaceType == 6)
                    {
                        return Reachability.NetworkType.ETHERNET;
                    }
					// else if Wifi
                    else if (profile.NetworkAdapter.IanaInterfaceType == 71)
                    {
                        return Reachability.NetworkType.WIFI;
                    }
                    // else : mobile
                    else
                    {
                        if (profile.IsWwanConnectionProfile)
                        {
                            WwanConnectionProfileDetails details = profile.WwanConnectionProfileDetails;
							if(details != null)
                            {
                                switch (details.GetCurrentDataClass())
                                {
                                    case WwanDataClass.None:
                                        return Reachability.NetworkType.OFFLINE;
                                    case WwanDataClass.Edge:
                                        return Reachability.NetworkType.EDGE;
                                    case WwanDataClass.Gprs:
                                        return Reachability.NetworkType.GPRS;
                                    case WwanDataClass.Cdma1xRtt:
                                        return Reachability.NetworkType.TWOG;
                                    case WwanDataClass.Cdma1xEvdo:
                                    case WwanDataClass.Cdma1xEvdoRevA:
                                    case WwanDataClass.Cdma1xEvdoRevB:
                                    case WwanDataClass.Cdma1xEvdv:
                                    case WwanDataClass.Cdma3xRtt:
                                        return Reachability.NetworkType.THREEG;
                                    case WwanDataClass.Hsdpa:
                                    case WwanDataClass.Hsupa:
                                        return Reachability.NetworkType.THREEGPLUS;
                                    case WwanDataClass.LteAdvanced:
                                        return Reachability.NetworkType.FOURG;
                                    default:
                                        return Reachability.NetworkType.UNKNOWN;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Reachability.NetworkType.OFFLINE;
        }

        #endregion
    }

    #endregion
}
