using System;
using System.Reflection;

namespace O2.Flux.Internal {
    internal static class FluxReflectionUtility {
        internal static MethodInfo GetBindingMethod(Type serviceType) {

            Type genericDefinition = typeof(Service<>);
            Type  constructedType = genericDefinition.MakeGenericType(serviceType);

            MethodInfo bindMethod = constructedType.GetMethod(Service<object>.BindMethodName,
                BindingFlags.Static | BindingFlags.Public);

            return bindMethod;
        }
        
        internal static MethodInfo GetUnbindingMethod(Type serviceType) {
            Type genericDefinition = typeof(Service<>);
            Type  constructedType = genericDefinition.MakeGenericType(serviceType);
            MethodInfo unbindMethod = constructedType.GetMethod(Service<object>.UnbindMethodName,
                BindingFlags.Static | BindingFlags.Public);

            return unbindMethod;
        }
    }
}