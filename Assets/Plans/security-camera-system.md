# Project Overview
- **Game Title**: Project-Final
- **High-Level Concept**: 2D top-down stealth/action game where the player avoids or fights enemies.
- **Players**: Single player.
- **Render Pipeline**: Universal Render Pipeline (URP).
- **Target Platform**: PC (StandaloneWindows64).

# Game Mechanics
## Core Gameplay Loop
The player navigates levels, avoiding detection by security cameras and enemies. If detected, the camera alerts nearby enemies to the player's position, increasing the challenge.

## Security Camera Behavior
- **Patrol**: The camera rotates back and forth within a specified angle.
- **Detection**: A procedural vision cone (mesh) represents the camera's field of view.
- **Alert Logic**: If the player remains in the vision cone for 2 seconds, the cone turns red, an alert sound plays, and nearby enemies are notified via the `NoiseManager`.

# UI
- **Detection Progress**: The vision cone color serves as the primary visual indicator (Green -> Red).

# Key Asset & Context
- **Scripts**:
    - `SecurityCamera.cs`: Main controller for rotation and detection logic.
    - `VisionConeVisual.cs`: Procedural mesh generator for the detection filter.
- **Assets**:
    - `Assets/Sprites/Camera.aseprite`: The sprite for the camera unit.
- **Existing Systems**:
    - `NoiseManager.MakeNoise(Vector2, float)`: Used to alert enemies.
    - `INoiseListener`: Interface implemented by enemies to respond to alerts.
    - `LayerMask obstacleLayer`: Used for line-of-sight checks.

# Implementation Steps
## Step 1: Create SecurityCamera Script
- **Description**: Implement the `SecurityCamera` class in `Assets/Scripts/SecurityCamera.cs`. This script will handle:
    - Back-and-forth rotation using `Mathf.Sin`.
    - Player detection using FOV and range checks.
    - A timer that triggers an alert after 2 seconds of continuous detection.
    - Interaction with `NoiseManager` to alert enemies.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## Step 2: Create VisionConeVisual Script
- **Description**: Implement `VisionConeVisual` in `Assets/Scripts/VisionConeVisual.cs`. This script will:
    - Generate a procedural `Mesh` based on FOV and distance.
    - Perform raycasts to clip the mesh against walls (as requested).
    - Expose a method to change the cone color.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## Step 3: Setup Security Camera Prefab
- **Description**: Create a new prefab at `Assets/Prefabs/SecurityCamera.prefab`:
    - Root GameObject with `SpriteRenderer` (using `Camera.aseprite`).
    - Child GameObject "VisionCone" with `MeshFilter`, `MeshRenderer` (using a transparent material), and the `VisionConeVisual` script.
    - Add `SecurityCamera` to the root and link the vision cone.
    - Add an `AudioSource` for the alert sound.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 2
- **Parallelizable**: No

## Step 4: Verification & Testing
- **Description**: 
    - Place the camera in a test scene with obstacles and enemies.
    - Verify rotation limits.
    - Walk into the cone; check if the timer starts and the cone stays green.
    - Stay for >2s; check if the cone turns red and enemies move towards the player.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

# Verification & Testing
- **Manual Test**: Confirm the camera's rotation range matches the inspector settings.
- **Logic Test**: Ensure the detection timer resets if the player leaves the cone before 2 seconds.
- **Integration Test**: Verify `NoiseManager.MakeNoise` correctly triggers `OnHearNoise` on nearby `EnemyMeleeAI` or `EnemyPistolAI` components.
- **Visual Test**: Check that the procedural mesh correctly clips against `obstacleLayer` colliders.
