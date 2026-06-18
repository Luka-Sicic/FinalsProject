# Project Overview
- Game Title: Project-Final
- High-Level Concept: Top-down shooter/action game where the player interacts with objects, kicks doors, and fights enemies.
- Players: Single player
- Target Platform: PC (StandaloneWindows64)
- Render Pipeline: URP
- Input System: Both (Legacy and New Input System), but scripts use InputActionReference.

# Game Mechanics
## Core Gameplay Loop
The player navigates levels, interacts with objects (weapons, doors), and completes objectives to reach the level exit.
## Controls and Input Methods
- Movement: WASD/Arrows (handled by PlayerController)
- Interaction: 'E' or specified key (handled by individual scripts via InputActionReference)
- Attack/Kick: Mouse/Keys (handled by PlayerController)

# UI
- Interaction Prompt: A world-space or screen-space canvas that appears when in range of an interactable object.

# Key Asset & Context
- `Assets/Scripts/LevelExit.cs`: New script to handle level transition logic.
- `Assets/Scripts/WeaponPickup.cs`: Reference for the self-contained interaction pattern.
- `Assets/Scripts/PlayerController.cs`: Reference for finding the player and accessing its position.

# Implementation Steps
## 1. Create LevelExit Script
- **Description**: Create a new C# script `Assets/Scripts/LevelExit.cs` that follows the `WeaponPickup` pattern. It will check the distance to the player and wait for the interaction input.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

## 2. Implement Level Transition Logic
- **Description**: In `LevelExit.cs`, implement the `Interact` logic to load the next scene using `SceneManager`. If no next scene exists, return to the Main Menu.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

## 3. Create LevelExit Prefab (Placeholder)
- **Description**: Create a GameObject in the scene (e.g., a simple Sprite or Cube) with the `LevelExit` script attached. Configure the interaction distance and assign the interaction input action.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

# Verification & Testing
- **Manual Test**: Place the `LevelExit` object in `Level1`. Walk up to it. Verify the interaction prompt appears (if implemented). Press the interaction key. Verify the game attempts to load the next scene or returns to the Main Menu if no next scene is found.
- **Edge Case**: Ensure multiple `LevelExit` objects don't conflict (handled by the static `currentInteractable` pattern if used, or just local logic).
- **Edge Case**: Test interaction when exactly at the distance threshold.
