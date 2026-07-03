using System;
using System.Collections.Generic;

namespace Synth
{
    // Client a controller of the application
    // holds all data (songs, albums, users)
    // does NOT handle WMPLib that is MusicPlayer
    internal class Client
    {
        // --- Data ---
        private List<Song> AllSongs { get; set; }
        private List<Album> AllAlbums { get; set; }
        private List<User> AllUsers { get; set; }

        // user who is currently logged in must be a SuperUser to use playlists/friends
        public SuperUser ActiveUser { get; private set; }

        // song that is selected / currently loaded into the player
        public IPlayable CurrentlyPlaying { get; set; }

        // Playback state
        public bool Playing { get; private set; }
        public bool Shuffle { get; private set; }
        public bool Repeat { get; private set; }

        // dedicated audio player keeps WMPLib out of Client
        private readonly MusicPlayer _musicPlayer;

        private const int ItemsPerPage = 6;

        // Client receives its data via constructor
        public Client(List<User> users, List<Album> albums, List<Song> songs)
        {
            AllUsers = users;
            AllAlbums = albums;
            AllSongs = songs;
            _musicPlayer = new MusicPlayer();
        }

        // -------------------────
        // USER MANAGEMENT
        // -------------------────

        public void SetActiveUser(User user)
        {
            // only SuperUser accounts can use playlists and friends
            if (user is SuperUser superUser)
            {
                ActiveUser = superUser;
                Console.WriteLine($"Logged in as: {superUser.Name}");
            }
            else
            {
                Console.WriteLine("[Error] This user does not have SuperUser access.");
            }
        }

        // all users with a numbered list
        public void ShowAllUsers()
        {
            Console.WriteLine("\n-- Users --");
            for (int i = 0; i < AllUsers.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {AllUsers[i]}");
            }
        }

        // select a user by 1-based index and make them active
        public void SelectUser(int index)
        {
            int i = index - 1;
            if (i < 0 || i >= AllUsers.Count)
            {
                Console.WriteLine("[Error] Invalid user selection.");
                return;
            }
            SetActiveUser(AllUsers[i]);
        }

        // show the playlists
        public void ShowUserPlaylists()
        {
            if (ActiveUser == null)
            {
                Console.WriteLine("[Error] No user logged in.");
                return;
            }

            var playlists = ActiveUser.ShowPlaylists();

            if (playlists.Count == 0)
            {
                Console.WriteLine("You have no playlists yet.");
                return;
            }

            Console.WriteLine($"\n── Playlists of {ActiveUser.Name} ──");
            for (int i = 0; i < playlists.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {playlists[i]}");
            }
        }

        // select one of the active users playlists by 1-based index
        public Playlist SelectUserPlaylist(int index)
        {
            if (ActiveUser == null)
            {
                Console.WriteLine("[Error] No user logged in.");
                return null;
            }

            var playlist = ActiveUser.SelectPlaylist(index);
            if (playlist == null)
                Console.WriteLine("[Error] Invalid playlist selection.");

            return playlist;
        }

        // -------------------────
        // SONG DISPLAY
        // -------------------────

        public void ShowAllSongs(int pageNumber = 1)
        {
            ShowPage(AllSongs, pageNumber, song =>
                $"{song} ({TimeSpan.FromSeconds(song.Duration):hh\\:mm\\:ss})");
        }

        // select a song from AllSongs by 1-based index
        public void SelectSong(int index)
        {
            int i = index - 1;
            if (i < 0 || i >= AllSongs.Count)
            {
                Console.WriteLine("[Error] Invalid song selection.");
                return;
            }
            CurrentlyPlaying = AllSongs[i];
            Console.WriteLine($"Selected: {AllSongs[i]}");
        }

        // -------------------────
        // ALBUM DISPLAY
        // -------------------────

        public void ShowAllAlbums(int pageNumber = 1)
        {
            ShowPage(AllAlbums, pageNumber, album =>
                $"{album} ({album.Songs.Count} songs)");
        }

