using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace O2.Flux {
    public class TransientDependencyContainer : IServiceResolver {
        readonly Dictionary<Type, Func<object>> TransientServicesFactoryContainer = new();

        public void RegisterTransientFactory<TAs, TType>(Func<TAs> factory)
            where TAs : class
            where TType : TAs {
            TransientServicesFactoryContainer[typeof(TAs)] = factory;
        }

        public void RegisterTransient<TAs, TType>()
            where TAs : class
            where TType : TAs, new() {
            TransientServicesFactoryContainer[typeof(TAs)] = (Func<TAs>)Factory;

            TAs Factory() => new TType();
        }

        public void RegisterTransient<TType>()
            where TType : class, new() {
            TransientServicesFactoryContainer[typeof(TType)] = (Func<TType>)Factory;
            TType Factory() => new();
        }

        public T ResolveService<T>() where T : class {
            Func<object> resolveFactory = ResolveFactoryRaw(typeof(T));
            return (T)resolveFactory?.Invoke();
        }

        public object ResolveService(Type type) {
            Func<object> resolveFactory = ResolveFactoryRaw(type);
            return resolveFactory?.Invoke();
        }

        Func<object> ResolveFactoryRaw(Type serviceType) {
            foreach (KeyValuePair<Type, Func<object>> keyValuePair in TransientServicesFactoryContainer) {
                if (keyValuePair.Key != serviceType)
                    continue;

                if (keyValuePair.Value is null) {
                    throw new NullReferenceException("Factory for service type " + serviceType.Name +
                                                     " is null. Cannot resolve service.");
                }

                return keyValuePair.Value;
            }

            return null;
        }
    }
}