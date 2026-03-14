using System;
using O2.Flux.Internal;
using UnityEngine;
#pragma warning disable CS0162 // Unreachable code detected

namespace O2.Flux {
    public static class Service<TService> where TService : class {
        public const string BindMethodName = nameof(Bind);
        public const string UnbindMethodName = nameof(Unbind);
        static TService Instance;
        const bool LogWarning = false;
        const bool LogInfo = false;

        public static void Bind(TService service) {
#if UNITY_EDITOR
            if (LogWarning && service == null)
                Debug.Log($"Trying to bind null service of type {typeof(TService).Name}");
#endif
            FluxUtility.HandleSpecialServiceInitialization(service);
            Instance = service;
        }

        public static void Unbind() {
            FluxUtility.HandleSpecialServiceDestruction(Instance);
            Instance = null;
        }

        public static TService Get() {
#if UNITY_EDITOR
            if (LogWarning && Instance == null)
                Debug.Log($"Trying to get null service of type {typeof(TService).Name}");
#pragma warning disable CS0162 // Unreachable code detected
            if (LogInfo) Debug.Log($"Getting service of type {typeof(TService).Name}");
#pragma warning restore CS0162 // Unreachable code detected
#endif
            return Instance;
        }

        public static ServiceReference<TService> GetServiceReference() {
            return new ServiceReference<TService>();
        }
#if FLUX_UNITASK_SUPPORT
        public static void WaitForService(System.Action<TService> onAvailable, bool notifyWhenProvided = false) {
            if (Instance != null) {
                ProviderListener(Instance);
                return;
            }

            FluxAsyncHelpers.WaitUntilAndExecute(() => Get() != null, () => ProviderListener(Get()));

            void ProviderListener(TService service) {
                if (notifyWhenProvided)
                    Debug.Log("Service of type " + typeof(TService).Name + " is now available. Notifying listener.");
                onAvailable(service);
            }
        }

        public static void WaitForServiceReference(Action<ServiceReference<TService>> onAvailable,
            bool notifyWhenProvided = true) {
            WaitForService(service => {
                if (notifyWhenProvided)
                    Debug.Log("Service of type " + typeof(TService).Name +
                              " is now available. Notifying listener with reference.");
                onAvailable(GetServiceReference());
            }, notifyWhenProvided);
        }
#endif
    }
}