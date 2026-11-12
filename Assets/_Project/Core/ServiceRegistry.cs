using System;
using System.Collections.Generic;

namespace Loveverse.Core
{
    /// <summary>
    /// Type-safe service container providing decoupled dependency injection for LOVEVERSE systems.
    /// </summary>
    public sealed class ServiceRegistry
    {
        private static ServiceRegistry _instance;
        public static ServiceRegistry Instance => _instance ??= new ServiceRegistry();

        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Reset()
        {
            _instance = new ServiceRegistry();
        }

        public void Register<TInterface>(TInterface implementation) where TInterface : class
        {
            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            var type = typeof(TInterface);
            if (_services.ContainsKey(type))
            {
                _services[type] = implementation;
            }
            else
            {
                _services.Add(type, implementation);
            }
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            var type = typeof(TInterface);
            if (_services.TryGetValue(type, out var service))
            {
                return (TInterface)service;
            }

            throw new KeyNotFoundException($"Service of type {type.Name} is not registered in ServiceRegistry.");
        }

        public bool TryResolve<TInterface>(out TInterface service) where TInterface : class
        {
            var type = typeof(TInterface);
            if (_services.TryGetValue(type, out var obj))
            {
                service = (TInterface)obj;
                return true;
            }

            service = null;
            return false;
        }

        public void Unregister<TInterface>() where TInterface : class
        {
            var type = typeof(TInterface);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
        }

        public void Clear()
        {
            _services.Clear();
        }
    }
}
