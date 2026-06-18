# Project Overview
- Game Title: Project Final
- High-Level Concept: Top-down 2D action game with kick mechanics and environmental interactions.
- Players: Single player
- Target Platform: Standalone Windows 64

# Game Mechanics
## Doors Rework
- Doors are currently too violent when kicked (high force).
- Doors should become non-interactable once they have been opened or kicked.

# Key Asset & Context
- `Assets/Scripts/Door.cs`: Handles door physics and interaction.
- `Assets/Prefabs/Door_Physics.prefab`: The prefab for doors in the scene.

# Implementation Steps
## 1. Modify Door.cs Logic
- **Description**: Add a state check to prevent multiple interactions and mark the door as opened.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
- **Details**:
    - Add a private boolean `_hasBeenOpened`.
    - Update `Interact()` to return early if `_hasBeenOpened` is true.
    - Update `GetInteractPrompt()` to return an empty string if `_hasBeenOpened` is true.
    - Set `_hasBeenOpened = true` inside `UnlockDoor()` and `Kick()`.

## 2. Tune Door Physics on Prefab
- **Description**: Adjust the force and damping on the door prefab to make it feel more realistic and less violent.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes
- **Details**:
    - Open `Assets/Prefabs/Door_Physics.prefab`.
    - Reduce `Kick Force` from 500 to **12f**.
    - Increase `Angular Damping` on the `Rigidbody2D` to **5f** to slow down the swing naturally.

# Verification & Testing
- Kicking a door should make it swing open at a reasonable speed.
- After kicking a door, verify that no interaction prompt appears.
- After unlocking a door manually, verify that it is no longer interactable.
- Ensure the "lethal" effect on enemies still works with the lower speed (may require tuning the lethal timer if the door takes longer to hit the enemy).