        // select an album and store it as CurrentlyPlaying collection
        // returns the album so the menu can show its songs
        public Album SelectAlbum(int index)
        {
            int i = index - 1;
            if (i < 0 || i >= AllAlbums.Count)
            {
                Console.WriteLine("[Error] Invalid album selection.");
                return null;
            }
            return AllAlbums[i];
        }

        // show the songs inside a specific album
        public void ShowSongsInAlbum(Album album, int pageNumber = 1)
        {
            if (album == null) return;
            Console.WriteLine($"\n── Songs in \"{album.Title}\" ──");
            ShowPage(album.Songs, pageNumber, song =>
                $"{song} ({TimeSpan.FromSeconds(song.Duration):hh\\:mm\\:ss})");
        }

        // select a song from inside an album by 1-based index
        public void SelectSongFromAlbum(Album album, int index)
        {
            if (album == null) return;
            int i = index - 1;
            var songs = album.Songs;
            if (i < 0 || i >= songs.Count)
            {
                Console.WriteLine("[Error] Invalid song selection.");
                return;
            }
            CurrentlyPlaying = songs[i];
            Console.WriteLine($"Selected: {songs[i]}");
        }

        // -------------------────
        // PLAYBACK  (using MusicPlayer)
        // -------------------────

        public void Play()
        {
            if (CurrentlyPlaying is not Song song)
            {
                Console.WriteLine("[Error] No song selected.");
                return;
            }
            _musicPlayer.Play(song);
            Playing = true;
        }

        public void Pause()
        {
            _musicPlayer.Pause();
            Playing = false;
        }

        public void Resume()
        {
            _musicPlayer.Resume();
            Playing = true;
        }

        public void Stop()
        {
            _musicPlayer.Stop();
            Playing = false;
        }

        // skip to the next song in AllSongs
        public void NextSong()
        {
            if (_musicPlayer.CurrentSong == null) return;

            int index = AllSongs.IndexOf(_musicPlayer.CurrentSong);
            if (index < 0 || index >= AllSongs.Count - 1)
            {
                Console.WriteLine("No next song.");
                return;
            }

            CurrentlyPlaying = AllSongs[index + 1];
            Play();
        }

        public void SetShuffle(bool value) => Shuffle = value;
        public void SetRepeat(bool value) => Repeat = value;

        // returns "00:01:23 / 00:03:45"
        public string GetCurrentSongTime() => _musicPlayer.GetTimeDisplay();

        // lets the now-playing screen know the current song details
        public Song GetCurrentSong() => _musicPlayer.CurrentSong;

        public bool IsPaused => _musicPlayer.IsPaused;

        // -------------------────
        // PLAYLIST MANAGEMENT  (uses SuperUser)
        // -------------------────

        public void CreatePlaylist(string title)
        {
            if (!CheckActiveUser()) return;
            ActiveUser.CreatePlayList(title);
            Console.WriteLine($"Playlist \"{title}\" created.");
        }

        public void ShowPlaylists()
        {
            ShowUserPlaylists();
        }

        public void SelectPlaylist(int index)
        {
            // stored in the calling menu — see Program.cs
            SelectUserPlaylist(index);
        }

        public void RemovePlaylist(int index)
        {
            if (!CheckActiveUser()) return;
            ActiveUser.RemovePlayList(index);
            Console.WriteLine("Playlist removed.");
        }

        // add a song (from AllSongs) to a playlist
        public void AddToPlaylist(int songIndex, Playlist playlist)
        {
            if (!CheckActiveUser()) return;
            int i = songIndex - 1;
            if (i < 0 || i >= AllSongs.Count)
            {
                Console.WriteLine("[Error] Invalid song index.");
                return;
            }
            ActiveUser.AddToPlayList(AllSongs[i], playlist);
            Console.WriteLine($"\"{AllSongs[i].Title}\" added to \"{playlist.Title}\".");
        }

