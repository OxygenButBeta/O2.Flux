using UnityEngine;
using Object = UnityEngine.Object;

namespace O2.Flux {
    internal static class FluxUtility {
        static readonly FluxMonoRunner GlobalRunner;

        static FluxUtility() {
            //Static constructor to ensure the GlobalRunner is initialized before any services are registered
            GameObject runnerObject = new("FluxMonoRunner") {
                hideFlags = HideFlags.HideAndDontSave
            };
            Object.DontDestroyOnLoad(runnerObject);
            GlobalRunner = runnerObject.AddComponent<FluxMonoRunner>();
        }

        internal static void HandleSpecialServiceInitialization(object serviceInstance) {
            if (serviceInstance is ITickable tick)
                GlobalRunner.Tickables.Add(tick);

            if (serviceInstance is IFixedTickable fixedTick)
                GlobalRunner.FixedTickables.Add(fixedTick);

            if (serviceInstance is ILateTickable lateTick)
                GlobalRunner.lateTickables.Add(lateTick);

            if (serviceInstance is IDestroyable destroy)
                GlobalRunner.destroyServices.Add(destroy);
        }

        internal static void HandleSpecialServiceDestruction(object serviceInstance) {
            if (serviceInstance is ITickable tick)
                GlobalRunner.Tickables.Remove(tick);

            if (serviceInstance is IFixedTickable fixedTick)
                GlobalRunner.FixedTickables.Remove(fixedTick);

            if (serviceInstance is ILateTickable lateTick)
                GlobalRunner.lateTickables.Remove(lateTick);

            if (serviceInstance is IDestroyable destroy)
                GlobalRunner.destroyServices.Remove(destroy);
        }
    }
}