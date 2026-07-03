using System;
using System.Threading;
using Synth;

internal class Program
{
    // one Client for the whole app
    static Client client;

    private static void Main(string[] args)
    {
        // load all data and create the Client
        var (users, albums, songs) = DataLoader.Load();
        client = new Client(users, albums, songs);

        // logging screen
        HandleUserSelectScreen();

        // main loop
        while (true)
        {
            ShowWelcomeScreen();
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

    // -----------------------------------
    // LOGGIN SCREEN
    // -----------------------------------

    private static void HandleUserSelectScreen()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("|──────────────────────────────────────────────|");
            Console.WriteLine("│            Welcome to Synth Player           │");
            Console.WriteLine("│──────────────────────────────────────────────│");
            Console.WriteLine("│  Please select your account:                 │");
            Console.WriteLine("|──────────────────────────────────────────────|");

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

    // ----------------
    // WELCOME SCREEN 
    // ----------------

    private static void ShowWelcomeScreen()
    {
        Console.Clear();
        string user = client.ActiveUser.Name;
        Console.WriteLine("|──────────────────────────────────────────────────────────────|");
        Console.WriteLine("│                  Synth CLI Player - Console                  │");
        Console.WriteLine("├──────────────────────────────────────────────────────────────┤");
        Console.WriteLine($"│  Logged in: {user}                                            │");
        Console.WriteLine("│                                                              │");
        Console.WriteLine("│  Main menu (Choose a number)                                 │");
        Console.WriteLine("│──────────────────────────────────────────────────────────────│");
        Console.WriteLine("│  [1] Songs                                                   │");
        Console.WriteLine("│  [2] Albums                                                  │");
        Console.WriteLine("│  [3] Artists (coming soon)                                   │");
        Console.WriteLine("│  [4] My Playlists                                            │");
        Console.WriteLine("│  [5] Friends                                                 │");
        Console.WriteLine("│  [0] Exit                                                    │");
        Console.WriteLine("|──────────────────────────────────────────────────────────────|");
        Console.Write("-> ");
    }

    // --------------
    // SONGS MENU
    // --------------

    private static void HandleSongsMenu()
    {
        Console.Clear();
        int currentPage = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("-- All Songs --");
            client.ShowAllSongs(currentPage);

            Console.WriteLine("\n[page N] switch page");
            Console.WriteLine("[select N] select a song");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");

            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }
            else if (command == "page" && parts.Length > 1 && int.TryParse(parts[1], out int page))
            {
                currentPage = page;
            }
            else if (command == "select" && parts.Length > 1 && int.TryParse(parts[1], out int songIndex))
            {
                client.SelectSong(songIndex);
                if (client.CurrentlyPlaying != null)
                    HandleSongOptionsMenu();
            }
            else
            {
                Console.WriteLine("Unknown command. Press Enter to try again.");
                Console.ReadLine();
            }
        }
    }

    // ----------------------------------------------------
    // SONG OPTIONS (after selecting a song from any list)
    // ---------------------------------------------------

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

    // ----------------------------
    // NOW PLAYING SCREEN
    // ----------------------------

    private static void HandleNowPlayingScreen()
    {
        while (true)
        {
            Console.Clear();

            var song = client.GetCurrentSong();
            if (song == null) return;

            string status = client.IsPaused ? "[PAUSED]" : "[PLAYING]";

            Console.WriteLine("── Now Playing ──────────────────────────────");
            Console.WriteLine($"  Title  : {song.Title}");
            Console.WriteLine($"  Artist : {string.Join(", ", song.Artists)}");
            Console.WriteLine($"  Genre  : {song.SongGenre}");
            Console.WriteLine($"  Status : {status}");
            Console.WriteLine($"  Time   : {client.GetCurrentSongTime()}");
            Console.WriteLine("─────────────────────────────────────────────");
            Console.WriteLine("\n[P] Pause    [R] Resume    [N] Next    [0] Back");
            Console.Write("-> ");

            // wait 5 seconds before refreshing the time display
            int waited = 0;
            while (!Console.KeyAvailable && waited < 100)
            {
                Thread.Sleep(50);
                waited++;
            }

            if (!Console.KeyAvailable) continue; // redraw (time update)

            char key = char.ToLower(Console.ReadKey(intercept: true).KeyChar);

            switch (key)
            {
                case 'p':
                    client.Pause();
                    break;
                case 'r':
                    client.Resume();
                    break;
                case 'n':
                    client.NextSong();
                    break;
                case '0':
                    return;
                default:
                    break;
            }
        }
    }

    // ----------------------------
    // ALBUMS MENU
    // ----------------------------

