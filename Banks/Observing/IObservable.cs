using System;

namespace Banks.Observing
{
    public interface IObservable
    {
        DateTime NewDate { get; }
        void AddObserver(IObserver observer);
        void Notify();
    }
}