using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Artist
    {
        public string Name { get; set; }
        private List<Song> Songs { get; set; }
        // TODO: add albums property once Album class

        public Artist(string name)
        {
            this.Name = name;
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void AddAlbum()
        {
            // TODO: add album logic later
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
