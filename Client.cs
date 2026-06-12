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

        private List<Song> AllSongs { get; set; }
        private const int SongsPerPage = 6;
        public IPlayable CurrentlyPlaying { get; private set; }
        private readonly WindowsMediaPlayer _player;

        public Client()
        {
            _player = new WindowsMediaPlayer();
            AllSongs = LoadSongs();
            CurrentlyPlaying = null;
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

            return songs;
        }

        public void ShowAllSongs(int pageNumber)
        {
            // calculate total pages
            int totalPages = (int)Math.Ceiling((double)AllSongs.Count / SongsPerPage);

            // validate
            if (pageNumber < 1 || pageNumber > totalPages)
            {
                Console.WriteLine($"\n[Error] Invalid page. Please choose between page 1 and {totalPages}.");
                return;
            }


            Console.WriteLine("\nTitle - Artist(s) - Genre");
            Console.WriteLine("-----------------------------------\n");

            // calculate where to start reading from the list
            int startIndex = (pageNumber - 1) * SongsPerPage;

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
                    if (songsPrinted == SongsPerPage)
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
    }
}