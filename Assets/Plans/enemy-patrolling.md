# Project Overview
- Game Title: Top-down 2D Shooter (based on scripts)
- High-Level Concept: Enemies use A* Pathfinding to chase and attack the player.
- Players: Single player
- Inspiration / Reference Games: Hotline Miami / Top-down shooters
- Target Platform: PC (StandaloneWindows64)
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Player explores the level.
- Enemies patrol defined paths.
- If an enemy sees the player, they chase and attack (melee or shotgun).
- If the player escapes and the enemy loses sight, they return to patrolling.

## Controls and Input Methods
- WASD for movement, Mouse for aiming and shooting (Standard top-down).

# UI
- N/A (Feature addition)

# Key Asset & Context
- `EnemyAStarFollow.cs`: Handles basic chasing AI.
- `EnemyShotgunAI.cs`: Handles shotgun-wielding AI.
- `AIDestinationSetter`: A* Pathfinding component that sets the destination.
- `AIPath`: A* Pathfinding component that handles movement.

# Implementation Steps

## Step 1: Update EnemyAStarFollow with Patrolling Logic
- Add `patrolPoints` (Transform array), `patrolWaitTime`, and `_currentPatrolIndex`.
- Implement a state check: If the player is not visible and memory has expired, enter Patrolling mode.
- In Patrolling mode, set `_setter.target` to `null` and manually update `_aiPath.destination` to the current patrol point.
- Increment the patrol index once the destination is reached, after the wait time.
- **File**: `Assets/Scripts/EnemyAStarFollow.cs`
- **Assigned role**: developer
- **Dependencies**: None

## Step 2: Update EnemyShotgunAI with Patrolling Logic
- Similar to Step 1, add `patrolPoints`, `patrolWaitTime`, and index tracking.
- Update the `Update` loop to switch between Chasing and Patrolling.
- **File**: `Assets/Scripts/EnemyShotgunAI.cs`
- **Assigned role**: developer
- **Dependencies**: None

## Step 3: Verify and Test
- Create empty GameObjects in the scene to act as patrol points.
- Assign these points to an enemy in the Inspector.
- Observe the enemy moving between points.
- Approach the enemy to trigger chasing.
- Hide behind an obstacle to observe the enemy returning to patrol after the memory time.

# Verification & Testing
- **Patrol Loop**: Check if the enemy correctly cycles through all points.
- **Detection**: Verify the enemy breaks patrol immediately when seeing the player.
- **Resume Patrol**: Verify the enemy returns to the nearest or next patrol point after losing the player.
- **Edge Case**: No patrol points assigned (should stand still or use original behavior).
