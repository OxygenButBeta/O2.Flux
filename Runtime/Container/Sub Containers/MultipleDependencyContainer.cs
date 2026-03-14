using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace O2.Flux {
    public class MultipleDependencyContainer : IServiceResolver {
        readonly Dictionary<Type, List<object>> SingletonServicesContainer = new();

        public void RegisterService<T>(T instance, bool CheckContainerForExisting = true) {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance),
                    $"Trying to register null service of type {typeof(T).Name}");

            List<object> x = GetTypedListOrCreate(typeof(T));
            if (CheckContainerForExisting && x.Count > 0) {
                if (x.Contains(instance)) {
                    throw new InvalidOperationException(
                        $"Service of type {typeof(T).Name} is already registered as singleton. Cannot register another instance.");
                }
            }

            x.Add(instance);
        }

        internal IEnumerable<T> ResolveMultiple<T>() {
            if (!SingletonServicesContainer.TryGetValue(typeof(T), out List<object> servicesList))
                yield break;

            foreach (var service in servicesList)
                yield return (T)service;
        }

        internal IEnumerable<object> ResolveMultiple(Type type) {
            if (!SingletonServicesContainer.TryGetValue(type, out List<object> servicesList))
                yield break;

            foreach (var service in servicesList)
                yield return service;
        }

        List<object> GetTypedListOrCreate(Type type) {
            if (SingletonServicesContainer.TryGetValue(type, out List<object> servicesList))
                return servicesList;

            servicesList = new List<object>();
            SingletonServicesContainer[type] = servicesList;

            return servicesList;
        }

        public T ResolveService<T>() where T : class {
            return ResolveMultiple<T>().FirstOrDefault();
        }

        public object ResolveService(Type type) {
            return ResolveMultiple(type).FirstOrDefault();
        }
    }
}