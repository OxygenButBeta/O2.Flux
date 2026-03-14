using System.Collections.Generic;
using System.Linq;

namespace O2.Flux {
    public static class FluxContainers {
        static readonly Dictionary<ContainerScope, List<FluxContainer>> AllContainers = new();

        public static void RegisterContainer(FluxContainer container, ContainerScope scope) {
            if (AllContainers.TryGetValue(scope, out List<FluxContainer> containers))
                containers.Add(container);
            else
                AllContainers[scope] = new List<FluxContainer> { container };
        }

        public static void UnregisterContainer(FluxContainer container, ContainerScope scope) {
            if (AllContainers.TryGetValue(scope, out List<FluxContainer> containers))
                containers.Remove(container);
        }

        public static IEnumerable<FluxContainer> GetContainersByScope(ContainerScope scope) {
            return AllContainers.TryGetValue(scope, out List<FluxContainer> containers)
                ? containers
                : Enumerable.Empty<FluxContainer>();
        }
    }
}