using System;

internal class FluxCannotResolveServiceException : Exception {
    public FluxCannotResolveServiceException(Type type) : base(
        $"Cannot resolve service of type {type.Name}. No factory registered for this type.") {
    }
}