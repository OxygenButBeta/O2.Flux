using System;
using System.Collections.Generic;
using O2.Flux.Internal;
using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Flux {
    [DefaultExecutionOrder(-9999)]
    public abstract class FluxInstaller : MonoBehaviour {
        internal FluxContainer Container;
        [SerializeField, DisableInPlayMode] ContainerScope Scope;
        readonly List<MonoBehaviour> AlreadyInjectedObjects = new(10);


#if UNITY_EDITOR
        [TitleGroup("Flux Debugger", "Container Runtime State", alignment: TitleAlignments.Centered)]
        [ShowInInspector, HideInEditorMode]
        [PropertyOrder(100)]
        List<FluxLogEntry> DependencyRegistry => Container?.ContainerLogs;

        [BoxGroup("Flux Debugger/Stats")]
        [HorizontalGroup("Flux Debugger/Stats/Row")]
        [ReadOnly, ShowInInspector, LabelText("Total Services")]
        int ServiceCount => Container?.ContainerLogs.Count ?? 0;

        [HorizontalGroup("Flux Debugger/Stats/Row")]
        [ReadOnly, ShowInInspector, LabelText("Scope")]
        string CurrentScope => Scope.ToString();

        [InfoBox("Registered dependencies and their binding types.", InfoMessageType.None)]
        [ShowInInspector, HideInEditorMode]
        [Searchable]
        [ListDrawerSettings(
            IsReadOnly = true, 
            ShowFoldout = true, 
            ShowPaging = false, 
            HideAddButton = true)]
        [TableList(AlwaysExpanded = true, DrawScrollView = true, MaxScrollViewHeight = 400)]
        [BoxGroup("Flux Debugger/Registry")]
        private List<FluxLogEntry> DebugTable => Container?.ContainerLogs;
#endif
        void Awake() {
            HandleLifeTime();
            Container = new FluxContainer();
            FluxContainers.RegisterContainer(Container, Scope);
            Initialize(Container);
            if (InjectObjects)
                InjectTargetObjects();
        }

        void HandleLifeTime() {
            switch (Scope) {
                case ContainerScope.Global:
                    DontDestroyOnLoad(gameObject);
                    break;
                case ContainerScope.Scene:
                case ContainerScope.GameObject:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnDestroy() {
            FluxContainers.UnregisterContainer(Container, Scope);
        }

        void InjectTargetObjects() {
            foreach (GameObject target in AutoInjectTargets) {
                foreach (MonoBehaviour behaviour in target.GetComponentsInChildren<MonoBehaviour>()) {
                    if (AlreadyInjectedObjects.Contains(behaviour))
                        continue;

                    AlreadyInjectedObjects.Add(behaviour);
                    Container.InjectTo(behaviour);
                }
            }
        }

        protected abstract void Initialize(FluxContainer Container);

        [SerializeField] bool InjectObjects;

        [SerializeField, ShowIf("InjectObjects")]
        GameObject[] AutoInjectTargets;
    }
}