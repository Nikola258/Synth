using System;
using System.Collections.Generic;
using System.Text;

namespace Synth
{
    internal interface IPlayable
    {
        void Play();
        void Pause();
        void Next();
        void Stop();
        double Length { get; }
        string Title { get; }
    }
}
