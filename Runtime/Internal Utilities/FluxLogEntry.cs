#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Flux.Internal {
    public struct FluxLogEntry {
        [HorizontalGroup("Main", 50), HideLabel]
        [PreviewField(40, ObjectFieldAlignment.Center), ReadOnly]
        public Texture Icon;

        [VerticalGroup("Main/Info")]
        [LabelText("Service"), ReadOnly, GUIColor(0.9f, 0.9f, 0.9f)]
        public string TypeName;

        [VerticalGroup("Main/Info")]
        [LabelText("Detail"), ReadOnly, GUIColor(0.7f, 0.7f, 0.7f)]
        public string Message;

        [HorizontalGroup("Main", 80), LabelText("Binding"), ReadOnly]
        public BindingType Binding;
    }
}
#endif