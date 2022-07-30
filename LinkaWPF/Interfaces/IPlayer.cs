using System;

namespace LinkaWPF.Interfaces
{
    public interface IPlayer
    {
        void Play();
        event EventHandler Ending;
    }
}
