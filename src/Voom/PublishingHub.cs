using System;
using System.Collections.Generic;

namespace Voom
{
    public delegate void Next<T>(T value);

    /// <summary>
    /// This is an experimental class. The public interface may change
    /// </summary>
    public class PublishingHub
    {
        private readonly object _lock = new object();
        private readonly Dictionary<Type, Dictionary<object, List<Delegate>>> _handlers = new Dictionary<Type, Dictionary<object, List<Delegate>>>();

        public void Publish<T>(T value)
        {
            if (!_handlers.TryGetValue(typeof(T), out var delegatesBySubscriber)) return;

            foreach (var keyValuePair in delegatesBySubscriber)
            {
                foreach (var @delegate in keyValuePair.Value)
                {
                    var next = (Next<T>)@delegate;
                    next(value);
                }
            }
        }

        public void Unsubscribe(object subscriber)
        {
            lock (_lock)
            {
                foreach (var delegatesBySubscriber in _handlers.Values)
                {
                    if (!delegatesBySubscriber.ContainsKey(subscriber))
                    {
                        throw new InvalidOperationException();
                    }

                    delegatesBySubscriber.Remove(subscriber);
                }
            }
        }

        public void Subscribe<T>(object subscriber, Next<T> @delegate)
        {
            lock (_lock)
            {

                if (!_handlers.TryGetValue(typeof(T), out var delegatesBySubscriber))
                {
                    delegatesBySubscriber = new Dictionary<object, List<Delegate>>();
                    _handlers.Add(typeof(T), delegatesBySubscriber);
                }


                if (!delegatesBySubscriber.TryGetValue(subscriber, out var delegates))
                {
                    delegates = new List<Delegate>();
                    delegatesBySubscriber.Add(subscriber, delegates);
                }

                delegates.Add(@delegate);
            }
        }
    }
}

