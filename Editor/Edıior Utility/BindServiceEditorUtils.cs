using UnityEditor;
using UnityEngine;

public static class BindServiceEditorUtils {
    [MenuItem("CONTEXT/Component/Bind as Service")]
    static void BindAsService(MenuCommand command) {
        Component targetComponent = (Component)command.context;
        GameObject targetGameObject = targetComponent.gameObject;
        MonoServiceBinder binder = targetGameObject.AddComponent<MonoServiceBinder>();
        SerializedObject so = new(binder);
        SerializedProperty serviceProp = so.FindProperty("ServiceComponent");
        serviceProp.objectReferenceValue = targetComponent;
        so.ApplyModifiedProperties();
        Debug.Log($"[ServiceBinder] {targetComponent.GetType().Name} Bind Success!", targetGameObject);
        EditorUtility.SetDirty(binder);
    }
}