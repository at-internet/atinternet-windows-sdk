using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region Audio
    public class Audio : RichMedia
    {
        #region Members
        public int Duration { get; set; }

        #endregion

        #region Constructor

        internal Audio(MediaPlayer player) : base(player)
        {
            broadcastMode = BroadcastMode.Clip;
            type = "audio";
        }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            base.SetEvent();
            if(Duration > MAX_DURATION)
            {
                Duration = MAX_DURATION;
            }

            tracker.SetParam("m1", Duration);
        }

        #endregion
    }

    #endregion

    #region Audios

    public class Audios
    {
        #region Members

        internal List<Audio> list { get; set; }
        internal MediaPlayer mediaPlayer { get; set; }

        #endregion

        #region Constructor

        internal Audios(MediaPlayer player)
        {
            this.mediaPlayer = player;
            list = new List<Audio>();
        }

        #endregion

        #region Methods

        public Audio Add(string name, int duration)
        {
            Audio audio = list.Find(a => a.Name.Equals(name));
            if(audio == null)
            {
                audio = new Audio(mediaPlayer);
                audio.Name = name;
                audio.Duration = duration;
                list.Add(audio);
            }
            else
            {
                if(mediaPlayer.tracker.Delegate != null)
                {
                    mediaPlayer.tracker.Delegate.WarningDidOccur("Audio with the same name already exists");
                }
            }
            return audio;
        }

        public Audio Add(string name, string chapter1, int duration)
        {
            Audio a = Add(name, duration);
            a.Chapter1 = chapter1;
            return a;
        }

        public Audio Add(string name, string chapter1, string chapter2, int duration)
        {
            Audio a = Add(name, chapter1, duration);
            a.Chapter2 = chapter2;
            return a;
        }

        public Audio Add(string name, string chapter1, string chapter2, string chapter3, int duration)
        {
            Audio a = Add(name, chapter1, chapter2, duration);
            a.Chapter3 = chapter3;
            return a;
        }

        public void Remove(string name)
        {
            Audio audio = list.Find(a => a.Name.Equals(name));
            if(audio != null)
            {
                if(audio.threadPoolTimer != null)
                {
                    audio.SendStop();
                }
                list.Remove(audio);
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
