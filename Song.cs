using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Song
    {
        public string Title { get; set; }
        public List<Artist> Artists { get; set; }
        public Genres SongGenre { get; set; }
        public int Duration { get; set; }

        public Song(string title, List<Artist> artists, int duration, Genres genre)
        {
            this.Title = title;
            this.Artists = artists;
            this.Duration = duration;
            this.SongGenre = genre;
        }

        public override string ToString()
        {
            return $"{Title} - {string.Join(", ", Artists)}";
        }
    }
}
