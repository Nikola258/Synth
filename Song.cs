using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Song : IPlayable
    {
        public string Title { get; set; }
        public List<Artist> Artists { get; set; }
        public Genres SongGenre { get; set; }
        public double Duration { get; set; }
        public string FilePath { get; set; }

        // for iplayable
        public double Length => Duration;


        public Song(string title, List<Artist> artists, double duration, Genres genre, string filePath = "")
        {
            this.Title = title;
            this.Artists = artists;
            this.Duration = duration;
            this.SongGenre = genre;
            this.FilePath = filePath;
        }

        public void Play() { }
        public void Pause() { }
        public void Stop() { }
        public void Next() { }

        public override string ToString()
        {
            return $"{Title} - {string.Join(", ", Artists)} - {SongGenre}";
        }
    }
}
