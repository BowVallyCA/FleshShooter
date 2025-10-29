
# Refactoring Guide

This document provides a concise checklist for the most critical refactoring tasks. Completing these tasks will establish a solid foundation for the rest of the project.

## Phase 1: Decoupling and Input

**Priority:** High. This should be done before adding any new features.

- [ ] **Replace Raw Input:**
    - Install the `Input System` package.
    - Create an `Input Actions` asset.
    - Create a `PlayerInputHandler.cs` script.
    - Remove all instances of `Input.GetKeyDown`, `Input.GetMouseButtonDown`, etc., from your project.
    - Use the `PlayerInputHandler` to fire events on your event bus.

- [ ] **Implement a Global Event Bus:**
    - Solidify your `EventBus` implementation.
    - Replace all direct C# events (like `PlayerController.OnHealthChanged`) with `EventBus` events.
    - `UiManager` should listen for a `HealthChangedEvent` instead of directly referencing `PlayerController`.

## Phase 2: Component-Based Refactoring

**Priority:** Medium. This can be done incrementally.

- [ ] **Create a `Health` Component:**
    - Create a generic `Health.cs` component.
    - Add it to the Player prefab and remove the health logic from `PlayerController`.
    - Plan to add it to all future enemies and destructible objects.

- [ ] **Break Up `PlayerController`:**
    - Create a `PlayerMotor.cs` for movement logic.
    - Create a `WeaponHolder.cs` for pickup/drop/hold logic.
    - The old `PlayerController` script may become a simple coordinator or be removed entirely.

- [ ] **Create an `IWeapon` Interface:**
    - Define an `IWeapon` interface with an `Attack()` method.
    - Make `FleshCube` implement `IWeapon`.
    - The `WeaponHolder` should hold a reference to an `IWeapon`, not a `FleshCube`.

## Phase 3: Project Organization

**Priority:** Low. This can be done at any time.

- [ ] **Use Namespaces:**
    - Add namespaces to all of your scripts (e.g., `FleshShooter.Player`, `FleshShooter.Weapons`).

- [ ] **Organize Folders:**
    - Move your scripts from `Assets/Custom/SystemsDesign/Scripts` into the main project structure under `Assets/_Project/Code/`.
