using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Flux {
    public abstract class BinderBase : SerializedMonoBehaviour {
        [SerializeField] bool BindToCustomContainer;

        [ShowIf(nameof(BindToCustomContainer))] [SerializeField]
        FluxInstaller Installer;

        internal void BindToContainer(Type serviceType, object instance) {
            if (instance == null) {
                return;
            }

            if (BindToCustomContainer && Installer) {
                Installer.Container.RegisterInstance(instance);
                return;
            }


            GlobalService.RegisterInstance(serviceType, instance);
        }

        public void UnbindInstance(Type serviceType, object instance) {
            if (BindToCustomContainer) {
                Debug.LogWarning(
                    "Unbinding from custom container is not supported. Please ensure that the installer handles unbinding if necessary.");
                return;
            }

            GlobalService.UnregisterInstance(serviceType, instance);
        }
    }
}