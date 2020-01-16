using System;
using System.Diagnostics;

namespace ATInternet
{
    public class DefaultTrackerDelegate : TrackerDelegate
    {
        public void BuildDidEnd(HitStatus status, string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Building Hit \n\tStatus: " + status.ToString() + "\n\tMessage: " + message);
        }

        public void DidCallPartner(string response)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Calling Partner \n\tResponse: " + response);
        }

        public void ErrorDidOccur(string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Error \n\tMessage: " + message);
        }

        public void SaveDidEnd(string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Saving Hit \n\tMessage: " + message);
        }

        public void SendDidEnd(HitStatus status, string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Sending Hit \n\tStatus: " + status.ToString() + "\n\tMessage: " + message);
        }

        public void TrackerNeedsFirstLaunchApproval(string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: First Launch \n\tMessage: " + message);
        }

        public void WarningDidOccur(string message)
        {
            Debug.WriteLine("AT INTERNET Debugging message: \n\tEvent: Warning \n\tMessage: " + message);
        }
    }
}