    private static void HandleAlbumsMenu()
    {
        int currentPage = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("-- All Albums --");
            client.ShowAllAlbums(currentPage);

            Console.WriteLine("\n[page N] switch page");
            Console.WriteLine("[select N] open an album");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");

            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back")
            {
                break;
            }
            else if (command == "page" && parts.Length > 1 && int.TryParse(parts[1], out int page))
            {
                currentPage = page;
            }
            else if (command == "select" && parts.Length > 1 && int.TryParse(parts[1], out int albumIndex))
            {
                var album = client.SelectAlbum(albumIndex);
                if (album != null)
                    HandleAlbumDetailMenu(album);
            }
            else
            {
                Console.WriteLine("Unknown command. Press Enter to try again.");
                Console.ReadLine();
            }
        }
    }

    // show the songs inside one album
    private static void HandleAlbumDetailMenu(Album album)
    {
        int currentPage = 1;

        while (true)
        {
            Console.Clear();
            client.ShowSongsInAlbum(album, currentPage);

            Console.WriteLine("\n[select N] pick a song");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");

            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back") break;
            else if (command == "select" && parts.Length > 1 && int.TryParse(parts[1], out int songIndex))
            {
                client.SelectSongFromAlbum(album, songIndex);
                if (client.CurrentlyPlaying != null)
                    HandleSongOptionsMenu();
            }
            else
            {
                Console.WriteLine("Unknown command.");
                Console.ReadLine();
            }
        }
    }

    // ----------------------------
    // ARTISTS MENU 
    // ----------------------------

    private static void HandleArtistsMenu()
    {
        Console.Clear();
        Console.WriteLine("Artists feature coming soon.");
        Console.ReadLine();
    }

    // ----------------------------
    // PLAYLIST MENU
    // ----------------------------

    private static void HandlePlaylistMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-- My Playlists --");
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
            {
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
    }

    // ----------------------------
    // ADD TO PLAYLIST MENU
    // ----------------------------

    private static void HandleAddToPlaylistMenu()
    {
        Console.Clear();
        Console.WriteLine("-- Add Song to Playlist --");

        // show the user their available playlists
        client.ShowUserPlaylists();

        Console.Write("\nEnter playlist number to add this song to (or '0' to go back): ");
        string input = Console.ReadLine() ?? "";

        if (input.Trim() == "0") return;

        if (int.TryParse(input.Trim(), out int playlistIndex))
        {
            // fetch the selected playlist using the client
            var playlist = client.SelectUserPlaylist(playlistIndex);

            if (playlist != null && client.CurrentlyPlaying is Song currentSong)
            {
                // add the song currently selected/playing to the chosen playlist,
                // it takes a Song object
                playlist.Add(currentSong);

                Console.WriteLine($"\nSuccessfully added \"{currentSong.Title}\" to \"{playlist.Title}\"!");
            }
            else
            {
                Console.WriteLine("\nCould not complete the action. Invalid playlist or no song selected.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
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

    // ----------------------------
    // FRIENDS MENU
    // ----------------------------

    private static void HandleFriendsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-- Friends --");
            client.ShowFriends();

            Console.WriteLine("\n[add] add a friend from user list");
            Console.WriteLine("[remove N] remove friend at position N");
            Console.WriteLine("[view N] view playlists of friend N");
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
                Console.Clear();
                Console.WriteLine("-- All Users --");
                client.ShowAllUsers();
                Console.Write("\nUser number to add as friend: ");
                if (int.TryParse(Console.ReadLine(), out int userIndex))
                {
                    client.AddFriend(userIndex);
                }
                Console.ReadLine();
            }
            else if (command == "remove" && parts.Length > 1 && int.TryParse(parts[1], out int removeIndex))
            {
                client.RemoveFriend(removeIndex);
                Console.ReadLine();
            }
            else if (command == "view" && parts.Length > 1 && int.TryParse(parts[1], out int viewIndex))
            {
                var friend = client.SelectFriend(viewIndex);
                if (friend != null)
                    HandleFriendPlaylistsMenu(friend);
            }
            else
            {
                Console.WriteLine("Unknown command. Press Enter.");
                Console.ReadLine();
            }
        }
    }

    // browse a friend's playlists and their songs
    private static void HandleFriendPlaylistsMenu(User friend)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"-- {friend.Name} Playlists --");

            var playlists = friend.ShowPlaylists();
            if (playlists.Count == 0)
            {
                Console.WriteLine("This friend has no playlists.");
                Console.ReadLine();
                return;
            }

            for (int i = 0; i < playlists.Count; i++)
                Console.WriteLine($"[{i + 1}] {playlists[i]}");

            Console.WriteLine("\n[select N] view songs in playlist N");
            Console.WriteLine("[back] go back");
            Console.Write("\n-> ");

            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "back") break;
            else if (command == "select" && parts.Length > 1 && int.TryParse(parts[1], out int index))
            {
                int i = index - 1;
                if (i >= 0 && i < playlists.Count)
                {
                    Console.Clear();
                    client.ShowSongsInPlaylist(playlists[i]);
                    Console.WriteLine("\nPress Enter to go back.");
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
}