using System;

namespace Banks.Observing
{
    public interface IObserver
    {
        void Modify(IObservable subject);
    }
}