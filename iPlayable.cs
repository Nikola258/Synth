namespace Synth
{
    // This interface defines what any "playable" thing must be able to do.
    // Both Song (and later Album/Playlist if needed) can implement this.
    internal interface IPlayable
    {
        string Title { get; }
        double Length { get; }
    }
}