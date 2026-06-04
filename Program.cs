using Synth;

internal class Program
{
    private static void Main(string[] args)
    {
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
                    Console.WriteLine("Albums feature coming soon...");
                    Console.ReadLine();
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
        int currentPage = 1;
        Client client = new Client();
        client.ShowAllSongs(currentPage);

        while (true)
        {
            Console.Write("\nEnter command: ");
            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0];

            if (command == "exit")
            {
                break;
            }

            if (command == "page" && parts.Length > 1)
            {
                if (int.TryParse(parts[1], out currentPage))
                {
                    client.ShowAllSongs(currentPage);
                }
                else
                {
                    Console.WriteLine("Invalid page number.");
                }
            }
            else
            {
                Console.WriteLine("Unknown command. Try typing 'page 2' or 'exit'.");
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
        Console.WriteLine("│  Main menu                                                   │");
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