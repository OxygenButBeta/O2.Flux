using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace O2.Flux.Internal {
    internal static class FluxReflectionUtility {

        public static void InjectToObject(object @object, FluxContainer container) {
            var tm = GetMarkedMembers(@object);
            Debug.Log("Inject Targets " + tm.Count());
            foreach (MemberInfo info in GetMarkedMembers(@object)) {
                if (info is MethodInfo methodInfo) {
                    HandleMethodInjection(methodInfo, @object, container);
                }
                else if (info is FieldInfo fieldInfo) {
                    HandleFieldInjection(fieldInfo, @object, container);
                }
            }
        }

        static void HandleFieldInjection(FieldInfo fieldInfo, object instance, FluxContainer container) {
            Type serviceType = fieldInfo.FieldType;
            try {
                var service = container.Resolve(serviceType);
                fieldInfo.SetValue(instance, service);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        static void HandleMethodInjection(MethodInfo methodInfo, object instance, FluxContainer container) {
            ParameterInfo[] args = methodInfo.GetParameters();
            var parameters = new object[args.Length];

            for (var i = 0; i < args.Length; i++) {
                try {
                    parameters[i] = container.Resolve(args[i].ParameterType);
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    return;
                }
            }

            methodInfo.Invoke(instance, parameters);
        }

        static IEnumerable<MemberInfo> GetMarkedMembers(object o) {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (FieldInfo info in o.GetType()
                         .GetFields(bindingFlags)) {
                foreach (CustomAttributeData data in info.CustomAttributes) {
                    if (data.AttributeType == typeof(FluxInject)) {
                        yield return info;
                    }
                }
            }

            foreach (MethodInfo info in o.GetType().GetMethods(bindingFlags)) {
                foreach (CustomAttributeData data in info.CustomAttributes) {
                    if (data.AttributeType == typeof(FluxInject)) {
                        yield return info;
                    }
                }
            }
        }
    }
}