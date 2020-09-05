using System;
using System.Collections.Generic;

namespace Voom
{
    public class GenericObservable<T> : IObservable<T>
    {
        #region Fields
        private readonly List<IObserver<T>> _observers;
        #endregion

        #region Constructor
        public GenericObservable()
        {
            _observers = new List<IObserver<T>>();
        }
        #endregion

        #region Public Methods
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<T>(_observers, observer);
        }

        public void Next(T item)
        {
            foreach (var observer in _observers) observer.OnNext(item);
        }
        #endregion
    }

}
