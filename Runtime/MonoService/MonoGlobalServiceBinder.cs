using System.Collections.Generic;
using O2.Flux;
using Sirenix.OdinInspector;
using UnityEngine;
[HideMonoScript]

[DefaultExecutionOrder(-9999)]
[AddComponentMenu("O2.FLUX/Mono Service Binder")]
public class MonoGlobalServiceBinder : BinderBase {
    [Title("Service Configurations")][Searchable]
    [TableList(AlwaysExpanded = true, DrawScrollView = false)] 
    public List<ServiceBindingData> Bindings = new();

    void Awake() {
        foreach (ServiceBindingData binding in Bindings)
            binding.Bind(this);
    }

    void OnDestroy() {
        foreach (ServiceBindingData binding in Bindings) 
            binding.Unbind(this);
    }
   
}