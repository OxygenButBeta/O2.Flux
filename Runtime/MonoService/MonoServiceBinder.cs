using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
[AddComponentMenu("O2.FLUX/Mono Service Binder")]
public class MonoServiceBinder : SerializedMonoBehaviour {
    [Title("Service Configurations")][Searchable]
    [TableList(AlwaysExpanded = true, DrawScrollView = false)] 
    public List<ServiceBindingData> Bindings = new();

    void Awake() {
        foreach (ServiceBindingData binding in Bindings)
            binding.Bind();
    }

    void OnDestroy() {
        foreach (ServiceBindingData binding in Bindings) 
            binding.Unbind();
    }
   
}