using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voom
{
    public class Publisher : ISubscribable, IPublisher
    {
        private readonly object _lock = new object();
        private readonly Dictionary<object, List<Delegate>> _handlers = new Dictionary<object, List<Delegate>>();

        async Task IPublisher.PublishAsync(object? value = null)
        {
            foreach (var delegates in _handlers.Values)
            {
                foreach (var @delegate in delegates)
                {
                    var returnValue = @delegate.DynamicInvoke(value);

                    if (returnValue is Task task)
                    {
                        await task;
                    }
                }
            }
        }

        void ISubscribable.Unsubscribe(object subscriber)
        {
            lock (_lock)
            {
                if (!_handlers.ContainsKey(subscriber))
                {
                    throw new InvalidOperationException();
                }

                _handlers.Remove(subscriber);
            }
        }

        void ISubscribable.Subscribe(object subscriber, Delegate @delegate)
        {
            lock (_lock)
            {
                if (!_handlers.TryGetValue(subscriber, out var delegates))
                {
                    delegates = new List<Delegate>();
                    _handlers.Add(subscriber, delegates);
                }

                delegates.Add(@delegate);
            }
        }
    }
}

