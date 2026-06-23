# Project Overview
- Game Title: Project-Final
- High-Level Concept: A top-down shooter/action game with weapon pickups and level progression.
- Players: Single player
- Inspiration / Reference Games: Hotline Miami (top-down action)
- Tone / Art Direction: Pixel art / Stylized action
- Target Platform: PC (StandaloneWindows64)
- Screen Orientation / Resolution: Landscape
- Render Pipeline: URP
- Input System: Both (Legacy + New)

# Game Mechanics
## Core Gameplay Loop
The player navigates levels, fights enemies using various weapons (Pistol, Shotgun, Bat), and progresses through exits to subsequent levels. The game saves progress (level and current weapon).
## Controls and Input Methods
- WASD for movement.
- Mouse to aim and shoot.
- 'E' to interact/kick.
- 'R' to reload.
- 'Esc' to pause.

# UI
## Load Game Menu
A new panel (`LoadGamePanel`) will be added to the Main Menu. It will contain:
- A scrollable or fixed list of 4 save slots.
- Each slot shows: "Slot [ID] - [Level Name] - [Date]".
- Each slot has a "Play" button (loads the save) and a "Delete" button (removes the save).
- A "Back" button to return to the Main Panel.

## New Game Confirmation
A popup panel (`ConfirmationPanel`) will appear if the player clicks "New Game" when 4 saves already exist. It will inform the player that the oldest save will be overwritten and ask for confirmation.

# Key Asset & Context
- `Assets/Scripts/GameSaveManager.cs`: Main save logic (refactored for slots).
- `Assets/Scripts/UI/MainMenuManager.cs`: Main menu navigation logic.
- `Assets/Scripts/UI/SaveSlotUI.cs`: (New) Manages individual slot UI elements.
- `Assets/Prefabs/UI/SaveSlotPrefab.prefab`: (New) UI prefab for a save slot.
- `Assets/Prefabs/UI/ConfirmationPanel.prefab`: (New) UI prefab for the overwrite warning.

# Implementation Steps
## 1. Refactor GameSaveManager
- **Description**: Update `GameSaveManager` to support 4 slots using a JSON-serialized `SaveSystemData` class. Implement logic to track `currentSlotId`, find the oldest save, and handle loading/deleting specific slots.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## 2. Create SaveSlotUI Script
- **Description**: Create a script to bridge the UI prefab and the `GameSaveManager`. It should have a `Setup(SaveSlot data)` method and listeners for Play and Delete buttons.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

## 3. Create UI Prefabs
- **Description**: Create `SaveSlotPrefab` and `ConfirmationPanel` prefabs in the project. The `SaveSlotPrefab` will be used in the `LoadGamePanel`.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: Yes

## 4. Update MainMenuManager
- **Description**: Add references to the new panels and prefabs. Implement `OpenLoadMenu()` to populate the slot list. Update `NewGame()` to check slot counts and show the confirmation popup if necessary. Update `LoadGame()` to open the new menu.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

## 5. Update PauseMenuManager & LevelExit
- **Description**: Ensure that calls to `SaveGame` and `SaveWeapon` use the `currentSlotId` managed by `GameSaveManager`.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

# Verification & Testing
- **Multi-slot check**: Start 4 new games and verify they appear as 4 distinct slots in the Load Menu.
- **Oldest deletion check**: Start a 5th new game, confirm the overwrite, and verify the oldest save is gone.
- **Loading check**: Load a specific slot and verify the level and weapon are restored correctly.
- **Deletion check**: Delete a save from the menu and verify the slot becomes empty or disappears.
- **Confirmation check**: Ensure the "New Game" warning only appears when all 4 slots are full.
