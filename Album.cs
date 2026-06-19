using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Album
    {
        private List<Artist> Artists;
        public string Title { get; private set; }

        
        public Album(List<Artist> artist, string title, List<Song> songs) {

            this.Artists = artist;
            this.Title = title;
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
