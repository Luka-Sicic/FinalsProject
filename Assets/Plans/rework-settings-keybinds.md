# Project Overview
- Game Title: Project-Final
- High-Level Concept: Top-down shooter/action game with weapon pickups and enemies.
- Players: Single player.
- Render Pipeline: UniversalRP.
- Input System: Transitioning from Legacy to New Input System.

# Game Mechanics
## Core Gameplay Loop
- Move the player with WASD/Arrows.
- Shoot with Mouse 0.
- Kick with Q.
- Reload with R.
- Interact with E to pick up weapons.

## Controls and Input Methods
- Keyboard and Mouse.
- Rebindable keys via the Settings menu.

# UI
- Settings Tab: A scrollable list of actions with their current bindings and buttons to rebind them.

# Key Asset & Context
- `Assets/InputSystem_Actions.inputactions`: The main input action asset.
- `Assets/Scripts/PlayerController.cs`: The player controller script to be updated.
- `Assets/Scripts/WeaponPickup.cs`: The interaction script to be updated.
- `Assets/Scripts/UI/RebindUI.cs`: The script handling individual rebind rows.
- `Assets/Scenes/MainMenu.unity`: The scene containing the settings menu.

# Implementation Steps
1. **Update Input Actions**:
    - Add `Reload` action to `InputSystem_Actions.inputactions`.
    - Remove `Hold` interaction from `Interact` action to make it instant.
    - Assigned role: developer
    - Dependencies: None
    - Parallelizable: No

2. **Refactor PlayerController**:
    - Update `PlayerController.cs` to use the New Input System via `InputActionReference` or `PlayerInput`.
    - Replace `Input.GetAxisRaw`, `Input.GetKeyDown`, etc.
    - Assigned role: developer
    - Dependencies: Step 1
    - Parallelizable: Yes (with Step 3)

3. **Refactor WeaponPickup**:
    - Update `WeaponPickup.cs` to use the New Input System's `Interact` action.
    - Assigned role: developer
    - Dependencies: Step 1
    - Parallelizable: Yes (with Step 2)

4. **Update Settings Menu UI**:
    - Add `Reload` rebind row in `MainMenu.unity` by duplicating an existing one.
    - Ensure all rebind rows in the `SettingsPanel` are correctly configured with their `InputActionReference`.
    - Ensure `InputSettingsManager` correctly initializes and loads overrides.
    - Assigned role: developer
    - Dependencies: Step 1
    - Parallelizable: No

5. **Verify Rebinding**:
    - Test the rebinding feature in Play Mode.
    - Verify persistence across scenes and game restarts.
    - Assigned role: developer
    - Dependencies: Step 2, 3, 4
    - Parallelizable: No

# Verification & Testing
- **Settings Menu**:
    - Open Settings.
    - Click rebind on "Move Up", press a different key.
    - Verify the text updates.
- **Gameplay**:
    - Verify movement works with default and rebound keys.
    - Verify attack works with default and rebound keys.
    - Verify kick, reload, and interact work with default and rebound keys.
- **Persistence**:
    - Rebind a key, quit the game, restart.
    - Verify the rebound key is still active.
