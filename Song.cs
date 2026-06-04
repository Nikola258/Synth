using System;
using System.Collections.Generic;
using System.Text;
using WMPLib;

namespace Synth
{
    internal class Song : IPlayable
    {
        public string Title { get; set; }
        public List<Artist> Artists { get; set; }
        public Genres SongGenre { get; set; }
        public int Duration { get; set; }
        public string FilePath { get; set; }

        // for iplayable
        public int Length => Duration;
        // for each song to manage
        private WindowsMediaPlayer _player;


        public Song(string title, List<Artist> artists, int duration, Genres genre, string filePath = "")
        {
            this.Title = title;
            this.Artists = artists;
            this.Duration = duration;
            this.SongGenre = genre;
            this.FilePath = filePath;
            _player = new WindowsMediaPlayer();
        }

        public void Play()
        {
            if(string.IsNullOrEmpty(this.FilePath))
            {
                Console.WriteLine($"[Error] Cannot play '{Title}' - file path is missing.");
                return;
            }
            _player.URL = this.FilePath;
            _player.controls.play();
            Console.WriteLine($"Playing: {Title} by {string.Join(", ", Artists)}");
        }
        public void Pause() { }
        public void Stop() { }
        public void Next() { }

        public override string ToString()
        {
            return $"{Title} - {string.Join(", ", Artists)} - {SongGenre}";
        }
    }
}
