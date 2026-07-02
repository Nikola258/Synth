namespace Synth
{
    // playlist extends SongCollection
    // it belongs to a User (the owner)
    internal class Playlist : SongCollection
    {
        public User Owner { get; private set; }

        public Playlist(User owner, string title) : base(title)
        {
            Owner = owner;
        }

        // add any IPlayable (usually a Song) to the playlist
        public void Add(IPlayable item)
        {
            Playables.Add(item);
        }

        // remove an IPlayable from the playlist by its position (1-based index from the UI)
        public void Remove(IPlayable item)
        {
            Playables.Remove(item);
        }

        public override string ToString()
        {
            return $"{Title} (by {Owner.Name}) — {Playables.Count} songs";
        }
    }
}