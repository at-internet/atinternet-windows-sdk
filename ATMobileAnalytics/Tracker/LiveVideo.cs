using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region LiveAudio
    public class LiveVideo : RichMedia
    {
        internal LiveVideo(MediaPlayer player) : base(player)
        {
            broadcastMode = BroadcastMode.Live;
            type = "video";
        }
    }

    #endregion

    #region LiveAudios

    public class LiveVideos
    {
        #region Members

        internal List<LiveVideo> list { get; set; }
        internal MediaPlayer mediaPlayer { get; set; }

        #endregion

        #region Constructor

        internal LiveVideos(MediaPlayer player)
        {
            mediaPlayer = player;
            list = new List<LiveVideo>();
        }

        #endregion

        #region Methods

        public LiveVideo Add(string name)
        {
            LiveVideo lv = list.Find(a => a.Name.Equals(name));
            if(lv == null)
            {
                lv = new LiveVideo(mediaPlayer);
                lv.Name = name;
                list.Add(lv);
            }
            else
            {
                if(mediaPlayer.tracker.Delegate != null)
                {
                    mediaPlayer.tracker.Delegate.WarningDidOccur("LiveAudio with the same name already exists");
                }
            }
            return lv;
        }

        public LiveVideo Add(string name, string chapter1)
        {
            LiveVideo lv = Add(name);
            lv.Chapter1 = chapter1;
            return lv;
        }

        public LiveVideo Add(string name, string chapter1, string chapter2)
        {
            LiveVideo lv = Add(name, chapter1);
            lv.Chapter2 = chapter2;
            return lv;
        }

        public LiveVideo Add(string name, string chapter1, string chapter2, string chapter3, int duration)
        {
            LiveVideo lv = Add(name, chapter1, chapter2);
            lv.Chapter3 = chapter3;
            return lv;
        }

        public void Remove(string name)
        {
            LiveVideo lv = list.Find(a => a.Name.Equals(name));
            if(lv != null)
            {
                if(lv.threadPoolTimer != null)
                {
                    lv.SendStop();
                }
                list.Remove(lv);
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
