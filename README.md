# O2.Flux

**A straightforward, "glue-code" killer Service Locator and Dependency Inversion tool for Unity.**

Let's cut the fluff: Unity projects get messy because everything is a `MonoBehaviour` and everything depends on everything else. **O2.Flux** is a lightweight tool designed to handle your dependencies without making your life miserable.

It's built for developers who want **Dependency Inversion** and a clean architecture **without the "holy grail" complexity of massive DI frameworks**.

---

# 🛠 Features

### Hybrid Binding
Supports both **MonoBehaviour components** and **pure C# classes (POCOs)**.

### Dependency Inversion (DI)
Don't depend on classes — depend on **interfaces**.  
Flux lets you bind a **concrete implementation to an interface directly from the Inspector**.

### Automatic Cleanup
Enable **Unbind On Destroy** and Flux will automatically unregister services when the object is destroyed.  
No stale references. No manual cleanup.

### Interface Overriding
One class implementing multiple interfaces?  
Use **Override Binding Type** to control exactly **which interface the service is registered as**.

### Lifecycle Management
If your POCO needs to behave like a `MonoBehaviour`, just implement:

```
ITickable
```

Flux automatically gathers all tickable services and runs them inside a **centralized update loop**.

One Update call to rule them all.

---

# 🔥 The "Cool" Part: WaitForService

Execution order in Unity is often painful.

What happens if **Service A depends on Service B**, but Service B hasn't initialized yet?

Flux solves this with **WaitForService**.

```csharp
void Awake()
{
    Service<SomeService>.WaitForService(OnServiceProvided);
}

void OnServiceProvided(SomeService generator)
{
    // This runs only when SomeService is fully bound and ready
}
```

No more fragile `Awake`, `Start`, or script execution order hacks.

---

# ⚙️ The "Reflection" Elephant in the Room

Yes — Flux uses **reflection during the binding phase**.

When you press **Play**, Flux scans and registers services using reflection:

- `MakeGenericType`
- `Invoke`
- etc.

Is it the fastest possible method?  
No.

Does it matter?

Also no.

This process runs **once during initialization**, and after that **all service access runs at normal C# speed**.

You pay a tiny startup cost for **massively cleaner architecture**.

---

# 📦 Requirements & Installation

### Odin Inspector (Required)

The entire editor interface is built using **Odin Inspector**.  
It turns service management into a **visual dashboard instead of manual setup**.

---

### UniTask (Optional)

Add the following scripting define symbol to enable UniTask integration:

```
FLUX_UNITASK_SUPPORT
```

This unlocks **high-performance async support with zero allocations**.

---

# 📥 Installation

### Install via Git URL

Open **Unity Package Manager**.

Click:

```
+ → Add package from git URL
```

Then paste:

```
https://github.com/YOUR_USERNAME/O2.Flux.git
```

---

# ⚡ Quick Example

### Define your service

No `MonoBehaviour` required.

```csharp
public interface IInputService : IService
{
    Vector2 GetMovement();
}

public class DesktopInput : IInputService, ITickable
{
    public Vector2 GetMovement()
        => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    public void Tick()
    {
        // Frame-perfect logic here
    }
}
```

---

### Access it anywhere

```csharp
var movement = Service<IInputService>.Instance.GetMovement();
```

Simple. Fast. Global access without the mess.

---

# 🧠 Summary

**O2.Flux is a "set it and forget it" tool.**

It handles the boring parts of dependency management:

- binding
- lifecycle
- cleanup
- initialization order

So you can focus on writing **actual game logic** instead of chasing references across `MonoBehaviours`.

Clean architecture in Unity — **without framework hell.**
