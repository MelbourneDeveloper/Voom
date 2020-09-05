using System;

namespace Voom
{
    public class GenericObserver<T> : IObserver<T>
    {
        #region Fields
        private Action<T> _action;
        private IDisposable unsubscriber;
        #endregion

        #region Constructor
        public GenericObserver(Action<T> action)
        {
            _action = action;
        }
        #endregion

        #region Public Methods
        public virtual void Subscribe(IObservable<T> provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe() => unsubscriber.Dispose();

        public virtual void OnCompleted() { }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnNext(T value)
        {
            _action(value);
        }
        #endregion
    }
}
