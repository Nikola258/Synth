using System.Collections.Generic;
using WMPLib;

namespace Synth
{
    // DataLoader creates all the sample data 
    internal static class DataLoader
    {
        public static (List<User> users, List<Album> albums, List<Song> songs) Load()
        {
            // need WMP only to read the file duration
            var wmp = new WindowsMediaPlayer();

            double GetDuration(string filePath)
            {
                try
                {
                    var media = wmp.newMedia(filePath);
                    return media.duration;
                }
                catch
                {
                    // if the file do not exist on this machine, return a fake duration
                    return 200;
                }
            }

            // -- Artists 
            var hippoRap = new Artist("Hippo Rap");
            var madMax = new Artist("Mad Max");
            var girlyFreak = new Artist("Girly Freak");
            var deadChild = new Artist("Dead Child");
            var rickySand = new Artist("Ricky Sand");
            var technoBlast = new Artist("Techno Blast");
            var afroWeed = new Artist("Afro Weed");
            var shoppensDad = new Artist("Shoppen's Dad");
            var japan = new Artist("Japan");
            var hiphop = new Artist("Hip Hop");
            var funk = new Artist("Funk");
            var chill = new Artist("Chill");

            // -- Songs
            var songs = new List<Song>
            {
                new Song("Hippo Rap Anthem", new List<Artist> { hippoRap }, GetDuration(@"C:\music\NeedForSpeedMostWanted.mp3"), Genres.Rap, @"C:\music\NeedForSpeedMostWanted.mp3"),
                new Song("Mad Max Lullaby", new List<Artist> { madMax }, GetDuration(@"C:\music\TechnoBoom.mp3"), Genres.Soul, @"C:\music\TechnoBoom.mp3"),
                new Song("Girly Freak Pop", new List<Artist> { girlyFreak }, GetDuration(@"C:\music\Nightcore.mp3"), Genres.Pop, @"C:\music\Nightcore.mp3"),
                new Song("Dead Child's Rock", new List<Artist> { deadChild }, GetDuration(@"C:\music\RockMix.mp3"), Genres.Rock, @"C:\music\RockMix.mp3"),
                new Song("Ricky Sand's Country Ballad", new List<Artist> { rickySand }, GetDuration(@"C:\music\BluesGuitar.mp3"), Genres.Country, @"C:\music\BluesGuitar.mp3"),
                new Song("Techno Blast", new List<Artist> { technoBlast }, GetDuration(@"C:\music\DarkTechno.mp3"), Genres.EDM, @"C:\music\DarkTechno.mp3"),
                new Song("WEEDnin out haaard", new List<Artist> { afroWeed }, GetDuration(@"C:\music\NormalTechno.mp3"), Genres.Jazz, @"C:\music\NormalTechno.mp3"),
                new Song("You are an Emperor conquering the Earth",new List<Artist> { shoppensDad }, GetDuration(@"C:\music\WhiskyBlues.mp3"), Genres.Jazz, @"C:\music\WhiskyBlues.mp3"),
                new Song("Japan's Finest", new List<Artist> { japan }, GetDuration(@"C:\music\japan-1.mp3"), Genres.Jazz, @"C:\music\japan-1.mp3"),
                new Song("Japan's Finest 2", new List<Artist> { japan }, GetDuration(@"C:\music\japan-2.mp3"), Genres.Jazz, @"C:\music\japan-2.mp3"),
                new Song("Japan`s Finest 3", new List<Artist> { japan }, GetDuration(@"C:\music\japan-3.mp3"), Genres.Jazz, @"C:\music\japan-3.mp3"),
                new Song("Japan`s fInest 4", new List<Artist> { japan }, GetDuration(@"C:\music\japan-4.mp3"), Genres.Jazz, @"C:\music\japan-4.mp3"),
                new Song("Japan`s Finest 5", new List<Artist> { japan }, GetDuration(@"C:\music\japan-5.mp3"), Genres.Jazz, @"C:\music\japan-5.mp3"),
                new Song("Japan`s Finest 6", new List<Artist> { japan }, GetDuration(@"C:\music\japan-6.mp3"), Genres.Jazz, @"C:\music\japan-6.mp3"),
                new Song("Hip Hop Beats", new List<Artist> { hiphop }, GetDuration(@"C:\music\hiphop-1.mp3"), Genres.Jazz, @"C:\music\hiphop-1.mp3"),
                new Song("Hip Hop Beats 2", new List<Artist> { hiphop }, GetDuration(@"C:\music\hiphop-2.mp3"), Genres.Jazz, @"C:\music\hiphop-2.mp3"),
                new Song("Hip Hop Bewts 3", new List<Artist> { hiphop }, GetDuration(@"C:\music\hiphop-3.mp3"), Genres.Jazz, @"C:\music\hiphop-3.mp3"),
                new Song("Hip Hop Beats 4", new List<Artist> { hiphop }, GetDuration(@"C:\music\hiphop-4.mp3"), Genres.Jazz, @"C:\music\hiphop-4.mp3"),
                new Song("Funky Beats", new List<Artist> { funk }, GetDuration(@"C:\music\funk-1.mp3"), Genres.Jazz, @"C:\music\funk-1.mp3"),
                new Song("Funky Beats 2", new List<Artist> { funk }, GetDuration(@"C:\music\funk-2.mp3"), Genres.Jazz, @"C:\music\funk-2.mp3"),
                new Song("Funky Beats 3", new List<Artist> { funk }, GetDuration(@"C:\music\funk-3.mp3"), Genres.Jazz, @"C:\music\funk-3.mp3"),
                new Song("Funky Beats 4", new List<Artist> { funk }, GetDuration(@"C:\music\funk-4.mp3"), Genres.Jazz, @"C:\music\funk-4.mp3"),
                new Song("Funky Beats 5", new List<Artist> { funk }, GetDuration(@"C:\music\funk-5.mp3"), Genres.Jazz, @"C:\music\funk-5.mp3"),
                new Song("Funky Beats 6", new List<Artist> { funk }, GetDuration(@"C:\music\funk-6.mp3"), Genres.Jazz, @"C:\music\funk-6.mp3"),
                new Song("Funky Beats 7", new List<Artist> { funk }, GetDuration(@"C:\music\funk-7.mp3"), Genres.Jazz, @"C:\music\funk-7.mp3"),
                new Song("Chill Vibes", new List<Artist> { chill }, GetDuration(@"C:\music\chill-1.mp3"), Genres.Jazz, @"C:\music\chill-1.mp3"),
                new Song("Chill Vibes 2", new List<Artist> { chill }, GetDuration(@"C:\music\chill-2.mp3"), Genres.Jazz, @"C:\music\chill-2.mp3"),
                new Song ("Chill Vibes 3", new List<Artist> { chill }, GetDuration(@"C:\music\chill-3.mp3"), Genres.Jazz, @"C:\music\chill-3.mp3"),
                new Song("Chill Vibes 4", new List<Artist> { chill }, GetDuration(@"C:\music\chill-4.mp3"), Genres.Jazz, @"C:\music\chill-4.mp3"),

            };

            // -- Albums 
            var albums = new List<Album>
            {
                new Album(new List<Artist> { hippoRap }, "Hippo Hits", new List<Song> { songs[0] }),
                new Album(new List<Artist> { madMax }, "Mad Max's Greatest", new List<Song> { songs[1] }),
                new Album(new List<Artist> { girlyFreak }, "Girly Freak's Finest", new List<Song> { songs[2], songs[4] }),
                new Album(new List<Artist> { japan }, "Japans best music", new List<Song> { songs[8], songs[9], songs[10], songs[11], songs[12], songs[13] }),
                new Album(new List<Artist> { hiphop }, "Hip Hop Beats", new List<Song> { songs[14], songs[15], songs[16], songs[17] }),
                new Album(new List<Artist> { funk }, "Funky Beats", new List<Song> { songs[18], songs[19], songs[20], songs[21], songs[22], songs[23], songs[24] }),
                new Album(new List<Artist> { chill }, "Chill Vibes", new List<Song> { songs[25], songs[26], songs[27], songs[28] })
            };


            // -- Users
            var alice = new SuperUser("Alice");
            var bob = new SuperUser("Bob");
            var cara = new SuperUser("Cara");

            // Alice have starter playlist
            var alicePlaylist = alice.CreatePlayList("Alice's Favourites");
            alicePlaylist.Add(songs[0]);
            alicePlaylist.Add(songs[2]);

            var users = new List<User> { alice, bob, cara };

            return (users, albums, songs);
        }
    }
}