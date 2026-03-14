using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Flux {
    [HideMonoScript]
    [DefaultExecutionOrder(-9999)]
    [AddComponentMenu("O2.FLUX/Universal Service Binder")]
    public partial class UniversalServiceBinder : SerializedMonoBehaviour {
        [Title("Services to Bind")]
        [ListDrawerSettings(
            ShowPaging = false,
            DraggableItems = true,
            ShowItemCount = false, HideAddButton = false)]
        public List<ServiceEntry> Services = new();

        void Awake() => BindAllServices();

        void BindAllServices() {
            foreach (ServiceEntry entry in Services) entry.Bind();
        }

        void OnDestroy() {
            foreach (ServiceEntry entry in Services) entry.Unbind();
        }
    }
}