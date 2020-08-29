using System;
using System.Threading.Tasks;

namespace Voom
{
    public class ObserverFactory<T> : IDisposable
    {
        private bool isRunning = true;
        Func<T> _func;

        public ObserverFactory(Func<T> func)
        {
            _func = func;
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
                }
            });

            return this;
        }
    }
}
