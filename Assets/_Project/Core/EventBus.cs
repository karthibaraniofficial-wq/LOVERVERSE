using System;
using System.Collections.Generic;

namespace Loveverse.Core
{
    /// <summary>
    /// Thread-safe publish/subscribe event bus for decoupled communication between game features.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Subscribers = new Dictionary<Type, List<Delegate>>();
        private static readonly object LockObj = new object();

        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            var type = typeof(T);

            lock (LockObj)
            {
                if (!Subscribers.TryGetValue(type, out var list))
                {
                    list = new List<Delegate>();
                    Subscribers[type] = list;
                }
                list.Add(handler);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            var type = typeof(T);

            lock (LockObj)
            {
                if (Subscribers.TryGetValue(type, out var list))
                {
                    list.Remove(handler);
                    if (list.Count == 0)
                    {
                        Subscribers.Remove(type);
                    }
                }
            }
        }

        public static void Publish<T>(T eventArgs)
        {
            List<Delegate> listCopy;
            var type = typeof(T);

            lock (LockObj)
            {
                if (!Subscribers.TryGetValue(type, out var list) || list.Count == 0)
                {
                    return;
                }
                listCopy = new List<Delegate>(list);
            }

            foreach (var del in listCopy)
            {
                if (del is Action<T> action)
                {
                    try
                    {
                        action.Invoke(eventArgs);
                    }
                    catch (Exception ex)
                    {
                        Lg.Error($"Exception during handling of event {typeof(T).Name}: {ex.Message}");
                    }
                }
            }
        }

        public static void Clear()
        {
            lock (LockObj)
            {
                Subscribers.Clear();
            }
        }
    }
}
