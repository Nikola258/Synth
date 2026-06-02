using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace Synth
{
    internal class Client
    {
        private List<Song> AllSongs { get; set; }
        private const int SongsPerPage = 6;

        public Client()
        {
            AllSongs = LoadSongs();
        }

        // add string filePath parameter for reading from file later
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
                new Song("Hippo Rap Anthem", new List<Artist> { hippoRap },210, Genres.Rap),
                new Song ("Mad Max Lullaby", new List<Artist> { madMax }, 180, Genres.Soul),
                new Song ("Girly Freak Pop", new List<Artist> { girlyFreak }, 240, Genres.Pop),
                new Song ("Dead Child's Rock", new List<Artist> { deadChild }, 200, Genres.Rock),
                new Song ("Ricky Sand's Country Ballad", new List<Artist> { rickySand }, 230, Genres.Country),
                new Song ("Techno Cock's Blast", new List<Artist> { rickySand }, 230, Genres.Country),
                new Song ("WEEDnin out haaard", new List<Artist> { afroWeed}, 260, Genres.Jazz),
                new Song ("You are a Emperor comquering whole Earth", new List<Artist> { shoppensDad }, 300, Genres.Classical ),
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
