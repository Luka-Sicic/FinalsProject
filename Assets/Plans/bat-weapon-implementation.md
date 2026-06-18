# Project Overview
- Game Title: Top-Down Shooter (Final Project)
- High-Level Concept: A top-down action game with multiple weapons and a state-based player controller.
- Players: Single player
- Inspiration / Reference Games: Hotline Miami, Enter the Gungeon
- Tone / Art Direction: 2D Pixel Art (Aseprite style)
- Target Platform: PC (Standalone Windows)
- Screen Orientation / Resolution: Landscape
- Render Pipeline: UniversalRP

# Game Mechanics
## Core Gameplay Loop
The player explores the scene, picks up weapons (Pistol, Shotgun, and now Bat), and uses them to attack.
## Controls and Input Methods
- WASD: Movement
- Mouse: Aiming (Rotation)
- Left Click: Fire/Attack
- E: Interact/Pickup

# UI
- Weapon Pickup Prompt: A canvas prompt appears when near a weapon.

# Key Asset & Context
- `Assets/Sprites/Bat.aseprite`: Sprite for the weapon item.
- `Assets/Sprites/PlayerBat.aseprite`: Sprite for the player holding the bat.
- `Assets/Sprites/PlayerBatAnimation.aseprite`: Animation for the bat swing.
- `Assets/Scripts/Weapon.cs`: Base class for weapons.
- `Assets/Scripts/Bat.cs`: New script for the melee bat weapon.
- `Assets/Animations/PlayerController.controller`: The player's animator controller.

# Implementation Steps
## 1. Create the Bat Script
- **Description**: Implement `Bat.cs` inheriting from `Weapon`.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
```csharp
using UnityEngine;

public class Bat : Weapon
{
    public float damage = 10f;
    public float attackRange = 1.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public string attackTrigger = "batAttack";

    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public override void Fire()
    {
        // Trigger animation
        if (animator != null)
        {
            animator.SetTrigger(attackTrigger);
        }

        // Melee logic (can be expanded later with damage)
        Debug.Log("Bat swung!");
        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
        // foreach(var enemy in hitEnemies) { ... }
    }
}
```

## 2. Configure Animations
- **Description**: Create AnimationClips for holding and swinging the bat.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
- Create `Assets/Animations/PlayerBatHold.anim` using `PlayerBat.aseprite`.
- Create `Assets/Animations/PlayerBatSwing.anim` using `PlayerBatAnimation.aseprite`.

## 3. Update Animator Controller
- **Description**: Add parameters and states for the Bat to `PlayerController.controller`.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No
- Parameters: `HasBat` (Bool), `playerbat` (Trigger), `batAttack` (Trigger).
- State `PlayerBat`: Looping state using `PlayerBatHold` animation.
- State `PlayerBatSwing`: One-shot state using `PlayerBatSwing` animation.
- Transitions:
    - `AnyState` -> `PlayerBat` if `playerbat` trigger is set.
    - `PlayerBat` -> `PlayerBatSwing` if `batAttack` trigger is set.
    - `PlayerBatSwing` -> `PlayerBat` after animation finishes (Has Exit Time).

## 4. Create Bat Prefabs
- **Description**: Create the Bat weapon prefab and the Bat pickup prefab.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 3
- **Parallelizable**: No
- **Bat Prefab**: A GameObject with the `Bat` script.
- **Bat Pickup Prefab**: A GameObject with a `SpriteRenderer` (using `Bat` sprite) and `WeaponPickup` component.
    - `Weapon Prefab`: `Bat` prefab.
    - `Anim Trigger`: `playerbat`.
    - `Anim Bool`: `HasBat`.

## 5. Scene Integration
- **Description**: Place the `BatPickup` prefab into `SampleScene`.
- **Assigned role**: developer
- **Dependencies**: Step 4
- **Parallelizable**: No

# Verification & Testing
- **Visual Check**: Does the Bat pickup look correct in the scene?
- **Interaction Test**: Does the "Press E" prompt appear? Does the player equip the bat and transition to the `PlayerBat` state?
- **Animation Test**: Does the player play the swing animation when left-clicking with the bat equipped?
- **Logic Test**: Does the console log "Bat swung!" when attacking?
