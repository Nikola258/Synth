using System.Collections.Generic;

namespace Synth
{
    // Album extends SongCollection
    // adds an Artists list on top of the base Title and Playables
    internal class Album : SongCollection
    {
        public List<Artist> Artists { get; private set; }

        // Songs is a convenience property, it reads from the base Playables list and casts each item to Song,
        // this makes it easy to loop over songs
        public List<Song> Songs
        {
            get
            {
                var songs = new List<Song>();
                foreach (var item in Playables)
                {
                    if (item is Song song)
                        songs.Add(song);
                }
                return songs;
            }
        }

        public Album(List<Artist> artists, string title, List<Song> songs) : base(title)
        {
            Artists = artists;

            // add each song into the base Playables list
            foreach (var song in songs)
            {
                Playables.Add(song);
            }
        }

        public override string ToString()
        {
            return $"{Title} - {string.Join(", ", Artists)}";
        }
    }
}