        // remove a song from a playlist by its 1-based position in the playlist
        public void RemoveFromPlaylist(int index, Playlist playlist)
        {
            if (!CheckActiveUser()) return;
            ActiveUser.RemoveFromPlayList(index, playlist);
            Console.WriteLine("Song removed from playlist.");
        }

        public void ShowSongsInPlaylist(Playlist playlist)
        {
            if (playlist == null) return;
            var items = playlist.ShowPlayables();
            if (items.Count == 0)
            {
                Console.WriteLine("This playlist is empty.");
                return;
            }
            Console.WriteLine($"\n── Songs in \"{playlist.Title}\" ──");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"[{i + 1}] {items[i].Title}");
        }

        // -------------------────
        // FRIENDS  (uses SuperUser)
        // -------------------────

        public void ShowFriends()
        {
            if (!CheckActiveUser()) return;
            var friends = ActiveUser.ShowFriends();
            if (friends.Count == 0)
            {
                Console.WriteLine("You have no friends added yet.");
                return;
            }
            Console.WriteLine($"\n── Friends of {ActiveUser.Name} ──");
            for (int i = 0; i < friends.Count; i++)
                Console.WriteLine($"[{i + 1}] {friends[i]}");
        }

        // returns the selected friend so the menu can show their playlists
        public User SelectFriend(int index)
        {
            if (!CheckActiveUser()) return null;
            var friends = ActiveUser.ShowFriends();
            int i = index - 1;
            if (i < 0 || i >= friends.Count)
            {
                Console.WriteLine("[Error] Invalid friend selection.");
                return null;
            }
            return friends[i];
        }

        // add a user from AllUsers as a friend (by 1-based index)
        public void AddFriend(int index)
        {
            if (!CheckActiveUser()) return;
            int i = index - 1;
            if (i < 0 || i >= AllUsers.Count)
            {
                Console.WriteLine("[Error] Invalid user selection.");
                return;
            }
            var user = AllUsers[i];
            ActiveUser.AddFriend(user);
            Console.WriteLine($"{user.Name} added as a friend.");
        }

        // remove a friend by their 1-based index in the friends list
        public void RemoveFriend(int index)
        {
            if (!CheckActiveUser()) return;
            var friends = ActiveUser.ShowFriends();
            int i = index - 1;
            if (i < 0 || i >= friends.Count)
            {
                Console.WriteLine("[Error] Invalid friend selection.");
                return;
            }
            var user = friends[i];
            ActiveUser.RemoveFriend(user);
            Console.WriteLine($"{user.Name} removed from friends.");
        }

        // -------------------────
        // HELPERS
        // -------------------────

        // page switching helper works for any list type
        // "display" is a function that turns one item into the string to print
        private void ShowPage<T>(List<T> items, int pageNumber, Func<T, string> display)
        {
            int totalPages = (int)Math.Ceiling((double)items.Count / ItemsPerPage);

            if (totalPages == 0)
            {
                Console.WriteLine("(nothing here yet)");
                return;
            }

            // clamp page number so it never crashes
            if (pageNumber < 1 || pageNumber > totalPages)
            {
                Console.WriteLine($"[Error] Page must be between 1 and {totalPages}.");
                return;
            }

            Console.WriteLine("-----------------------");

            int startIndex = (pageNumber - 1) * ItemsPerPage;
            int endIndex = Math.Min(startIndex + ItemsPerPage, items.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine($"[{i + 1}] {display(items[i])}");
            }

            Console.WriteLine($"--------------- Page {pageNumber} / {totalPages}");
        }

        // small helper: prints an error and returns false if no user is logged in
        private bool CheckActiveUser()
        {
            if (ActiveUser == null)
            {
                Console.WriteLine("[Error] No user logged in.");
                return false;
            }
            return true;
        }
    }
}