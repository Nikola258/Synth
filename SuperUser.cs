using System.Collections.Generic;

namespace Synth
{
    // SuperUser is a user who can manage playlists and friends
    // SuperUser inherits from User
    internal class SuperUser : User
    {
        public SuperUser(string name) : base(name) { }

        // --- Friends ---

        public void AddFriend(User user)
        {
            // don't add the same friend twice
            if (!Friends.Contains(user))
                Friends.Add(user);
        }

        public void RemoveFriend(User user)
        {
            Friends.Remove(user);
        }

        // --- Playlists ---

        public Playlist CreatePlayList(string title)
        {
            var playlist = new Playlist(this, title);
            Playlists.Add(playlist);
            return playlist;
        }

        public void RemovePlayList(int index)
        {
            int i = index - 1;
            if (i >= 0 && i < Playlists.Count)
                Playlists.RemoveAt(i);
        }

        // add a song (by its 1-based index in allSongs) to a playlist
        public void AddToPlayList(IPlayable item, Playlist playlist)
        {
            playlist.Add(item);
        }

        // remove a song (by its 1-based index in the playlist) from a playlist
        public void RemoveFromPlayList(int index, Playlist playlist)
        {
            int i = index - 1;
            var items = playlist.ShowPlayables();
            if (i >= 0 && i < items.Count)
                playlist.Remove(items[i]);
        }
    }
}