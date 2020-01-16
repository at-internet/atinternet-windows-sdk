using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region Video
    public class Video : RichMedia
    {
        #region Members
        public int Duration { get; set; }

        #endregion

        #region Constructor

        internal Video(MediaPlayer player) : base(player)
        {
            broadcastMode = BroadcastMode.Clip;
            type = "video";
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

    #region Videos

    public class Videos
    {
        #region Members

        internal List<Video> list { get; set; }
        internal MediaPlayer mediaPlayer { get; set; }

        #endregion

        #region Constructor

        internal Videos(MediaPlayer player)
        {
            this.mediaPlayer = player;
            list = new List<Video>();
        }

        #endregion

        #region Methods

        public Video Add(string name, int duration)
        {
            Video video = list.Find(v => v.Name.Equals(name));
            if(video == null)
            {
                video = new Video(mediaPlayer);
                video.Name = name;
                video.Duration = duration;
                list.Add(video);
            }
            else
            {
                if(mediaPlayer.tracker.Delegate != null)
                {
                    mediaPlayer.tracker.Delegate.WarningDidOccur("Video with the same name already exists");
                }
            }
            return video;
        }

        public Video Add(string name, string chapter1, int duration)
        {
            Video v = Add(name, duration);
            v.Chapter1 = chapter1;
            return v;
        }

        public Video Add(string name, string chapter1, string chapter2, int duration)
        {
            Video v = Add(name, chapter1, duration);
            v.Chapter2 = chapter2;
            return v;
        }

        public Video Add(string name, string chapter1, string chapter2, string chapter3, int duration)
        {
            Video v = Add(name, chapter1, chapter2, duration);
            v.Chapter3 = chapter3;
            return v;
        }

        public void Remove(string name)
        {
            Video video = list.Find(v => v.Name.Equals(name));
            if(video != null)
            {
                if(video.threadPoolTimer != null)
                {
                    video.SendStop();
                }
                list.Remove(video);
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
