using System;

namespace Voom
{
    /// <summary>
    /// This thing is probably useless. Use Observer.Create in System.Reactive
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _action;

        public Observer(Action<T> action)
        {
            _action = action;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value) => _action(value);
    }
}
