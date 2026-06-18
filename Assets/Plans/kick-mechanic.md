# Project Overview
- Game Title: Project Final
- High-Level Concept: Top-down 2D action game where the player can use weapons, interact with doors, and fight enemies.
- Players: Single player
- Target Platform: Standalone Windows 64
- Render Pipeline: UniversalRP

# Game Mechanics
## Core Gameplay Loop
The player explores the environment, uses weapons (pistol, shotgun, bat) to defeat enemies, and interacts with objects like doors.
## Controls and Input Methods
- Movement: WASD / Arrow Keys (Legacy Input)
- Attack: Left Mouse Button
- **Kick: Q key** (New mechanic)
- Interaction: (Context-dependent, currently doors handle interaction)

# UI
- N/A for this specific task.

# Key Asset & Context
- `Assets/Scripts/PlayerController.cs`: Main script for player movement and actions.
- `Assets/Scripts/Door.cs`: Script handling door behavior, including a `Kick` method.
- `Assets/Scripts/Health.cs`: Script handling health and death for enemies.
- `Assets/Sprites/PlayerKick.aseprite`: Sprite source for the kick animation.

# Implementation Steps
## 1. Update PlayerController.cs
- **Description**: Add kick-related variables and logic to handle the 'Q' key press.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
- **Details**:
    - Add `kickRange`, `kickDamage`, `kickForce`, and `enemyLayers` fields.
    - In `Update()`, check for `Input.GetKeyDown(KeyCode.Q)`.
    - Implement `Kick()` method:
        - Trigger `playerkick` in the Animator.
        - Use `Physics2D.OverlapCircleAll` to find objects in front of the player.
        - If a `Door` is hit, call `door.Kick(direction)`.
        - If an object with `Health` is hit, call `health.TakeDamage(damage)`.

## 2. Configure Player Animator
- **Description**: Add the kick animation to the Player's Animator Controller.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No
- **Details**:
    - Add a Trigger parameter named `playerkick`.
    - Create a new State named `PlayerKick`.
    - Assign the animation clip (from `PlayerKick.aseprite`) to the `PlayerKick` state.
    - Create a transition from any state (or Idle/Walk) to `PlayerKick` using the `playerkick` trigger.
    - Set up a transition back to the movement states after the animation finishes.

## 3. Configure Player Prefab
- **Description**: Set the kick-related variables on the Player prefab.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No
- **Details**:
    - Set `Kick Range` to ~1.2.
    - Set `Kick Damage` to 1 (instantly kills enemies with 1 health).
    - Set `Kick Force` to ~10.
    - Assign `Enemy` and `Obstacle` (for doors) layers to the `Kick Layers` mask.

# Verification & Testing
- Press 'Q' while standing near a door to see if it kicks open.
- Press 'Q' while near an enemy to see if they die.
- Verify that the `PlayerKick` animation plays correctly.
- Ensure the kick has a reasonable range and doesn't hit objects behind the player.
