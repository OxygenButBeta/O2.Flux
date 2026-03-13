using System;
using System.Collections.Generic;
using UnityEngine;

namespace O2.Flux {
    internal class FluxMonoRunner : MonoBehaviour {
        internal readonly List<ITickable> Tickables = new();
        internal readonly List<ILateTickable> lateTickables = new();
        internal readonly List<IFixedTickable> FixedTickables = new();
        internal readonly List<IDestroyable> destroyServices = new();

        void Update() {
            for (var i = Tickables.Count - 1; i >= 0; i--) {
                try {
                    Tickables[i].Tick();
                }
                catch (Exception e) {
                    throw new Exception(
                        $"Exception during Tick of service {Tickables[i].GetType().Name}: {e.Message}", e);
                }
            }
        }

        void LateUpdate() {
            for (var i = lateTickables.Count - 1; i >= 0; i--) {
                try {
                    lateTickables[i].LateTick();
                }
                catch (Exception e) {
                    throw new Exception(
                        $"Exception during LateTick of service {lateTickables[i].GetType().Name}: {e.Message}", e);
                }
            }
        }

        void FixedUpdate() {
            for (var i = FixedTickables.Count - 1; i >= 0; i--) {
                try {
                    FixedTickables[i].FixedTick();
                }
                catch (Exception e) {
                    throw new Exception(
                        $"Exception during FixedTick of service {FixedTickables[i].GetType().Name}: {e.Message}", e);
                }
            }
        }

        void OnDestroy() {
            for (var i = destroyServices.Count - 1; i >= 0; i--) {
                try {
                    destroyServices[i].OnDestroy();
                }
                catch (Exception e) {
                    throw new Exception(
                        $"Exception during OnDestroy of service {destroyServices[i].GetType().Name}: {e.Message}", e);
                }
            }
        }
    }
}