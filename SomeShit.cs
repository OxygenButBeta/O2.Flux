using UnityEngine;

public class SomeShit : MonoBehaviour {
    [FluxInject] IPlatformManager platformManager;

    [FluxInject]
    void SomeMethod(IPlatformManager splatformManager) {
        Debug.Log("Injected method: " + splatformManager);
        splatformManager.Initialize();
        Debug.Log(platformManager);
    }
}