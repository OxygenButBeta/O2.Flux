using System;
using UnityEngine;

[Serializable]
public class SteamPlatform : IPlatformManager {
    public void Initialize() {
        Debug.Log("Steam Platform Initialized");
    }
}