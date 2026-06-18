# Project Overview
- **Game Title**: 2D Action/Shooter (Project-Final)
- **High-Level Concept**: A top-down or side-scrolling action game where players and enemies engage in combat.
- **Players**: Single player.
- **Render Pipeline**: UniversalRP.
- **Unity Version**: 6000.4.1f1.

# Game Mechanics
## Core Gameplay Loop
Players and enemies take damage through the `Health` system. When damage is taken, visual feedback (blood splatters) will now be provided.
## Controls and Input Methods
Standard keyboard/mouse or gamepad controls as managed by `PlayerController`.

# UI
- N/A

# Key Asset & Context
- **Scripts**: 
    - `Assets/Scripts/Health.cs`: The core component managing health and damage.
- **Assets**:
    - `Assets/Sprites/Blood.aseprite`: Contains 12 blood splatter sprites (`Blood_0` to `Blood_11`).
- **Sorting Layer**:
    - `Ground`: The layer where blood splatters will be placed to ensure they appear behind characters but above the ground tiles.

# Implementation Steps
## 1. Update Health.cs
- **Description**: Add fields for blood sprites and logic to spawn them on damage.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

## 2. Configure Blood Sprites on Prefabs
- **Description**: Assign the sprites from `Blood.aseprite` to the `Health` component on the following prefabs:
    - `Assets/Prefabs/Player.prefab`
    - `Assets/Prefabs/Enemy.prefab`
    - `Assets/Prefabs/EnemyBat.prefab`
    - `Assets/Prefabs/EnemyPistol.prefab`
    - `Assets/Prefabs/EnemyShotgun.prefab`
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

# Verification & Testing
- **Manual Test**: Play the game and damage the player by running into enemies or getting shot.
- **Manual Test**: Attack and damage enemies.
- **Verification**: 
    - Check that a random blood sprite appears at the hit location.
    - Verify that blood splatters stay on the ground forever.
    - Verify that blood splatters are correctly sorted on the `Ground` layer (Order 1) so they are visible above tiles but below characters.
