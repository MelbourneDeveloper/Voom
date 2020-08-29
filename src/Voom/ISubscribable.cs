using System;

namespace Voom
{
    public interface ISubscribable
    {
        void Subscribe(object subscriber, Delegate @delegate);
        void Unsubscribe(object subscriber);
    }
}

