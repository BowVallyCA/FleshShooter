
# Code Architecture Recommendations

This document provides recommendations for improving the code architecture of the FleshShooter project. The current architecture is functional for a prototype but will become a significant bottleneck as the project grows.

## 1. Namespace Declaration

**Problem:** All scripts currently reside in the global namespace. This can lead to naming conflicts with third-party assets and makes the codebase harder to navigate.

**Recommendation:** Use namespaces to organize your code logically. A good starting point would be:

- `FleshShooter.Core`: For core systems like the event bus, service locator, and game state manager.
- `FleshShooter.Player`: For all player-related code (controller, health, input).
- `FleshShooter.Weapons`: For weapon-related code.
- `FleshShooter.UI`: For UI-related code.
- `FleshShooter.Gameplay`: For general gameplay mechanics.

**Example:**

```csharp
// PlayerController.cs
namespace FleshShooter.Player
{
    public class PlayerController : MonoBehaviour
    {
        // ...
    }
}
```

## 2. Decoupling with a Service Locator and Event Bus

**Problem:** The project has a mix of event systems (a partially implemented `EventBus` and a C# `Action` in `PlayerController`). This creates confusion and tight coupling. For example, `UiManager` has a direct reference to `PlayerController`.

**Recommendation:** Fully commit to a decoupled architecture using a Service Locator and an Event Bus.

- **Service Locator:** A central place to access global services like the `InputManager`, `GameManager`, and `UIManager`. This avoids singletons and direct references between managers.
- **Event Bus:** A system for broadcasting events throughout the application. This allows different parts of your game to react to events without knowing about each other.

**Example (Event Bus):**

```csharp
// Health.cs
public void TakeDamage(int amount)
{
    // ...
    EventBus.Instance.Publish(new PlayerHealthChangedEvent(currentHealth));

    if (currentHealth <= 0)
    {
        EventBus.Instance.Publish(new PlayerDiedEvent());
    }
}

// UiManager.cs
void OnEnable()
{
    EventBus.Instance.Subscribe<PlayerHealthChangedEvent>(OnPlayerHealthChanged);
}

void OnDisable()
{
    EventBus.Instance.Unsubscribe<PlayerHealthChangedEvent>(OnPlayerHealthChanged);
}

private void OnPlayerHealthChanged(PlayerHealthChangedEvent e)
{
    healthSlider.value = e.NewHealth;
}
```

## 3. Component-Based Design

**Problem:** The `PlayerController` is a "God Class" that handles too many responsibilities: movement, input, health, and weapon handling. This makes the class large, hard to read, and difficult to maintain.

**Recommendation:** Break down large classes into smaller, single-responsibility components.

- **`PlayerInputHandler`**: Responsible for capturing input from the Input System and translating it into game events (e.g., `ShootPressedEvent`, `MoveInputEvent`).
- **`PlayerMotor`**: Responsible for moving the player based on input events.
- **`Health`**: A generic component for any object that can take damage.
- **`WeaponHolder`**: Responsible for picking up, holding, and dropping weapons.

**Example:**

Instead of one large `PlayerController` on your player object, you would have several smaller components:

- `PlayerInputHandler.cs`
- `PlayerMotor.cs`
- `Health.cs`
- `WeaponHolder.cs`

This approach makes your code more modular, reusable, and easier to test.
