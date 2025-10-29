
# Project Plan

This document outlines a high-level project plan to guide the development of FleshShooter, focusing on refactoring the existing codebase and building a solid foundation for future features.

## Phase 1: Refactoring and Foundation (1-2 weeks)

**Goal:** To clean up the existing codebase, establish a solid architecture, and prepare for future development.

**Tasks:**

1.  **Integrate Unity's Input System:**
    - [ ] Install the Input System package.
    - [ ] Create an Input Actions asset with all necessary player actions.
    - [ ] Create a `PlayerInputHandler` script to replace all `Input.Get...` calls.

2.  **Refactor `PlayerController`:**
    - [ ] Break `PlayerController` into smaller components: `PlayerMotor`, `WeaponHolder`, etc.
    - [ ] Implement a simple player state machine (`PlayerState`).

3.  **Implement a Generic Health System:**
    - [ ] Create a `Health` component.
    - [ ] Replace the health logic in `PlayerController` with the `Health` component.
    - [ ] Update `UiManager` to use the `Health` component's events.

4.  **Establish a Decoupled Architecture:**
    - [ ] Fully implement and use an `EventBus` for communication between systems.
    - [ ] Use namespaces for all scripts.
    - [ ] Organize project files according to the established structure.

## Phase 2: Core Gameplay Loop (2-3 weeks)

**Goal:** To implement the core gameplay loop in a robust and extensible way, based on the new architecture.

**Tasks:**

1.  **Weapon System Overhaul:**
    - [ ] Create an `IWeapon` interface.
    - [ ] Refactor `FleshCube` to implement `IWeapon`.
    - [ ] The `WeaponHolder` should be able to equip and use any `IWeapon`.

2.  **Flesh Mechanic:**
    - [ ] Implement the core mechanic of sacrificing health to create weapons/ammo.
    - [ ] Create a system for combining different "flesh" components to create different effects.

3.  **Basic Enemy AI:**
    - [ ] Create a simple enemy that can move towards the player and attack.
    - [ ] The enemy should use the `Health` component.

4.  **Game State Manager:**
    - [ ] Create a `GameManager` to handle game states (e.g., `MainMenu`, `Playing`, `Paused`, `GameOver`).
    - [ ] Move the "lose screen" logic from `UiManager` to the `GameManager`.

## Phase 3: Expansion (Ongoing)

**Goal:** To expand the game with more content and features.

**Tasks:**

1.  **More Weapons:**
    - [ ] Design and implement new weapon types that implement `IWeapon`.

2.  **More Enemies:**
    - [ ] Design and implement new enemy types with different behaviors.

3.  **Level Design:**
    - [ ] Create one or more levels for the game.

4.  **UI/UX Polish:**
    - [ ] Add menus, HUD improvements, and other UI elements.
    - [ ] Use DOTween to add satisfying animations and feedback.

5.  **Audio:**
    - [ ] Add sound effects and music.
