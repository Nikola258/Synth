// Program.csusing Synth;

using Synth;

internal class Program
{

    private static void Main(string[] args)
    {
        ShowWelcomeScreen();

        Client client = new Client();
        // TDOD add switch case for menu options



        int currentPage = 1;
        client.ShowAllSongs(currentPage);

        while (true)
        {
            Console.Write("\nEnter command: ");
            // array of words("page 2" becomes ["page", "2"])
            string[] parts = (Console.ReadLine() ?? "").Trim().ToLower().Split(' ');
            string command = parts[0]; // first word ("page" or "exit")

            if (command == "exit")
            {
                break;
            }

            if (command == "page" && parts.Length > 1)
            {
                // convert the second word to a number
                currentPage = int.Parse(parts[1]);
                client.ShowAllSongs(currentPage);
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
        Console.WriteLine("│  1  Synth Player                                             │");
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


    // TODO:// Create Client class with LoadSongs method that reads from a file and creates Song and Artist objects// Create ShowAllSongs method that displays all songs in the library as a list// Create a methode for duration of songs in minutes and seconds    }


}