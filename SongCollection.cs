using System;
using System.Collections.Generic;

namespace Synth
{
    // SongCollection is the shared base for both Album and Playlist
    // holds a list of IPlayable items and a title
    // "abstract" means you cannot create a SongCollection directly,
    // you must use Album or Playlist instead
    internal abstract class SongCollection
    {
        public string Title { get; set; }

        // list that stores IPlayable items 
        protected List<IPlayable> Playables { get; set; } = new List<IPlayable>();

        public SongCollection(string title)
        {
            Title = title;
        }

        // returns all playable items in this collection
        public List<IPlayable> ShowPlayables()
        {
            return Playables;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}