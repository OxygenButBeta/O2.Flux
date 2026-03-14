
using System;
using UnityEngine;

[Serializable]
public class EpicGamesPlatform : IPlatformManager {
    public void Initialize() {
        Debug.Log("Epic Games Platform Initialized");
    }
}
