using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region LiveAudio
    public class LiveAudio : RichMedia
    {
        internal LiveAudio(MediaPlayer player) : base(player)
        {
            broadcastMode = BroadcastMode.Live;
            type = "audio";
        }
    }

    #endregion

    #region LiveAudios

    public class LiveAudios
    {
        #region Members

        internal List<LiveAudio> list { get; set; }
        internal MediaPlayer mediaPlayer { get; set; }

        #endregion

        #region Constructor

        internal LiveAudios(MediaPlayer player)
        {
            mediaPlayer = player;
            list = new List<LiveAudio>();
        }

        #endregion

        #region Methods

        public LiveAudio Add(string name)
        {
            LiveAudio la = list.Find(a => a.Name.Equals(name));
            if(la == null)
            {
                la = new LiveAudio(mediaPlayer);
                la.Name = name;
                list.Add(la);
            }
            else
            {
                if(mediaPlayer.tracker.Delegate != null)
                {
                    mediaPlayer.tracker.Delegate.WarningDidOccur("LiveAudio with the same name already exists");
                }
            }
            return la;
        }

        public LiveAudio Add(string name, string chapter1)
        {
            LiveAudio la = Add(name);
            la.Chapter1 = chapter1;
            return la;
        }

        public LiveAudio Add(string name, string chapter1, string chapter2)
        {
            LiveAudio la = Add(name, chapter1);
            la.Chapter2 = chapter2;
            return la;
        }

        public LiveAudio Add(string name, string chapter1, string chapter2, string chapter3, int duration)
        {
            LiveAudio la = Add(name, chapter1, chapter2);
            la.Chapter3 = chapter3;
            return la;
        }

        public void Remove(string name)
        {
            LiveAudio la = list.Find(a => a.Name.Equals(name));
            if(la != null)
            {
                if(la.threadPoolTimer != null)
                {
                    la.SendStop();
                }
                list.Remove(la);
            }
        }

        public void RemoveAll()
        {
            while(list.Count > 0)
            {
                Remove(list.First().Name);
            }
        }

        #endregion
    }

    #endregion
}
