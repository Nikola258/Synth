using Synth;

internal class Program
{
    private static void Main(string[] args)
    {

        // TODO:
        // - add more songs to the list
        // - add Console.Clear(); for cleaner look

        // TODO: add explanation of how to use the menu in HandleSongsMenu()
        while (true)
        {
            ShowWelcomeScreen();
            string userInput = Console.ReadLine() ?? "0";

            switch (userInput)
            {
                case "1": // Songs
                    HandleSongsMenu();
                    break;
                case "2": // Albums
                    HandleAlbumsMenu();
                    break;
                case "3": // Artists
                    Console.WriteLine("Artists feature coming soon...");
                    Console.ReadLine();
                    break;
                case "4": // My playlists
                    Console.WriteLine("My playlists feature coming soon...");
                    Console.ReadLine();
                    break;
                case "5": // Friends
                    Console.WriteLine("Friends feature coming soon...");
                    Console.ReadLine();
                    break;
                case "0": // Exit
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }


    private static void HandleSongsMenu()
    {
        Console.Clear();
        int currentPage = 1;
        Client client = new Client();

        while (true)
        {
            client.ShowAllSongs(currentPage);
            Console.WriteLine("\n- [page {number}] to switch page");
            Console.WriteLine("- [select {number}] to select a song");
            Console.WriteLine("- [back] to go back");
            Console.Write("\nEnter command: ");
            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }

            else if (command == "page" && parts.Length > 1)
            {
                if (int.TryParse(parts[1], out currentPage))
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid page number.");
                    Console.ReadLine(); // Pause to let user read error
                    Console.Clear();
                }
            }
            else if (command == "select" && parts.Length > 1)
            {
                if (int.TryParse(parts[1], out int songIndex))
                {
                    client.SelectSong(songIndex);
                    HandleSongOptionsMenu(client);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid song number.");
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Unknown command. Try typing 'page 2' or 'exit'.");
                Console.Clear();
            }
        }
    }

    // album menu
    private static void HandleAlbumsMenu()
    {
        Console.Clear();
        int currentPage = 1;
        Client client = new Client();

        while (true)
        {
            client.ShowAllAlbums(currentPage);
            Console.WriteLine("\n- [page {number}] to switch page");
            Console.WriteLine("- [select {number}] to select a album to see details");
            Console.WriteLine("- [back] to go back");
            Console.Write("\nEnter command: ");
            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }

            else if (command == "page" && parts.Length > 1)
            {
                if (int.TryParse(parts[1], out currentPage))
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid page number.");
                    Console.ReadLine(); // Pause to let user read error
                    Console.Clear();
                }
            }
            else if (command == "select" && parts.Length > 1)
            {
                if (int.TryParse(parts[1], out int songIndex))
                {
                    // IMPORTANT!
                    // will be select album to see it's contents
                    //client.SelectAlbum();
                    //HandleAlbumsOptionsMenu(client);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid song number.");
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Unknown command. Try typing 'page 2' or 'exit'.");
                Console.Clear();
            }
        }
    }

    private static void HandleAlbumOptionsMenu(Client client)
    {
        while (true)
        {
            Console.WriteLine("\n[1] Play Album");
            Console.WriteLine("[2] Add Album to playlist (not working)");
            Console.WriteLine("[0] Back");

            Console.Write("-> ");
            string input = Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":

                    client.Play();
                    HandleNowPlayingMenu(client);
                    //Console.ReadLine();
                    break;

                case "2":
                    Console.WriteLine("Feature coming soon...");
                    //Console.ReadLine();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private static void HandleSongOptionsMenu(Client client)
    {
        while (true)
        {
            Console.WriteLine("\n[1] Play");
            Console.WriteLine("[2] Add to playlist (not working)");
            Console.WriteLine("[0] Back");

            Console.Write("-> ");
            string input = Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":
                    
                    client.Play();
                    HandleNowPlayingMenu(client);
                    //Console.ReadLine();
                    break;

                case "2":
                    Console.WriteLine("Feature coming soon...");
                    //Console.ReadLine();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private static void HandleNowPlayingMenu(Client client)
    {
        // return if no song is currently playing and to use song variable for easier access to song details
        if (client.CurrentlyPlaying is not Song song) return;

        bool isPaused = false;

        while (true)
        {
            Console.Clear();

            string statusTag = isPaused ? " [PAUSED]" : " [PLAYING]";

            Console.WriteLine("Now Playing\n");
            Console.WriteLine($"Playing: {song.Title} by {string.Join(", ", song.Artists)}");
            Console.WriteLine($"Artist: {string.Join(", ", song.Artists)}");
            Console.WriteLine($"Genre : {song.SongGenre}");
            Console.WriteLine($"\nStatus: {statusTag}");
            Console.WriteLine(client.GetCurrentSongTime());

            Console.WriteLine("\n[P] Pause");
            Console.WriteLine("[R] Resume");
            Console.WriteLine("[N] Next");
            Console.WriteLine("[0] Back");

            Console.Write("-> ");

            // check for user input without blocking the loop
            // this allows the UI (time) to keep updating smoothly
            // wait up to ~5 seconds (100 × 50ms) before refreshing the screen
            int loopCounter = 0;
            while (!Console.KeyAvailable && loopCounter < 100)
            {
                Thread.Sleep(50); // small delay to avoid high CPU usage
                loopCounter++;
            }

            // if no key was pressed during the wait period and restart the loop to redraw the UI (time update)
            if (!Console.KeyAvailable)
            {
                continue;
            }

            // use ReadKey instead of ReadLine so input does not block updates and the progress display keeps moving in real time
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            string input = keyInfo.KeyChar.ToString().ToLower();

            switch (input.ToLower())
            {
                case "p":
                    client.Pause();
                    isPaused = true;
                    break;

                case "r":
                    client.Resume();
                    isPaused = false;
                    break;

                case "n":
                    client.NextSong();
                    isPaused = false;
                    if (client.CurrentlyPlaying is Song nextSong)
                    {
                        song = nextSong;
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option");
                    Thread.Sleep(1000);
                    break;
            }


        }
    }

    private static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("╭──────────────────────────────────────────────────────────────╮");
        Console.WriteLine("│                  Synth CLI player - Console                  │");
        Console.WriteLine("├──────────────────────────────────────────────────────────────┤");
        Console.WriteLine("│  Synth Player                                                │");
        Console.WriteLine("│──────────────────────────────────────────────────────────────│");
        Console.WriteLine("│  Logged in: Main users                                       │");
        Console.WriteLine("│                                                              │");
        Console.WriteLine("│  Main menu (Choose a number)                                 │");
        Console.WriteLine("│──────────────────────────────────────────────────────────────│");
        Console.WriteLine("│  [1] Songs                                                   │");
        Console.WriteLine("│  [2] Albums                                                  │");
        Console.WriteLine("│  [3] Artists                                                 │");
        Console.WriteLine("│  [4] My playlists                                            │");
        Console.WriteLine("│  [5] Friends                                                 │");
        Console.WriteLine("│  [0] Exit                                                    │");
        Console.WriteLine("│                                                              │");
        Console.WriteLine("╰──────────────────────────────────────────────────────────────╯");
        Console.Write("-> ");
    }
}