using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WMPLib;

namespace Synth
{

    internal class Client
    {
        private List<User> AllUsers { get; set; }
        // The user who is currently logged in (must be a SuperUser to use playlists/friends)
        public SuperUser ActiveUser { get; private set; }
        private List<Song> AllSongs { get; set; }
        private List<Album> AllAlbums { get; set; }
        private const int ItemsPerPage = 6;
        public IPlayable CurrentlyPlaying { get; private set; }
        private readonly WindowsMediaPlayer _player;

        public Client(List<User> users, List<Album> albums, List<Song> songs)
        {
            AllUsers = users;
            AllAlbums = albums;
            AllSongs = songs;
        }

        // ─────────────────────────────────────────────
        // USER MANAGEMENT
        // ─────────────────────────────────────────────

        public void SetActiveUser(User user)
        {
            // Only SuperUser accounts can use playlists and friends
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

        // Print all users with a numbered list
        public void ShowAllUsers()
        {
            Console.WriteLine("\n- Users -");
            for (int i = 0; i < AllUsers.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {AllUsers[i]}");
            }
        }

        // Select a user by 1-based index and make them active
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

        // Select one of the active user's playlists by 1-based index
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

        // ─────────────────────────────────────────────
        // PLAYLIST MANAGEMENT  (delegates to SuperUser)
        // ─────────────────────────────────────────────

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
            // Stored in the calling menu — see Program.cs
            SelectUserPlaylist(index);
        }

        public void RemovePlaylist(int index)
        {
            if (!CheckActiveUser()) return;
            ActiveUser.RemovePlayList(index);
            Console.WriteLine("Playlist removed.");
        }

        // Add a song (from AllSongs) to a playlist
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

