using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using O2.Flux;
using O2.Flux.Internal;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class ServiceBindingData {
    [BoxGroup("ServiceBox", ShowLabel = false)]
    [VerticalGroup("ServiceBox/Main")]
    [Required, AssetSelector, HideLabel]
    [OnValueChanged(nameof(ResetOverride))]
    public MonoBehaviour ServiceComponent;

    [HorizontalGroup("ServiceBox/Main/Settings")]
    [ToggleLeft, LabelText("Override Binding Type"), Tooltip("Manual Type Override")]
    public bool OverrideType;

    [HorizontalGroup("ServiceBox/Main/Settings")]
    [ToggleLeft, LabelText("Unbind on Destroy"), Tooltip("Unbind On Destroy")]
    public bool UnbindOnDestroy = true;

    [VerticalGroup("ServiceBox/Main")]
    [ShowIf(nameof(OverrideType))]
    [ValueDropdown(nameof(GetFilteredTypes))]
    [ShowInInspector, HideLabel]
    [GUIColor(0.8f, 0.9f, 1f)]
    public Type OverrideTypeValue;

    Type constructedType;
    void ResetOverride() => OverrideTypeValue = null;

    public void Bind(BinderBase binder) {
        if (!ServiceComponent)
            return;

        Type serviceType = (OverrideType ? OverrideTypeValue : ServiceComponent.GetType()) ??
                           ServiceComponent.GetType();

        binder.BindToContainer(serviceType, ServiceComponent);
    }

    public void Unbind(BinderBase binder) {
        if (!UnbindOnDestroy || ServiceComponent == null) return;

        Type serviceType = (OverrideType && OverrideTypeValue != null)
            ? OverrideTypeValue
            : ServiceComponent.GetType();

        binder.UnbindInstance(serviceType, ServiceComponent);
    }

    IEnumerable<Type> GetFilteredTypes() {
        if (!ServiceComponent)
            return Enumerable.Empty<Type>();

        Type type = ServiceComponent.GetType();
        var results = new List<Type> { type };
        results.AddRange(type.GetInterfaces());
        results.AddRange(type.GetBaseClasses());

        return results.Distinct();
    }
}