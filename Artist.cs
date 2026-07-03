using System.Collections.Generic;

namespace Synth
{
    internal class Artist
    {
        public string Name { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();

        public Artist(string name)
        {
            Name = name;
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void AddAlbum(Album album)
        {
            Albums.Add(album);
        }

        // returns only the artist names, used in ToString() of Song
        public override string ToString()
        {
            return Name;
        }
    }
}