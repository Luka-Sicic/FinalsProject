# Project Overview
- **Game Title:** Project-Final
- **High-Level Concept:** 2D Action game where the player can pick up different weapons.
- **Players:** Single-player.
- **Tone / Art Direction:** Not specified (2D).
- **Target Platform:** Standalone Windows.
- **Render Pipeline:** Universal Render Pipeline (URP).
- **Input System:** Legacy Input Manager (used in current scripts) and New Input System.

# Game Mechanics
## Core Gameplay Loop
The player moves around the environment and can pick up weapons to fight enemies.
## Controls and Input Methods
- **WASD/Arrows:** Movement (already implemented in `PlayerController`).
- **Left Click:** Fire weapon (already implemented).
- **E Key:** Interact / Pick up weapon (to be implemented).

# UI
- **Interaction Prompt:** A "Press E to Pick Up" text prompt that appears when the player is near a weapon pickup.

# Key Asset & Context
- **Scripts to create:**
    - `WeaponPickup.cs`: A unified script to replace `PistolPickup.cs` and `ShotgunPickup.cs`.
    - `InteractionPrompt.cs`: (Optional) Logic to handle showing/hiding the UI prompt.
- **Scripts to modify:**
    - `PlayerController.cs`: (Optional) Could be modified to handle interaction, but keeping it in the pickup script for now to match the existing architecture.
- **Assets to create:**
    - **Interaction UI Prefab:** A Canvas with a TextMeshPro text element.

# Implementation Steps
1. **Create Unified `WeaponPickup` Script**:
    - Implement distance checking and 'E' key detection.
    - Add fields for `weaponPrefab`, `pickupDistance`, `animTrigger`, and `animBool`.
    - Handle showing and hiding the UI prompt.
    - **File**: `Assets/Scripts/WeaponPickup.cs`
2. **Create Interaction UI Prefab**:
    - Create a Canvas with a `TextMeshProUGUI` element saying "Press E to Pick Up".
    - Save as a prefab.
    - **Asset**: `Assets/Prefabs/InteractionPrompt.prefab`
3. **Refactor Scene Pickups**:
    - In the scene, find `[PistolPickup]` and `[ShotgunPickup]` objects.
    - Remove the old `PistolPickup` and `ShotgunPickup` scripts.
    - Add the new `WeaponPickup` script.
    - Configure the `WeaponPickup` settings for each (e.g., set the `weaponPrefab` to the corresponding weapon prefab).
4. **Final Verification**:
    - Verify that the prompt appears when close to a weapon.
    - Verify that pressing 'E' picks up the weapon.
    - Verify that walking away hides the prompt.
5. **Cleanup**:
    - Delete the redundant `Assets/Scripts/PistolPickup.cs` and `Assets/Scripts/ShotgunPickup.cs`.

# Verification & Testing
- **Distance Test**: Approach a weapon and ensure the prompt appears only within the specified range.
- **Interaction Test**: Press 'E' while in range; ensure the weapon is equipped and the pickup is destroyed.
- **Negative Test**: Press 'E' while out of range; ensure nothing happens.
- **Animation Test**: Ensure the player animator triggers the correct weapon animation upon pickup.
