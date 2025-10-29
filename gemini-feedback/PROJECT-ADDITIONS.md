
# Project Additions Recommendations

This document provides recommendations for tools and packages that will significantly improve your development workflow and the quality of your game.

## 1. Unity's Input System

**This is the most important addition you should make.**

**Problem:** Your current input is handled by `Input.GetMouseButtonDown` and `Input.GetKeyDown`. This is hardcoded, difficult to configure, and doesn't support controllers.

**Recommendation:** Use Unity's official "Input System" package. It provides a powerful and flexible way to handle input that is completely decoupled from your game logic.

**Key Benefits:**
- **Rebinding:** Easily create in-game control rebinding menus.
- **Cross-Platform:** Supports keyboard/mouse, gamepads, touch, and more with the same code.
- **Event-Driven:** The system is event-driven, which fits perfectly with a decoupled architecture.

**Implementation Steps:**
1.  Install the "Input System" package from the Unity Package Manager.
2.  Create an "Input Actions" asset in your project.
3.  Define your actions (e.g., "Move", "Look", "Shoot", "Pickup").
4.  Generate a C# class from the asset.
5.  Create a `PlayerInputHandler` script to listen for input events and publish them to your Event Bus.

**Example (`PlayerInputHandler.cs`):**

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Shoot.performed += OnShoot;
        playerControls.Player.Pickup.performed += OnPickup;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Shoot.performed -= OnShoot;
        playerControls.Player.Pickup.performed -= OnPickup;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        // Publish a ShootEvent on your Event Bus
        EventBus.Instance.Publish(new ShootEvent());
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        // Publish a PickupEvent on your Event Bus
        EventBus.Instance.Publish(new PickupEvent());
    }
}
```

## 2. Cinemachine

**Problem:** Your camera is likely a simple parented object. This lacks smoothness and makes effects like camera shake difficult to implement.

**Recommendation:** Use Cinemachine for all your camera work. It's a powerful, modular system for creating dynamic and responsive cameras.

**Key Benefits:**
- **Smooth Follow:** Create smooth, professional-looking follow cameras with ease.
- **Camera Shake:** Implement camera shake with a single line of code using "Impulse".
- **Composition:** Easily frame your shots with rules like "Rule of Thirds".

**Example (Camera Shake):**
1.  Add a `CinemachineVirtualCamera` to your scene and set it to follow your player.
2.  Add a `CinemachineImpulseListener` to the virtual camera.
3.  On your `FleshCube` prefab, add a `CinemachineImpulseSource`.
4.  In your `FleshCube`'s `Explode` method, call:

```csharp
// In FleshCube.cs
private CinemachineImpulseSource impulseSource;

void Awake()
{
    impulseSource = GetComponent<CinemachineImpulseSource>();
}

public void Explode()
{
    // ... your explosion logic
    impulseSource.GenerateImpulse();
}
```

## 3. DOTween

**Problem:** Your animations are likely handled by the Animator, which can be cumbersome for simple UI and object movements.

**Recommendation:** Use DOTween for all procedural animations. It's a fast, lightweight, and incredibly easy-to-use tweening library.

**Key Benefits:**
- **"Juice":** Easily add satisfying animations for weapon recoil, UI feedback, and object interactions.
- **Performance:** Highly optimized for performance.
- **Readability:** Write complex animation sequences in a few lines of clean, readable code.

**Example (Weapon Recoil):**

```csharp
// In your weapon's Shoot() method
using DG.Tweening;

public void Shoot()
{
    // ...
    // Visual recoil
    transform.DOPunchPosition(-transform.forward * 0.2f, 0.15f);

    // Camera recoil
    Camera.main.transform.DOShakePosition(0.1f, 0.1f);
}
```
