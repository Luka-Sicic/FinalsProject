# Project Overview
- Game Title: Top-down 2D Shooter/Action Game
- High-Level Concept: An enemy that patrols or waits until the player comes within a specific range, then uses A* pathfinding to chase them.
- Players: Single player
- Inspiration / Reference Games: Standard top-down roguelikes (e.g., Enter the Gungeon, Soul Knight)
- Tone / Art Direction: Pixel art (Aseprite)
- Target Platform: PC
- Screen Orientation / Resolution: Landscape
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Player moves around the map.
- Enemies are idle until the player enters their detection range AND is within their Field of View (FOV).
- Enemies use Raycasting to ensure they have a clear Line of Sight (LoS) to the player (cannot see through walls).
- Once in range and visible, enemies chase the player using pathfinding to navigate around obstacles.

## Controls and Input Methods
- AI-driven (A* Pathfinding Project).
- Detection Logic: Distance Check + FOV Check + Raycast LoS Check.

# UI
- N/A for this task.

# Key Asset & Context
- `Assets/Prefabs/Enemy.prefab`: The existing enemy prefab to be updated.
- `Assets/Scripts/EnemyAStarFollow.cs`: Script to manage detection, chasing logic, and FOV/LoS checks.
- A* Pathfinding Components: `AIPath`, `Seeker`, `AIDestinationSetter`.

# Implementation Steps
## 1. Update the EnemyAStarFollow Script
- **Description**: Modify the script to include:
    - `obstacleLayer`: To identify walls during raycasting.
    - `fovAngle`: To define the "front" detection cone.
    - Logic to determine facing direction (based on sprite flip).
    - Raycast2D to check for obstacles between enemy and player.
    - Angle check for FOV.
    - **Rotation Fix**: Set `aiPath.enableRotation = false` in `Start` and force `transform.rotation = Quaternion.identity` to ensure the enemy stays upright.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## 2. Update Enemy Prefab Components and Settings
- **Description**: 
    - Fix Rotation: Ensure `AIPath.enableRotation` is unchecked in the inspector for the prefab (or handled via script).
    - Assign `Walls` layer (Layer 7) to the `obstacleLayer` field in `EnemyAStarFollow`.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

## 3. Configure A* Settings for 2D
- **Description**: Set `AIPath` to "Orientation: YAxisForward" (or "ZAxisForward" depending on 2D setup) and ensure "Gravity: None" since it's a top-down game. Set "Can Move" to true.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

## 4. Verification
- **Description**: Place the Enemy in the scene and move the Player close to it. Verify the Enemy starts chasing and switches to the Walk animation.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

# Verification & Testing
- **Detection Range**: Verify the enemy only starts moving when the player is within `chaseRange`.
- **Animation**: Ensure `IsWalking` is true when moving and false when stopped.
- **Sprite Flipping**: Ensure the enemy faces the direction of travel.
- **Obstacle Avoidance**: Verify the enemy moves around obstacles defined in the Astar Grid Graph.
