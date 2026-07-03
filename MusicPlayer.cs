using System;
using WMPLib;

namespace Synth
{
    // MusicPlayer is responsible for ALL audio playback.
    // it wraps WMPLib so the rest of the app never has to touch it directly.
    // Client just calls MusicPlayer.Play(song), .Pause(), .Next()
    internal class MusicPlayer
    {
        private readonly WindowsMediaPlayer _wmp;

        // song that is currently loaded
        public Song CurrentSong { get; private set; }

        // tracks whether playback is paused
        public bool IsPaused { get; private set; }

        public MusicPlayer()
        {
            _wmp = new WindowsMediaPlayer();
        }

        // load a new song and start playing it immediately
        public void Play(Song song)
        {
            CurrentSong = song;
            IsPaused = false;
            _wmp.URL = song.FilePath;
            _wmp.controls.play();
        }

        // pause the current song
        public void Pause()
        {
            if (CurrentSong == null) return;
            _wmp.controls.pause();
            IsPaused = true;
        }

        // resume after a pause
        public void Resume()
        {
            if (CurrentSong == null) return;
            _wmp.controls.play();
            IsPaused = false;
        }

        // stop playback entirely
        public void Stop()
        {
            _wmp.controls.stop();
            IsPaused = false;
        }

        // returns a formatted string like "00:01:23 / 00:03:45"
        public string GetTimeDisplay()
        {
            if (CurrentSong == null) return "";

            double current = _wmp.controls.currentPosition;
            string currentStr = TimeSpan.FromSeconds(current).ToString(@"hh\:mm\:ss");
            string totalStr = TimeSpan.FromSeconds(CurrentSong.Duration).ToString(@"hh\:mm\:ss");

            return $"{currentStr} / {totalStr}";
        }
    }
}