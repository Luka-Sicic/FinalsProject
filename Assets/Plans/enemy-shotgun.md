# Project Overview
- Game Title: Project-Final
- High-Level Concept: Top-down 2D shooter with A* pathfinding enemies.
- Players: Single player
- Inspiration: Hotline Miami / Top-down shooters
- Target Platform: Standalone Windows
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Player navigates levels, kicks doors, and fights enemies.
- Enemies use A* pathfinding to chase and attack the player.

## Controls and Input Methods
- WASD for movement.
- Mouse to aim and shoot.
- Q to kick.

# UI
- Standard 2D top-down view.

# Key Asset & Context
- `Assets/Sprites/Enemy1Shotgun.aseprite`: The sprite for the new enemy.
- `Assets/Scripts/EnemyAStarFollow.cs`: Existing pathfinding and detection logic.
- `Assets/Scripts/Shotgun.cs`: Pre-existing shotgun weapon script.
- `Assets/Scripts/Health.cs`: Health and death logic.

# Implementation Steps
1. **Create EnemyBullet Script and Prefab**
   - Description: Create `EnemyBullet.cs` that damages objects with `Health` but ignores `Enemy` tag. Create a prefab `EnemyBullet.prefab` using this script.
   - Assigned role: developer
   - Dependencies: None
   - Parallelizable: Yes

2. **Add Health to Player**
   - Description: Attach `Health` component to the Player object in the scene. Set a placeholder `deathSprite`.
   - Assigned role: developer
   - Dependencies: None
   - Parallelizable: Yes

3. **Create EnemyShotgunAI Script**
   - Description: Create `EnemyShotgunAI.cs` inheriting from (or extending) the logic of `EnemyAStarFollow`. Add attack range, fire rate, and logic to fire the shotgun when the player is visible and in range. Ensure the enemy faces the player when attacking.
   - Assigned role: developer
   - Dependencies: None
   - Parallelizable: Yes

4. **Create EnemyShotgun Prefab**
   - Description: Create a new prefab `Assets/Prefabs/EnemyShotgun.prefab` by duplicating `Enemy.prefab`.
     - Update Sprite to `Enemy1Shotgun`.
     - Add `Shotgun` component and assign `EnemyBullet` prefab.
     - Replace `EnemyAStarFollow` with `EnemyShotgunAI`.
     - Set up `firePoint` child object.
   - Assigned role: developer
   - Dependencies: Step 1, Step 3
   - Parallelizable: No

5. **Placement & Verification**
   - Description: Place an `EnemyShotgun` in the scene to test its behavior.
   - Assigned role: developer
   - Dependencies: Step 4
   - Parallelizable: No

# Verification & Testing
- **Movement**: Verify the enemy chases the player using A* pathfinding.
- **Line of Sight**: Verify the enemy only fires when it has a clear line of sight.
- **Range**: Verify the enemy only fires when within the specified attack range.
- **Damage**: Verify the enemy's bullets can damage the player (and kill them if health reaches 0).
- **Death**: Verify the enemy can be killed and spawns a corpse.
