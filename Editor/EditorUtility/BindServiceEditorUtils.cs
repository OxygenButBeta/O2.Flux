using UnityEditor;
using UnityEngine;
using System.Linq;

public static class BindServiceEditorUtils {
    [MenuItem("CONTEXT/Component/Bind as Service")]
    static void BindAsService(MenuCommand command) {
        Component targetComponent = (Component)command.context;
        GameObject targetGameObject = targetComponent.gameObject;

        MonoGlobalServiceBinder binder = targetGameObject.GetComponent<MonoGlobalServiceBinder>();
        if (!binder) {
            binder = targetGameObject.AddComponent<MonoGlobalServiceBinder>();
            Undo.RegisterCreatedObjectUndo(binder, "Create MonoServiceBinder");
        }

        if (binder.Bindings.Any(b => b.ServiceComponent == targetComponent)) {
            Debug.LogWarning($"[ServiceBinder] {targetComponent.GetType().Name} is already in list!", targetGameObject);
            return;
        }

        Undo.RecordObject(binder, "Bind Component as Service");

        ServiceBindingData newData = new() {
            ServiceComponent = targetComponent as MonoBehaviour,
            UnbindOnDestroy = true
        };
        
        binder.Bindings.Add(newData);

        EditorUtility.SetDirty(binder);
    }
}