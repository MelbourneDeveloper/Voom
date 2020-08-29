using System;
using System.Threading;
using System.Threading.Tasks;

namespace Voom
{
    public class ObserverFactory<T> : IDisposable
    {
        private bool isRunning = true;
        Func<T> _func;
        int _millisecondsTimeout = 1000;

        public ObserverFactory(
            Func<T> func,
            int? millisecondsTimeout = null)
        {
            _func = func;
            if (millisecondsTimeout.HasValue)
            {
                _millisecondsTimeout = millisecondsTimeout.Value;
            }
        }

        public void Dispose()
        {
            isRunning = false;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            Task.Run(() =>
            {
                while (isRunning)
                {
                    observer.OnNext(_func());
                    Thread.Sleep(_millisecondsTimeout);
                }
            });

            return this;
        }
    }
}
