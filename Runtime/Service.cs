using System;
using System.Collections.Generic;
using O2.Flux.Internal;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

namespace O2.Flux {
    public static class GlobalService {
        static readonly SingletonDependencyContainer singletonDependencyContainer = new();

        public static void RegisterInstance<T>(T instance) where T : class {
            singletonDependencyContainer.RegisterServiceAs<T>(instance);
            FluxUtility.HandleSpecialServiceInitialization(instance);
        }

        public static void RegisterInstance(Type type, object instance) {
            singletonDependencyContainer.RegisterService(type, instance);
            FluxUtility.HandleSpecialServiceInitialization(instance);
        }

        public static void UnregisterInstance(Type type, object instance) {
            singletonDependencyContainer.RemoveService(type);
            FluxUtility.HandleSpecialServiceDestruction(instance);
        }

        public static void UnregisterInstance<T>(T instance) {
            singletonDependencyContainer.RemoveService(typeof(T));
            FluxUtility.HandleSpecialServiceDestruction(instance);
        }

        public static void WaitForService<TService>(System.Action<TService> onAvailable,
            bool notifyWhenProvided = false) where TService : class {
            TService service = singletonDependencyContainer.ResolveService<TService>();
            if (singletonDependencyContainer.ResolveService<TService>() != null) {
                ProviderListener(service);
                return;
            }

            FluxAsyncHelpers.WaitUntilAndExecute(() => singletonDependencyContainer.ResolveService<TService>() != null,
                () => ProviderListener(singletonDependencyContainer.ResolveService<TService>()));

            void ProviderListener(TService subService) {
                if (notifyWhenProvided)
                    Debug.Log("Service of type " + typeof(TService).Name + " is now available. Notifying listener.");
                onAvailable(subService);
            }
        }
    }
  
}