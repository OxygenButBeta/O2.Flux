#if FLUX_UNITASK_SUPPORT
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

// A small version of my AsyncHelpers class that only contains the WaitUntilAndExecute method, to avoid a dependency on the entire AsyncHelpers class in the Service class.
namespace O2.Flux.Internal {
    internal static class FluxAsyncHelpers {
        public static void WaitUntilAndExecute(Func<bool> condition, Action action, CancellationToken token = default) {
            WaitUntilAndExecuteAsync(condition, action, token).Forget();
        }

        static async UniTaskVoid WaitUntilAndExecuteAsync(Func<bool> condition, Action action, CancellationToken ct) {
            try {
                await UniTask.WaitUntil(condition, cancellationToken: ct);
                action?.Invoke();
            }
            catch (OperationCanceledException) {
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}
#endif