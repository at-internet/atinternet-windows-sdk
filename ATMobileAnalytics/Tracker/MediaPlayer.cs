using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region MediaPlayer
    public class MediaPlayer
    {
        #region Members

        internal Tracker tracker;

        public int PlayerId { get; set; }

        private Videos _videos;
        public Videos Videos { get { return _videos ?? (_videos = new Videos(this)); } }

        private Audios _audios;
        public Audios Audios { get { return _audios ?? (_audios = new Audios(this)); } }

        private LiveVideos _liveVideos;
        public LiveVideos LiveVideos { get { return _liveVideos ?? (_liveVideos = new LiveVideos(this)); } }

        private LiveAudios _liveAudios;
        public LiveAudios LiveAudios { get { return _liveAudios ?? (_liveAudios = new LiveAudios(this)); } }

        #endregion

        #region Constructor

        internal MediaPlayer(Tracker tracker)
        {
            this.tracker = tracker;
            PlayerId = 1;
        }

        #endregion
    }

    #endregion

    #region MediaPlayers

    public class MediaPlayers
    {
        #region Members

        private Tracker tracker { get; set; }

        internal Dictionary<int, MediaPlayer> players = new Dictionary<int, MediaPlayer>();

        #endregion

        #region Constructor

        internal MediaPlayers(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods

        public MediaPlayer Add()
        {
            MediaPlayer mp = new MediaPlayer(tracker);
            if(players.Count > 0)
            {
                mp.PlayerId = players.Keys.Max() + 1;
            }
            else
            {
                mp.PlayerId = 1;
            }

            players.Add(mp.PlayerId, mp);

            return mp;
        }

        public MediaPlayer Add(int playerId)
        {
            MediaPlayer mp;
            if (!players.ContainsKey(playerId))
            {
                mp = new MediaPlayer(tracker);
                mp.PlayerId = playerId;

                players.Add(mp.PlayerId, mp);
            }
            else
            {
                if(tracker.Delegate != null)
                {
                    tracker.Delegate.WarningDidOccur("Player with the same id already exists");
                }

                mp = players[playerId];
            }
            return mp;
        }

        public void Remove(int playerId)
        {
            MediaPlayer mp = players.ContainsKey(playerId) ? players[playerId] : null;
            if(mp != null)
            {
                foreach (Video v in mp.Videos.list)
                {
                    if (v.threadPoolTimer != null)
                    {
                        v.SendStop();
                    }
                }
                mp.Videos.RemoveAll();
                foreach (Audio a in mp.Audios.list)
                {
                    if (a.threadPoolTimer != null)
                    {
                        a.SendStop();
                    }
                }
                mp.Audios.RemoveAll();
                foreach (LiveVideo lv in mp.LiveVideos.list)
                {
                    if (lv.threadPoolTimer != null)
                    {
                        lv.SendStop();
                    }
                }
                mp.LiveVideos.RemoveAll();
                foreach (LiveAudio la in mp.LiveAudios.list)
                {
                    if (la.threadPoolTimer != null)
                    {
                        la.SendStop();
                    }
                }
                mp.LiveAudios.RemoveAll();
            }
            players.Remove(playerId);
        }

        public void RemoveAll()
        {
            while(players.Count > 0)
            {
                Remove(players.First().Value.PlayerId);
            }
        }

        #endregion
    }

    #endregion
}
