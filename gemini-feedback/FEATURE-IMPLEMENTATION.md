
# Feature Implementation Recommendations

This document provides detailed recommendations for implementing key features in a more robust and scalable way.

## 1. Player State Machine

**Problem:** The `PlayerController`'s `Update` method is becoming a complex series of `if` statements. This will get worse as you add more player actions.

**Recommendation:** Implement a simple state machine to manage the player's state. This will make your code cleaner and easier to reason about.

**Example:**

```csharp
// PlayerController.cs
public enum PlayerState
{
    Idle,       // Can move, look for pickups
    Holding,    // Can shoot, throw
    Spawning,   // In the process of spawning a weapon
    Cooldown    // Cannot spawn a weapon
}

private PlayerState currentState;

public void SetState(PlayerState newState)
{
    currentState = newState;
    // You can add OnEnter/OnExit logic here for each state
}

void Update()
{
    switch (currentState)
    {
        case PlayerState.Idle:
            // Handle movement
            // Handle pickup logic
            break;
        case PlayerState.Holding:
            // Handle shooting
            // Handle throwing
            break;
        case PlayerState.Spawning:
            // Disable most actions, play spawning animation
            break;
        case PlayerState.Cooldown:
            // Handle cooldown timer
            break;
    }
}
```

## 2. Decoupled Weapon System

**Problem:** The `PlayerController` directly interacts with the `FleshCube` class. This makes it difficult to add new weapons without modifying the `PlayerController`.

**Recommendation:** Use an interface-based weapon system. The `PlayerController` should only know about the `IWeapon` interface, not the concrete implementation.

**Example:**

```csharp
// IWeapon.cs
public interface IWeapon
{
    void PrimaryAttack();
    void SecondaryAttack();
    void Equip();
    void Unequip();
}

// FleshCube.cs
public class FleshCube : MonoBehaviour, IWeapon
{
    public void PrimaryAttack()
    {
        Shoot();
    }

    public void SecondaryAttack()
    {
        // e.g., throw the cube
    }

    // ... implement Equip/Unequip
}

// WeaponHolder.cs (on the player)
private IWeapon heldWeapon;

public void EquipWeapon(IWeapon weapon)
{
    heldWeapon = weapon;
    heldWeapon.Equip();
}

// In your input handler
private void OnShoot(InputAction.CallbackContext context)
{
    if (heldWeapon != null)
    {
        heldWeapon.PrimaryAttack();
    }
}
```

## 3. Component-Based Health System

**Problem:** The player's health logic is inside the `PlayerController`. This is not reusable for other entities like enemies.

**Recommendation:** Create a generic `Health` component that can be attached to any object.

**Example:**

```csharp
// Health.cs
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDied;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnDied?.Invoke();
        }
    }
}

// In PlayerController, you would remove the health logic.
// To deal damage:
Health enemyHealth = enemy.GetComponent<Health>();
if (enemyHealth != null)
{
    enemyHealth.TakeDamage(25);
}
```
