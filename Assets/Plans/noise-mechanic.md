# Project Overview
- **Game Title**: Project-Final
- **High-Level Concept**: A top-down 2D action shooter where players fight AI enemies using various weapons.
- **Players**: Single player.
- **Tone / Art Direction**: 2D, likely stylized/retro (based on pixel-perfect settings and sprite usage).
- **Target Platform**: Standalone Windows.
- **Render Pipeline**: Universal Render Pipeline (URP).

# Game Mechanics
## Core Gameplay Loop
The player navigates the level, engaging enemies using weapons. Weapons have limited ammo and require reloading. Enemies patrol and chase the player upon detection (FOV and distance).

## Noise Mechanic
Firing a weapon will now alert nearby enemies. This creates a strategic layer where firing a gun might draw unwanted attention from multiple directions, even if the player isn't in their direct line of sight.

# UI
- No major UI changes required, although a "noise radius" visualization could be added for debugging or as a player aid in the future.

# Key Asset & Context
- **`INoiseListener.cs`**: New interface for any object that can respond to noise.
- **`NoiseManager.cs`**: New static utility to handle noise propagation via physics overlap.
- **`Weapon.cs`**: Abstract base to hold the `noiseRadius` property.
- **`EnemyShotgunAI.cs` & `EnemyAStarFollow.cs`**: AI scripts to be updated to implement the interface.

# Implementation Steps

## 1. Define the Noise Interface
- **Description**: Create `INoiseListener.cs` in `Assets/Scripts/`.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
```csharp
public interface INoiseListener {
    void OnHearNoise(UnityEngine.Vector2 sourcePosition);
}
```

## 2. Implement the Noise Manager
- **Description**: Create `NoiseManager.cs` in `Assets/Scripts/`. It will use `Physics2D.OverlapCircleAll` to find potential listeners.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No
```csharp
using UnityEngine;

public static class NoiseManager {
    public static void MakeNoise(Vector2 position, float radius) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (var col in colliders) {
            if (col.TryGetComponent<INoiseListener>(out var listener)) {
                listener.OnHearNoise(position);
            }
        }
    }
}
```

## 3. Update the Weapon System
- **Description**: Add `noiseRadius` to `Weapon.cs`. Update `Pistol.cs` and `Shotgun.cs` to call `NoiseManager.MakeNoise` inside their `Fire()` methods.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No
- **Files**: `Assets/Scripts/Weapon.cs`, `Assets/Scripts/Pistol.cs`, `Assets/Scripts/Shotgun.cs`

## 4. Update Enemy AI
- **Description**: Implement `INoiseListener` in `EnemyShotgunAI.cs` and `EnemyAStarFollow.cs`.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes
- **Logic**:
  - In `OnHearNoise(Vector2 pos)`:
    - If the enemy is NOT currently chasing the player (`_setter.target == null`):
      - Set `_lastSeenPosition = pos`.
      - Set `_isSearching = true`.
      - Update `_aiPath.destination = pos`.
      - Ensure `_aiPath.canMove = true`.

# Verification & Testing
- **Unit Test**: None.
- **Manual Check**:
  1. Place an enemy behind a wall or facing away.
  2. Stand just outside their `chaseRange` but inside the `noiseRadius`.
  3. Fire the weapon.
  4. Verify the enemy stops patrolling and moves toward the player's firing position.
  5. Verify the enemy enters the "Searching" state once they reach the noise source.
- **Edge Case**: Ensure firing while the enemy is already chasing the player doesn't disrupt the chase.
