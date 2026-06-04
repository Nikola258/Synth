using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Synth
{

    internal class Client
    {
        
        private List<Song> AllSongs { get; set; }
        private const int SongsPerPage = 6;
        public IPlayable CurrentlyPlaying { get; private set; }

        public Client()
        {
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
            CurrentlyPlaying?.Stop();

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
            CurrentlyPlaying.Play();
        }

        // add progress bar or something nice visually to show the progress
        public void ShowNowPlaying()
        {
            if (CurrentlyPlaying is Song song)
            {
                Console.Clear();
                Console.WriteLine("Now Playing\n");
                Console.WriteLine($"Artist: {string.Join(", ", song.Artists)}");
                Console.WriteLine($"Genre : {song.SongGenre}");
            }
        }

        private List<Song> LoadSongs()
        {
            // artists
            var hippoRap = new Artist("Hippo Rap");
            var madMax = new Artist("Mad Max");
            var girlyFreak = new Artist("Girly Freak");
            var deadChild = new Artist("Dead Child");
            var rickySand = new Artist("Ricky Sand");
            var technoCock = new Artist("Techno Cock");
            var afroWeed = new Artist("Afro Weed");
            var shoppensDad = new Artist("Shoppen's Dad");
            


            // song list
            var songs = new List<Song>
            {
                new Song("Hippo Rap Anthem", new List<Artist> { hippoRap },210, Genres.Rap, @"C:\music\NeedForSpeedMostWanted.mp3"),
                new Song ("Mad Max Lullaby", new List<Artist> { madMax }, 180, Genres.Soul, @"C:\music\TechoBoom.mp3"),
                new Song ("Girly Freak Pop", new List<Artist> { girlyFreak }, 240, Genres.Pop, @"C:\music\Nightcore.mp3"),
                new Song ("Dead Child's Rock", new List<Artist> { deadChild }, 200, Genres.Rock, @"C:\music\RockMix.mp3"),
                new Song ("Ricky Sand's Country Ballad", new List<Artist> { rickySand }, 230, Genres.Country, @"C:\music\BluesGuitar.mp3"),
                new Song ("Techno Cock's Blast", new List<Artist> { technoCock }, 230, Genres.Country, @"C:\music\DarkTechno.mp3"),
                new Song ("WEEDnin out haaard", new List<Artist> { afroWeed }, 260, Genres.Jazz, @"C:\music\NormalTechno.mp3"),
                new Song ("You are a Emperor comquering whole Earth", new List<Artist> { shoppensDad }, 300, Genres.Jazz, @"C:\music\WhiskyBlues.mp3"),
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


            foreach(var song in AllSongs)
    {
                // only print if we have skipped the songs from previous pages
                if (currentLoopIndex >= startIndex)
                {
                    // print the song ( 1-based indexing for the UI)
                    Console.WriteLine($"[{currentLoopIndex + 1}] " + song);
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

    }
}
