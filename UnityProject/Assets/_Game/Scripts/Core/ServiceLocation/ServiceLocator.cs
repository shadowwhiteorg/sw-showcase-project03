using System;
using System.Collections.Generic;

namespace _Game.Core.ServiceLocation
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T service)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
                throw new Exception($"Service of type {type.Name} is already registered.");

            _services[type] = service;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
                return service as T;

            throw new Exception($"Service of type {type.Name} is not registered.");
        }

        public void Unregister<T>()
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
                _services.Remove(type);
        }
    }
}