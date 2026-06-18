# Project Overview
- Game Title: Project-Final
- High-Level Concept: Top-down shooter/action game with character movement and weapon mechanics.
- Players: Single player
- Target Platform: Standalone Windows
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Navigate levels, use weapons (Gun, Pistol, Shotgun) to defeat enemies (Bats, Security Cameras, AI).
## Controls and Input Methods
- Keyboard for movement (WASD/Horizontal/Vertical).
- Mouse for aiming and firing.
- 'R' for reload, 'Q' for kick.

# UI
- Main Menu with "New Game", "Settings", "Quit".
- In-game custom crosshair/aim cursor.

# Key Asset & Context
- `Assets/Sprites/MouseAim.aseprite`: Custom cursor for levels.
- `Assets/Sprites/MouseRegular.aseprite`: Custom cursor for menus.
- `Assets/Scripts/UI/CursorManager.cs`: New script to manage cursor changes.

# Implementation Steps
1. **Create CursorManager Script**:
   - Create `Assets/Scripts/UI/CursorManager.cs`.
   - Implement logic to set the cursor texture and hotspot on `Start`.
   - Assigned role: developer
   - Dependencies: None

2. **Configure Main Menu Cursor**:
   - Open `Assets/Scenes/MainMenu.unity`.
   - Create an empty GameObject named `CursorManager`.
   - Attach the `CursorManager` script.
   - Assign `MouseRegular` to the `cursorTexture` field.
   - Set `hotspot` to `(0, 0)`.
   - Assigned role: developer
   - Dependencies: Step 1

3. **Configure Level Cursor**:
   - Open `Assets/Scenes/Level1.unity`.
   - Create an empty GameObject named `CursorManager`.
   - Attach the `CursorManager` script.
   - Assign `MouseAim` to the `cursorTexture` field.
   - Set `hotspot` to center (e.g., `(16, 16)` if the sprite is 32x32).
   - Assigned role: developer
   - Dependencies: Step 1

4. **Verify Texture Settings**:
   - Ensure `MouseAim` and `MouseRegular` textures have "Texture Type" set to "Cursor" in their Import Settings to ensure proper scaling and transparency.
   - Assigned role: developer
   - Dependencies: None

# Verification & Testing
- Start the game in `MainMenu`. Verify the cursor is `MouseRegular`.
- Click "New Game" to load the level. Verify the cursor changes to `MouseAim`.
- Check that the cursor hotspot is correctly aligned (especially for the aim cursor).
