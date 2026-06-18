# Project Overview
- **Game Title**: (To be determined)
- **High-Level Concept**: Add a functional Main Menu scene as the entry point of the game, providing access to gameplay and settings.
- **Players**: Single player.
- **Inspiration**: Standard top-down action game menus.
- **Target Platform**: PC (StandaloneWindows64).
- **Render Pipeline**: URP.

# Game Mechanics
## Core Gameplay Loop
The Main Menu serves as the starting point for the player's experience, allowing them to start a new session, resume a previous one (placeholder), adjust options, or exit the game.

## Controls and Input Methods
- **Mouse**: Clicking buttons and interacting with UI elements.
- **Keyboard/Gamepad**: Navigation through the menu using the New Input System's UI action map (Navigate, Submit, Cancel).

# UI
## Layout
- **Title**: Centered at the top of the screen.
- **Main Panel**: A vertical list of four buttons (New Game, Load Game, Settings, Quit) centered on the screen.
- **Settings Panel**: An overlay or separate panel containing a "Back" button to return to the main menu.
- **Styling**: Simple, clean aesthetic matching the existing 2D pixel art style where possible.

# Key Asset & Context
- **Scripts**: 
    - `Assets/Scripts/UI/MainMenuManager.cs`: Main controller for button actions and scene transitions.
- **Scenes**: 
    - `Assets/Scenes/MainMenu.unity`: The new entry-point scene.
- **Existing Assets**:
    - `Assets/InputSystem_Actions.inputactions`: Used for UI navigation.
    - `Assets/Scenes/SampleScene.unity`: The gameplay scene to be loaded.

# Implementation Steps
1. **Create MainMenuManager Script**
   - **Description**: Implement `MainMenuManager` in `Assets/Scripts/UI/`.
   - **Assigned role**: developer
   - **Dependencies**: None
   - **Parallelizable**: Yes
   - **Key Methods**:
     - `NewGame()`: Loads "SampleScene".
     - `LoadGame()`: Logs a "Not implemented" message for now.
     - `OpenSettings(bool open)`: Toggles the visibility of the Settings panel.
     - `QuitGame()`: Exits the application.

2. **Create and Setup Main Menu Scene**
   - **Description**: Create `Assets/Scenes/MainMenu.unity`. Setup Canvas, EventSystem (with InputSystemUIInputModule), and UI hierarchy.
   - **Assigned role**: developer
   - **Dependencies**: Step 1
   - **Parallelizable**: No
   - **Hierarchy**:
     - `Canvas`
       - `Background` (Full screen Image)
       - `MainPanel` (Vertical Layout Group)
         - `TitleText` (TMP)
         - `NewGameButton`
         - `LoadGameButton`
         - `SettingsButton`
         - `QuitButton`
       - `SettingsPanel` (Initially inactive)
         - `SettingsTitle` (TMP)
         - `BackButton`

3. **Wire UI to Script**
   - **Description**: Attach `MainMenuManager` to a GameObject in the scene and link the buttons' `onClick` events to the corresponding methods.
   - **Assigned role**: developer
   - **Dependencies**: Step 2
   - **Parallelizable**: No

4. **Update Build Settings**
   - **Description**: Add `MainMenu` to the build list at index 0 and ensure `SampleScene` follows.
   - **Assigned role**: developer
   - **Dependencies**: Step 2
   - **Parallelizable**: Yes

# Verification & Testing
- **Navigation Test**: Verify that buttons can be selected and highlighted using both mouse and keyboard/gamepad.
- **Transition Test**: Clicking "New Game" should load the "SampleScene".
- **UI Logic Test**: Clicking "Settings" should reveal the Settings panel and hide/disable the main buttons. Clicking "Back" should reverse this.
- **Quit Test**: Clicking "Quit" should trigger a debug log in the console (as `Application.Quit` only works in builds).
