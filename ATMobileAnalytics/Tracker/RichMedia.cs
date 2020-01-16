using System;
using Windows.System.Threading;

namespace ATInternet
{

    #region BroadcastMode

    public enum BroadcastMode
    {
        Clip, Live
    }

    #endregion

    #region RichMediaAction

    public enum RichMediaAction
    {
        Play, Pause, Stop, Move, Refresh
    }

    #endregion

    #region RichMedia
    public class RichMedia : BusinessObject
    {
        protected const int MAX_DURATION = 86400;

        #region Members

        internal MediaPlayer mediaPlayer { get; set; }
        internal BroadcastMode broadcastMode { get; set; }
        internal string type{ get; set; }

        public RichMediaAction Action { get; set; }
        public bool IsBuffering { get; set; }
        public bool IsEmbedded { get; set; }
        public string Name { get; set; }
        public string Chapter1 { get; set; }
        public string Chapter2 { get; set; }
        public string Chapter3 { get; set; }
        public int Level2 { get; set; }
        public int RefreshDuration { get; set; }
        public string WebDomain { get; set; }
        internal ThreadPoolTimer threadPoolTimer { get; set; }
        #endregion

        #region Constructor

        internal RichMedia(MediaPlayer player) : base(player.tracker)
        {
            mediaPlayer = player;
            Name = string.Empty;
            Level2 = -1;
            RefreshDuration = 0;
            IsBuffering = false;
            IsEmbedded = false;
        }

        #endregion

        #region Methods

        internal string BuildMediaName()
        {
            string mediaName = Chapter1 == null ? string.Empty : Chapter1 + "::";
            mediaName += Chapter2 == null ? string.Empty : Chapter2 + "::";
            mediaName += Chapter3 == null ? string.Empty : Chapter3 + "::";

            return mediaName += Name;
        }

        public void SendPlay(int refreshDuration)
        {
            if(refreshDuration > 0)
            {
                if(refreshDuration < 5)
                {
                    refreshDuration = 5;
                }

                if(threadPoolTimer == null)
                {
                    threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                    {
                        Action = RichMediaAction.Refresh;
                        tracker.dispatcher.Dispatch(this);
                    },
                TimeSpan.FromSeconds(refreshDuration));
                }
            }
            Action = RichMediaAction.Play;
            tracker.dispatcher.Dispatch(this);
        }

        public void SendPlay()
        {
            SendPlay(5);
        }

        public void SendPause()
        {
            Action = RichMediaAction.Pause;
            if(threadPoolTimer != null)
            {
                threadPoolTimer.Cancel();
                threadPoolTimer = null;
            }

            tracker.dispatcher.Dispatch(this);
        }

        public void SendStop()
        {
            Action = RichMediaAction.Stop;
            if (threadPoolTimer != null)
            {
                threadPoolTimer.Cancel();
                threadPoolTimer = null;
            }

            tracker.dispatcher.Dispatch(this);
        }
        public void SendMove()
        {
            Action = RichMediaAction.Move;
            tracker.dispatcher.Dispatch(this);
        }

        internal override void SetEvent()
        {
            ParamOption encode = new ParamOption() { Encode = true };

            tracker.SetParam("type", type)
                    .SetParam("p", BuildMediaName(), encode)
                    .SetParam("a", Action.ToString().ToLower())
                    .SetParam("m6", broadcastMode.ToString().ToLower())
                    .SetParam("plyr", mediaPlayer.PlayerId)
                    .SetParam("m5", IsEmbedded ? "ext" : "int");

            if (Level2 > 0)
            {
                tracker.SetParam("s2", Level2);
            }

            if (Action == RichMediaAction.Play)
            {
                if (IsBuffering)
                {
                    tracker.SetParam("buf", 1);
                }

                if (IsEmbedded && WebDomain != null)
                {
                    tracker.SetParam("m9", WebDomain);
                }

                if (!IsEmbedded)
                {
                    if (!string.IsNullOrEmpty(TechnicalContext.ScreenName))
                    {
                        tracker.SetParam("prich", TechnicalContext.ScreenName, encode);
                    }

                    if (TechnicalContext.Level2 > 0)
                    {
                        tracker.SetParam("s2rich", TechnicalContext.Level2);
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}
