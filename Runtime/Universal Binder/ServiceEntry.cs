using System;
using System.Collections.Generic;
using System.Linq;
using O2.Flux.Internal;
using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Flux {
    [Serializable]
    public class ServiceEntry {
        [BoxGroup("Service", ShowLabel = false)]
        [VerticalGroup("Service/Main")]
        [SerializeReference]
        [TypeFilter(nameof(GetFilteredTypes))]
        [HideLabel]
        public object Instance;

        [HorizontalGroup("Service/Main/Settings")] 
        [ToggleLeft, LabelText("Override")]
        public bool OverrideType;

        [HorizontalGroup("Service/Main/Settings")]
        [ToggleLeft, LabelText("Unbind On Destroy")]
        public bool UnbindOnDestroy = true;

        [ShowIf(nameof(OverrideType))]
        [ValueDropdown(nameof(GetOverrideTypes))]
        [Indent, ShowInInspector, HideLabel]
        [GUIColor(0.8f, 0.9f, 1f)]
        public Type OverrideTypeValue;

        internal void Bind() {
            if (Instance == null) return;

            Type serviceType = GetTargetType();
            FluxReflectionUtility.GetBindingMethod(serviceType).Invoke(null, new[] { Instance });
            FluxUtility.HandleSpecialServiceInitialization(Instance);
        }

        internal void Unbind() {
            if (!UnbindOnDestroy || Instance == null) 
                return;

            Type serviceType = GetTargetType();
            FluxReflectionUtility.GetUnbindingMethod(serviceType).Invoke(null, null);
            FluxUtility.HandleSpecialServiceDestruction(Instance);
        }

        Type GetTargetType() {
            return OverrideType && OverrideTypeValue != null
                ? OverrideTypeValue
                : Instance.GetType();
        }

        IEnumerable<Type> GetFilteredTypes() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System"))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .Where(t => !typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null);
        }

        IEnumerable<Type> GetOverrideTypes() {
            if (Instance == null) yield break;

            Type type = Instance.GetType();
            foreach (Type i in type.GetInterfaces()) 
                yield return i;

            Type baseType = type.BaseType;
            while (baseType != null && baseType != typeof(object)) {
                yield return baseType;
                baseType = baseType.BaseType;
            }

            yield return type;
        }
    }
}