        // Remove a song from a playlist by its 1-based position in the playlist
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
            Console.WriteLine($"\n── Songs in \"{playlist.Title}\" -");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"[{i + 1}] {items[i].Title}");
        }
        public void SelectSong(int songIndex)
        {
            int index = songIndex - 1; // convert to 0-based


            if (songIndex <= 0 || songIndex > AllSongs.Count)
            {
                Console.WriteLine("[Error] Invalid song selection.");
                return;
            }
            _player.controls.stop();

            CurrentlyPlaying = AllSongs[index];
            Console.WriteLine($"\nSelected: {AllSongs[index]}");
        }

        public void Play()
        {
            if (CurrentlyPlaying == null)
            {
                Console.WriteLine("\n[Error] No song selected. Please select a song first.");
                return;
            }

            if (CurrentlyPlaying is Song song)
            {
                _player.URL = song.FilePath;
                _player.controls.play();

            }
            else
            {
                Console.WriteLine("[Error] Cannot play selection - missing file path.");
            }
        }

        public void Pause()
        {
            _player.controls.pause();
            Console.WriteLine("Song paused.");
        }

        public void Resume()
        {
            _player.controls.play();
            Console.WriteLine("Song resumed.");
        }

        public void NextSong()
        {
            // Check if CurrentlyPlaying is actually a Song
            if (CurrentlyPlaying is not Song song)
                return;

            // Find where this song is in the list (0, 1, 2, etc)
            int index = AllSongs.IndexOf(song);

            // If this is the last song, stop
            if (index >= AllSongs.Count - 1)
            {
                Console.WriteLine("No next song available.");
                return;
            }

            // Play the next song
            SelectSong(index + 2);
            Play();
        }
        

        private List<Song> LoadSongs()
        {
            
            double GetDuration(string filePath)
            {
                var media = _player.newMedia(filePath);
                return media.duration;
            }

            Song CreateSong(string title, List<Artist> artists, Genres genre, string filePath)
            {
                return new Song(title, artists, GetDuration(filePath), genre, filePath);
            }

            Album CreateAlbum(string title, List<Artist> artists, List<Song> albumSongs)
            {
                return new Album(artists, title, albumSongs);
            }

            // artists
            var hippoRap = new Artist("Hippo Rap");
            var madMax = new Artist("Mad Max");
            var girlyFreak = new Artist("Girly Freak");
            var deadChild = new Artist("Dead Child");
            var rickySand = new Artist("Ricky Sand");
            var technoBlast = new Artist("Techno Blast");
            var afroWeed = new Artist("Afro Weed");
            var shoppensDad = new Artist("Shoppen's Dad");



            // song list
            var songs = new List<Song>
            {
                CreateSong("Hippo Rap Anthem", new List<Artist> { hippoRap }, Genres.Rap, @"C:\music\NeedForSpeedMostWanted.mp3"),
                CreateSong("Mad Max Lullaby", new List<Artist> { madMax }, Genres.Soul, @"C:\music\TechnoBoom.mp3"),
                CreateSong("Girly Freak Pop", new List<Artist> { girlyFreak }, Genres.Pop, @"C:\music\Nightcore.mp3"),
                CreateSong("Dead Child's Rock", new List<Artist> { deadChild }, Genres.Rock, @"C:\music\RockMix.mp3"),
                CreateSong("Ricky Sand's Country Ballad", new List<Artist> { rickySand }, Genres.Country, @"C:\music\BluesGuitar.mp3"),
                CreateSong("Techno Blast", new List<Artist> { technoBlast }, Genres.Country, @"C:\music\DarkTechno.mp3"),
                CreateSong("WEEDnin out haaard", new List<Artist> { afroWeed }, Genres.Jazz, @"C:\music\NormalTechno.mp3"),
                CreateSong("You are a Emperor comquering whole Earth", new List<Artist> { shoppensDad }, Genres.Jazz, @"C:\music\WhiskyBlues.mp3"),
            };

            // albums
            var hippoAlbum = CreateAlbum("Hippo Hits", new List<Artist> { hippoRap }, new List<Song> { songs[0] });
            var madMaxAlbum = CreateAlbum("Mad Max's Greatest", new List<Artist> { madMax }, new List<Song> { songs[1] });
            var girlyAlbum = CreateAlbum("Girly Freak's Finest", new List<Artist> { girlyFreak }, new List<Song> { songs[2], songs[4] });

            AllAlbums.Add(hippoAlbum);
            AllAlbums.Add(madMaxAlbum);
            AllAlbums.Add(girlyAlbum);

            return songs;
        }

        public void ShowAllSongs(int pageNumber)
        {
            // calculate total pages
            int totalPages = (int)Math.Ceiling((double)AllSongs.Count / ItemsPerPage);

            // validate
            if (pageNumber < 1 || pageNumber > totalPages)
            {
                Console.WriteLine($"\n[Error] Invalid page. Please choose between page 1 and {totalPages}.");
                return;
            }


            Console.WriteLine("\nTitle - Artist(s) - Genre");
            Console.WriteLine("-----------------------------------\n");

            // calculate where to start reading from the list
            int startIndex = (pageNumber - 1) * ItemsPerPage;

            int currentLoopIndex = 0; // tracks every song we pass by
            int songsPrinted = 0;     // tracks how many displayed


            foreach (var song in AllSongs)
            {
                // only print if we have skipped the songs from previous pages
                if (currentLoopIndex >= startIndex)
                {
                    // print the song ( 1-based indexing for the UI)
                    Console.WriteLine($"[{currentLoopIndex + 1}] " + song + $" ({TimeSpan.FromSeconds(song.Duration):hh\\:mm\\:ss}) " );
                    songsPrinted++;

                    // stop when 6 songs are printed
                    if (songsPrinted == ItemsPerPage)
                    {
                        break;
                    }
                }

                currentLoopIndex++;
            }
            Console.WriteLine($"\n----------------------- Page: {pageNumber} / {totalPages}");
        }

        public string GetCurrentSongTime()
        {
            if (CurrentlyPlaying is not Song song) return "";

            // get current track time from the player
            double current = _player.controls.currentPosition;

            // convert raw seconds into clean h/m/s format
            string currentStr = TimeSpan.FromSeconds(current).ToString(@"hh\:mm\:ss");
            string totalStr = TimeSpan.FromSeconds(song.Duration).ToString(@"hh\:mm\:ss");

            
            return $"{currentStr} / {totalStr}";
        }

        public void ShowAllAlbums(int pageNumber)
        {
            // calculate total pages
            int totalPages = (int)Math.Ceiling((double)AllAlbums.Count / ItemsPerPage);

            // validate
            if (pageNumber < 1 || pageNumber > totalPages)
            {
                Console.WriteLine($"\n[Error] Invalid page. Please choose between page 1 and {totalPages}.");
                return;
            }


            Console.WriteLine("\nTitle - Artist(s) - Genre");
            Console.WriteLine("-----------------------------------\n");

            // calculate where to start reading from the list
            int startIndex = (pageNumber - 1) * ItemsPerPage;

            int currentLoopIndex = 0; // tracks every song we pass by
            int songsPrinted = 0;     // tracks how many displayed


            foreach (var album in AllAlbums)
            {
                // only print if we have skipped the songs from previous pages
                if (currentLoopIndex >= startIndex)
                {
                    // print the song ( 1-based indexing for the UI)
                    Console.WriteLine($"{album}, ({album.Songs.Count} songs)");
                    songsPrinted++;

                    // stop when 6 songs are printed
                    if (songsPrinted == ItemsPerPage)
                    {
                        break;
                    }
                }

                currentLoopIndex++;
            }
            Console.WriteLine($"\n----------------------- Page: {pageNumber} / {totalPages}");
        }

        public void SelectAlbum(int albumIndex)
        {
            int index = albumIndex - 1; // convert to 0-based


            if (albumIndex <= 0 || albumIndex > AllAlbums.Count)
            {
                Console.WriteLine("[Error] Invalid song selection.");
                return;
            }

            ShowAllSongsInAlbums(index);
        }

        public void ShowAllSongsInAlbums(int pageNumber)
        {
            // calculate total pages
            int totalPages = (int)Math.Ceiling((double)AllSongs.Count / ItemsPerPage);

            Console.WriteLine("\nTitle - Artist(s) - Genre");
            Console.WriteLine("-----------------------------------\n");

            // calculate where to start reading from the list
            int startIndex = (pageNumber - 1) * ItemsPerPage;

            int currentLoopIndex = 0; // tracks every song we pass by
            int songsPrinted = 0;     // tracks how many displayed


            foreach (var song in AllAlbums)
            {
                // only print if we have skipped the songs from previous pages
                if (currentLoopIndex >= startIndex)
                {
                    // print the song ( 1-based indexing for the UI)
                    Console.WriteLine($"[{currentLoopIndex + 1}] {song}");
                    songsPrinted++;

                    // stop when 6 songs are printed
                    if (songsPrinted == ItemsPerPage)
                    {
                        break;
                    }
                }

                currentLoopIndex++;
            }
            Console.WriteLine($"\n----------------------- Page: {pageNumber} / {totalPages}");
        }
    }
}