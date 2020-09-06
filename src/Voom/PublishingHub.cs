using System;
using System.Collections.Generic;

namespace Voom
{
    public delegate void Next<T>(T value);

    public class PublishingHub
    {
        private readonly object _lock = new object();
        private readonly Dictionary<object, List<Delegate>> _handlers = new Dictionary<object, List<Delegate>>();

        public void Publish<T>(T value)
        {
            foreach (var delegates in _handlers.Values)
            {
                foreach (var @delegate in delegates)
                {
                    var next = @delegate as Next<T>;

                    next?.Invoke(value);
                }
            }
        }

        public void Unsubscribe(object subscriber)
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

        public void Subscribe<T>(object subscriber, Next<T> @delegate)
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

