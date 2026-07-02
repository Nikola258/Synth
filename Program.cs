using Synth;

internal class Program
{
    // after
    private static void Main(string[] args)
    {
        // Load all data and create the Client
        var (users, albums, songs) = DataLoader.Load();
        client = new Client(users, albums, songs);

        // Ask the user to pick an account before showing the main menu
        HandleUserSelectScreen();

        // Main loop
        while (true)
        {
            string input = Console.ReadLine() ?? "0";
            switch (input.Trim())
            {
                case "1": HandleSongsMenu(); break;
                case "2": HandleAlbumsMenu(); break;
                case "3": HandleArtistsMenu(); break;
                case "4": HandlePlaylistMenu(); break;
                case "5": HandleFriendsMenu(); break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Enter to try again.");
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

    // ─────────────────────────────────────────────────────────────────────────
    // SONG OPTIONS (after selecting a song from any list)
    // ─────────────────────────────────────────────────────────────────────────

    private static void HandleSongOptionsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Selected: {client.CurrentlyPlaying?.Title}");
            Console.WriteLine("\n[1] Play");
            Console.WriteLine("[2] Add to playlist");
            Console.WriteLine("[0] Back");
            Console.Write("-> ");

            string input = Console.ReadLine() ?? "0";

            switch (input.Trim())
            {
                case "1":
                    client.Play();
                    HandleNowPlayingScreen();
                    break;
                case "2":
                    // !add playlist menu
                    HandleAddToPlaylistMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Console.ReadLine();
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

    // ─────────────────────────────────────────────────────────────────────────
    // USER SELECT (runs once at startup)
    // ─────────────────────────────────────────────────────────────────────────
    private static void HandleUserSelectScreen()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╭──────────────────────────────────────────────╮");
            Console.WriteLine("│            Welcome to Synth Player           │");
            Console.WriteLine("│──────────────────────────────────────────────│");
            Console.WriteLine("│  Please select your account:                 │");
            Console.WriteLine("╰──────────────────────────────────────────────╯");
            client.ShowAllUsers();
            Console.Write("\nEnter user number: ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input.Trim(), out int index))
            {
                client.SelectUser(index);
                if (client.ActiveUser != null)
                {
                    Console.WriteLine($"\nWelcome, {client.ActiveUser.Name}! Press Enter to continue...");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
                Console.ReadLine();
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PLAYLIST MENU
    // ─────────────────────────────────────────────────────────────────────────

    private static void HandlePlaylistMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("── My Playlists ──");
            client.ShowUserPlaylists();

            Console.WriteLine("\n[create] create new playlist");
            Console.WriteLine("[select N] open a playlist");
            Console.WriteLine("[delete N] delete a playlist");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");
            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }
            else if (command == "create")
            {
                Console.Write("Playlist name: ");
                string name = Console.ReadLine() ?? "My Playlist";
                client.CreatePlaylist(name);
                Console.ReadLine();
            }
            else if (command == "select" && parts.Length > 1 && int.TryParse(parts[1], out int index))
                var playlist = client.SelectUserPlaylist(index);
            if (playlist != null)
                HandlePlaylistDetailMenu(playlist);
        }
            else if (command == "delete" && parts.Length > 1 && int.TryParse(parts[1], out int delIndex))
        {
            client.RemovePlaylist(delIndex);
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Unknown command. Press Enter.");
            Console.ReadLine();
        }
    }

    // Inside a single playlist
    private static void HandlePlaylistDetailMenu(Playlist playlist)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"── Playlist: {playlist.Title} ──");
            client.ShowSongsInPlaylist(playlist);
            Console.WriteLine("\n[add] add a song from All Songs");
            Console.WriteLine("[remove N] remove song at position N");
            Console.WriteLine("[play N] play song at position N");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");

            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }
            else if (command == "add")
            {
                // Show all songs so the user can pick one
                Console.Clear();
                Console.WriteLine("-- All Songs (pick one to add) --");
                client.ShowAllSongs(1);
                Console.Write("\nSong number: ");
                if (int.TryParse(Console.ReadLine(), out int songIndex))
                {
                    client.AddToPlaylist(songIndex, playlist);
                }
                Console.ReadLine();
            }
            else if (command == "remove" && parts.Length > 1 && int.TryParse(parts[1], out int removeIndex))
            {
                client.RemoveFromPlaylist(removeIndex, playlist);
                Console.ReadLine();
            }
            else if (command == "play" && parts.Length > 1 && int.TryParse(parts[1], out int playIndex))
            {
                var items = playlist.ShowPlayables();
                int i = playIndex - 1;
                if (i >= 0 && i < items.Count && items[i] is Song s)
                {
                    client.CurrentlyPlaying = items[i]; // set via the property
                    // We need to expose this — see note below
                    client.Play();
                    HandleNowPlayingScreen();
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Unknown command. Press Enter.");
                Console.ReadLine();
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADD TO PLAYLIST MENU
    // ─────────────────────────────────────────────────────────────────────────
    private static void HandleAddToPlaylistMenu()
    {
        Console.Clear();
        Console.WriteLine("-- Add Current Song to Playlist --");

        // Display the user's current playlists
        client.ShowUserPlaylists();

        Console.Write("\nEnter playlist number to add this song to (or 0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0)
        {
            var playlist = client.SelectUserPlaylist(index);
            if (playlist != null && client.CurrentlyPlaying is Song currentSong)
            {
                // Assuming your client has a way to add a Song object directly, 
                // or you can pass the index of CurrentlyPlaying.
                // Replace this line with the correct method your 'Client' class provides:
                client.AddToPlaylist(index, playlist);
                Console.WriteLine($"Added to {playlist.Title}!");
            }
            else
            {
                Console.WriteLine("Invalid playlist selection.");
            }
        }
        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }
    private static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("╭──────────────────────────────────────────────────────────────╮");
        Console.WriteLine("│                  Synth CLI player - Console                  │");
        // add user variable and then add it to the main screen insted of just the text "user"
        string user = client.ActiveUser?.Name ?? "Guest";
        Console.WriteLine("|──────────────────────────────────────────────────────────────|");
        Console.WriteLine("│                  Synth CLI Player - Console                  │");
        Console.WriteLine("├──────────────────────────────────────────────────────────────┤");
        Console.WriteLine("│  Logged in: user                                             |");
        Console.WriteLine("│                                                              │");
        Console.WriteLine("│  Main menu (Choose a number)                                 │");
        Console.WriteLine("│──────────────────────────────────────────────────────────────│");
        Console.WriteLine("│  [1] Songs                                                   │");
        Console.WriteLine("│  [2] Albums                                                  │");
        Console.WriteLine("│  [3] Artists (coming soon)                                   │");
        Console.WriteLine("│  [4] My Playlists (coming soon)                              │");
        Console.WriteLine("│  [5] Friends (coming soon)                                   │");
        Console.WriteLine("│  [0] Exit                                                    │");
        Console.WriteLine("|──────────────────────────────────────────────────────────────|");
        Console.Write("-> ");
    }
}