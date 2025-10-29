
# Feature Template

Use this template to plan out new features for the FleshShooter project.

---

### **Feature Name:**

*A short, descriptive name for the feature.*

### **Description:**

*A high-level overview of the feature. What is it, and why is it being added?*

### **Gameplay:**

*Describe the feature from the player's perspective. How will they interact with it? What will they see and hear?*

**Example:**
- *The player presses and holds the 'Spawn' button.*
- *The player's health is drained over time, and a flesh blob begins to form in their hands.*
- *The player can combine the flesh blob with another item to create a weapon.*

### **Implementation Details:**

*A technical plan for implementing the feature. What new scripts will be needed? What existing scripts will be modified? What architectural patterns will be used?*

**Example:**
- *Create a new `FleshSpawner` component on the player.*
- *The `FleshSpawner` will listen for a `SpawnFleshEvent` from the `PlayerInputHandler`.*
- *When the event is received, the `FleshSpawner` will start a coroutine that drains the player's `Health` component and instantiates a `FleshBlob` prefab.*
- *The `FleshBlob` will have a `Combine` method that takes another object as a parameter.*

### **Code Impact:**

*List the specific files that will be created or modified.*

- **New Files:**
    - `FleshSpawner.cs`
    - `FleshBlob.cs`
- **Modified Files:**
    - `PlayerInputHandler.cs` (to add the spawn event)
    - `Health.cs` (to add a `DrainHealth` method)

### **Scope:**

*Define the scope of the feature. What is the minimum viable product (MVP)? What are some "nice to have" additions that can be added later?*

- **MVP:**
    - Player can sacrifice health to spawn a basic `FleshCube` weapon.
- **Nice to Have:**
    - Different types of flesh blobs.
    - Combining flesh blobs with other items to create different weapons.
    - Visual effects for the spawning process.
