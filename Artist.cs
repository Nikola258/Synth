using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Artist
    {
        public string Name { get; set; }
        private List<Song> Songs { get; set; }
        // add albums later

        public Artist(string name, List<Song> songs)
        {
            this.Name = name;
            this.Songs = songs;
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void AddAlbum()
        {
            // add album class later
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
