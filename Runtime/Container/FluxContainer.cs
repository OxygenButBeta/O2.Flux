using System;
using System.Collections.Generic;
using System.Diagnostics;
using O2.Flux.Internal;
using UnityEngine;

namespace O2.Flux {
    public class FluxContainer {
#if UNITY_EDITOR
        internal readonly List<FluxLogEntry> ContainerLogs = new(20);
#endif

        [Conditional("UNITY_EDITOR")]
        void AddDebugInfo(Type type, BindingType binding, string message) {
#if UNITY_EDITOR
            string iconName = binding switch {
                BindingType.Singleton => "d_FilterSelectedOnly",
                BindingType.Multiple => "d_FilterByLabel",
                _ => "d_Refresh"
            };

            ContainerLogs.Add(new FluxLogEntry {
                TypeName = type.Name,
                Binding = binding,
                Message = message,
                Icon = UnityEditor.EditorGUIUtility.IconContent(iconName).image
            });
#endif
        }

        readonly MultipleDependencyContainer multipleContainer = new();
        readonly SingletonDependencyContainer singletonContainer = new();
        readonly TransientDependencyContainer transientContainer = new();

        readonly IReadOnlyList<IServiceResolver> resolvers;

        public FluxContainer() {
            resolvers = new List<IServiceResolver> {
                singletonContainer,
                multipleContainer,
                transientContainer
            };
        }


        public void RegisterInstance<T>(T instance, BindingType bindingType = BindingType.Singleton) where T : class {
            switch (bindingType) {
                case BindingType.Singleton:
                    singletonContainer.RegisterService(typeof(T), instance);
                    AddDebugInfo(typeof(T), bindingType, "Registered singleton service of type " + typeof(T).Name);
                    break;
                case BindingType.Multiple:
                    multipleContainer.RegisterService(instance);
                    AddDebugInfo(typeof(T), bindingType, "Registered multiple service of type " + typeof(T).Name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bindingType), bindingType, null);
            }

            RegisterEntryPoint(instance);
        }

        public void RegisterInstanceAs<T>(object instance, BindingType bindingType = BindingType.Singleton)
            where T : class {
            switch (bindingType) {
                case BindingType.Singleton:
                    singletonContainer.RegisterServiceAs<T>(instance);
                    AddDebugInfo(typeof(T), bindingType, "Registered singleton service of type");
                    break;
                case BindingType.Multiple:
                    multipleContainer.RegisterService(instance);
                    AddDebugInfo(typeof(T), bindingType,
                        "Registered multiple service of type " + typeof(T).Name + " as " + typeof(T).Name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bindingType), bindingType, null);
            }

            RegisterEntryPoint(instance);
        }

        public void RegisterEntryPoint(object instance) {
            FluxUtility.HandleSpecialServiceInitialization(instance);
        }

        public void UnregisterEntryPoint(object instance) {
            FluxUtility.HandleSpecialServiceDestruction(instance);
        }

        public T Resolve<T>() where T : class {
            foreach (IServiceResolver resolver in resolvers) {
                T service = resolver.ResolveService<T>();
                if (service is not null)
                    return service;
            }

            throw new FluxCannotResolveServiceException(typeof(T));
        }

        public object Resolve(Type type) {
            foreach (IServiceResolver resolver in resolvers) {
                var service = resolver.ResolveService(type);
                if (service is not null)
                    return service;
            }

            throw new FluxCannotResolveServiceException(type);
        }

        public IEnumerable<object> ResolveMultiple(Type type) {
            foreach (IServiceResolver resolver in resolvers) {
                if (resolver is MultipleDependencyContainer multiple) {
                    foreach (var x1 in multiple.ResolveMultiple(type)) {
                        yield return x1;
                    }

                    continue;
                }

                object service = resolver.ResolveService(type);
                if (service is not null)
                    yield return service;
            }
        }

        public IEnumerable<T> ResolveMultiple<T>() where T : class {
            foreach (IServiceResolver resolver in resolvers) {
                if (resolver is MultipleDependencyContainer multiple) {
                    foreach (T x1 in multiple.ResolveMultiple<T>()) {
                        yield return x1;
                    }

                    continue;
                }

                T service = resolver.ResolveService<T>();
                if (service is not null)
                    yield return service;
            }
        }


        public void RegisterTransientFactory<TAs, TType>(Func<TAs> factory) where TType : TAs where TAs : class {
            AddDebugInfo(typeof(TAs), BindingType.Multiple, "Registered transient service of type " + typeof(TAs).Name +
                                                            " as " + typeof(TType).Name +
                                                            " with factory");
            transientContainer.RegisterTransientFactory<TAs, TType>(factory);
        }

        public void RegisterTransient<TAs, TType>() where TType : TAs, new() where TAs : class {
            AddDebugInfo(typeof(TAs), BindingType.Multiple, "Registered transient service of type " + typeof(TAs).Name +
                                                            " as " + typeof(TType).Name);
            transientContainer.RegisterTransient<TAs, TType>();
        }

        public void RegisterTransient<TType>() where TType : class, new() {
            AddDebugInfo(typeof(TType), BindingType.Multiple, "Registered transient service of type " + typeof(TType).Name);
            transientContainer.RegisterTransient<TType>();
        }

        public void InjectTo(GameObject gameObject) {
            foreach (MonoBehaviour behaviour in gameObject.GetComponentsInChildren<MonoBehaviour>())
                InjectTo(behaviour);
        }

        public void InjectTo(object @object) {
            FluxReflectionUtility.InjectToObject(@object, this);
        }

        public void InjectTo(MonoBehaviour behaviour) {
            FluxReflectionUtility.InjectToObject(behaviour, this);
        }
    }
}