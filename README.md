# O2.Flux

**A straightforward, "glue-code" killer Service Locator and Dependency Inversion tool for Unity.**

Let's cut the fluff: Unity projects get messy because everything is a `MonoBehaviour` and everything depends on everything else. **O2.Flux** is a lightweight tool designed to handle your dependencies without making your life miserable.

<p align="center">
  <img src="https://raw.githubusercontent.com/OxygenButBeta/O2.Flux/main/Editor/Logo.png" width="200">
  <br>
  <sub>
    Dependency Inversion and clean architecture, without the "holy grail" complexity of massive DI frameworks.
  </sub>
</p>
---

# 🎨 The "Lazy Developer" Workflow

Flux isn't just a bunch of scripts; it's designed to keep you out of the IDE as much as possible. We use **Odin Inspector** to turn your project's architecture into a visual dashboard.

## 1. Right-Click Magic (Context Menu)

Stop manually adding components and dragging references.  
Just right-click any `MonoBehaviour` in the Inspector and select **"Bind as Service"**.

- Instantly adds the binder
- Hooks up the reference
- Done in **2 seconds** — no dragging required

---

## 2. The Universal Dashboard

The **UniversalServiceBinder** is your mission control.  
See every pure C# service (**POCO**) in one clean list.

**Searchable & Draggable**

Find or reorder your services instantly.

**Smart Filtering**

The UI only shows interfaces that the class actually implements.  
It won't let you make mistakes.

---

## 3. Visual Validation

If a service slot is empty or a reference is missing, **Odin will glow red and warn you before you even hit Play**.

---

# 🛠 Features

## Hybrid Binding

Supports both **MonoBehaviour components** and **pure C# classes (POCOs)**.

---

## Dependency Inversion (DI)

Don't depend on classes — depend on **interfaces**.

Flux lets you bind a **concrete implementation to an interface directly from the Inspector**.

---

## Automatic Cleanup

Enable **Unbind On Destroy** and Flux will automatically unregister services when the object is destroyed.

No stale references.  
No manual cleanup.

---

## Lifecycle Management

If your POCO needs to behave like a `MonoBehaviour`, simply implement:

```
ITickable
```

Flux gathers all services and runs them inside a **centralized update loop (The Flux Runner)**, eliminating the overhead of thousands of independent Unity `Update()` calls.

---

# 🔥 The "Cool" Part: WaitForService

Execution order in Unity is often painful.  
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

---

# ⚙️ The "Reflection" Elephant in the Room

Yes — Flux uses **reflection during the binding phase**.

When you press **Play**, Flux scans and registers services using:

- `MakeGenericType`
- `Invoke`

Is it the fastest possible method? **No.**

Does it matter? **Also no.**

This process runs **once during initialization**.  
After that, all service access runs at **native C# speed**.

You pay a tiny startup cost for a **massively cleaner architecture and a better developer experience**.

---

# 📦 Requirements & Installation

## Odin Inspector (Required)

The entire editor interface is built using **Odin Inspector**.

It turns service management into a **visual dashboard instead of a manual nightmare**.

---

## UniTask (Optional)

Add the following scripting define symbol to enable high-performance async support with zero allocations:

```
FLUX_UNITASK_SUPPORT
```

---

# ⚡ Quick Example

## Define your service (No MonoBehaviour required)

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
        /* Frame-perfect logic */
    }
}
```

---

## Access it anywhere

```csharp
var movement = Service<IInputService>.Get().GetMovement();
```

---

# 🧠 Summary

**O2.Flux** is a **"set it and forget it"** tool.

It handles the boring parts:

- binding
- lifecycle
- cleanup
- initialization order

So you can focus on writing **actual game logic** instead of chasing references across `MonoBehaviours`.
