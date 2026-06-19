using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal class Artist
    {
        public string Name { get; set; }
        // changed properties to to use auto-initialization 
        // it is made to avoid null reference exceptions when adding songs and albums to an artist
        // also some artist might not have any songs or albums, so it is better to have empty lists instead of null
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();

        // here making two constructor to make it flexible for creating artists with or without albums
        public Artist(string name)
        {
            this.Name = name;
        }
        public Artist(string name, List<Album> albums, List<Song> songs)
        {
            this.Name = name;
            this.Albums = albums ?? new List<Album>();
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void AddAlbum(Album album)
        {
            Albums.Add(album);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
