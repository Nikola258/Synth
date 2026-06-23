using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Album
    {
        private List<Artist> Artists;
        public string Title { get; private set; }
        public List<Song> Songs;

        
        public Album(List<Artist> artist, string title, List<Song> songs) {

            this.Artists = artist;
            this.Title = title;
            this.Songs = songs;
        }

        public List<Artist> ShowArtists()
        {
            return Artists;
        }

        public override string ToString()
        {
            return $"{Title} - {string.Join(", ", Artists)}";
        }
    }
}
