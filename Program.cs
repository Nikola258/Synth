// Program.csusing Synth;

internal class Program
{

    private static void Main(string[] args)

    {

        ShowWelcomeScreen();
        
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