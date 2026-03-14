using System;
using O2.Flux;
using UnityEngine;

/// <summary>
/// In This sample the network core script is responsible to initialize the platform manager service.
/// It uses the Service class to wait for the IPlatformManager service to become available,
/// and once it does, it calls the Initialize method on it.
/// This allows the network core to ensure that the platform manager is properly set up before any network operations are performed,
/// demonstrating how services can be used to manage dependencies and initialization order in a modular way.
///
///
/// With dependency inversion, the NetworkCore does not need to know about the specific
/// implementation of the IPlatformManager (e.g., SteamPlatform or EpicGamesPlatform).
/// </summary>
public class NetworkCore : MonoBehaviour {
    // Attribute ile inject yok
    // Scope yok
    // Scope tanımı yok!!
    void Awake() {
        GlobalService.WaitForService<IPlatformManager>(pManager => pManager.Initialize());
        GlobalService.WaitForService<IPlatformManager>(pManager => pManager.Initialize());
    }
}