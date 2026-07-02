using System.Collections.Generic;

namespace Synth
{
    // base user class
    // SuperUser extends this with playlist and friends features
    internal class User
    {
        public string Name { get; set; }
        public List<User> Friends { get; set; } = new List<User>();
        // add playlist stuff later
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();

        public User(string name)
        {
            Name = name;
        }

        // returns the friend list
        public List<User> ShowFriends()
        {
            return Friends;
        }

        // returns the playlist list
        public List<Playlist> ShowPlaylists()
        {
            return Playlists;
        }

        // selects a playlist by 1-based index, returns null if invalid
        public Playlist SelectPlaylist(int index)
        {
            int i = index - 1;
            if (i < 0 || i >= Playlists.Count)
                return null;
            return Playlists[i];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
