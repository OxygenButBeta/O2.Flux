using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace O2.Flux {
    public class SingletonDependencyContainer : IServiceResolver {
        readonly Dictionary<Type, object> SingletonServicesContainer = new();

        public void RegisterServiceAs<T>(object instance) where T : class {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance),
                    $"Trying to register null service of type {typeof(T).Name}");

            if (SingletonServicesContainer.ContainsKey(typeof(T)))
                throw new InvalidOperationException(
                    $"Service of type {typeof(T).Name} is already registered as singleton. Cannot register another instance.");

            SingletonServicesContainer[typeof(T)] = instance;
        }

        public void RegisterService(Type type, object instance) {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance),
                    $"Trying to register null service of type {type.Name}");

            if (!SingletonServicesContainer.TryAdd(type, instance))
                throw new InvalidOperationException(
                    $"Service of type {type.Name} is already registered as singleton. Cannot register another instance.");
        }

        public T ResolveService<T>() where T : class {
            if (!SingletonServicesContainer.TryGetValue(typeof(T), out var service))
                return null;

            return (T)service;
        }

        public object ResolveService(Type type) {
            return SingletonServicesContainer.GetValueOrDefault(type);
        }

        public void RemoveService(Type type) {
            if (!SingletonServicesContainer.Remove(type))
                throw new InvalidOperationException(
                    $"Service of type {type.Name} is not registered as singleton. Cannot remove non-existing service.");
        }
    }


    internal class SingletonService<T> {
        T instance;

        public T Get() {
            return instance ?? throw new FluxCannotResolveServiceException(typeof(T));
        }

        public void RegisterSingletonFactory(T service) {
            instance = service;
        }
    }
}

public interface IServiceResolver {
    public T ResolveService<T>() where T : class;
    public object ResolveService(Type type);
}