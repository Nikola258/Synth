namespace Synth
{
    // this interface defines what any "playable" thing must be able to do
    internal interface IPlayable
    {
        string Title { get; }
        double Length { get; }
    }
}