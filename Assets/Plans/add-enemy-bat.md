# Project Overview
- Game Title: Project-Final
- High-Level Concept: Top-down 2D action game with A* pathfinding enemies and multiple weapon types.
- Players: Single player
- Inspiration / Reference Games: Hotline Miami style top-down shooters/action games.
- Tone / Art Direction: Pixel art (Aseprite).
- Target Platform: Standalone Windows
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Player explores levels, avoids or kills enemies using melee or ranged weapons.
- Enemies chase the player using A* pathfinding and attack when in range.

## Controls and Input Methods
- Movement: WASD/Arrows (standard for top-down).
- Attack: Mouse/Button (based on Weapon scripts).

# UI
- Standard HUD for health and weapons (not directly modified in this task).

# Key Asset & Context
- `Assets/Scripts/EnemyMeleeAI.cs`: New script for the Bat enemy logic.
- `Assets/Prefabs/EnemyBat.prefab`: New prefab for the Bat enemy.
- `Assets/Animations/EnemyBatController.controller`: New Animator Controller for the Bat.
- Sprites: `Assets/Sprites/Enemy1Bat.aseprite` (Idle), `Assets/Sprites/Enemy1BatSwing.aseprite` (Attack).

# Implementation Steps
1. **Create EnemyMeleeAI Script**:
   - Create `Assets/Scripts/EnemyMeleeAI.cs` based on `EnemyShotgunAI.cs`.
   - Implement melee attack logic using `Physics2D.OverlapCircleAll`.
   - Handle chasing, searching, and patrolling using `AIPath`.
   - Add an `Attack` method that triggers the animation and deals damage.

2. **Setup Animator Controller**:
   - Create `Assets/Animations/EnemyBatController.controller`.
   - Add `IsWalking` (bool) and `Attack` (trigger) parameters.
   - Create `Idle` and `Walk` states using the `Enemy1Bat` sprite.
   - Create an `Attack` state using the `Enemy1BatSwing_Clip` from `Assets/Sprites/Enemy1BatSwing.aseprite`.
   - Set up transitions:
     - `Idle` <-> `Walk` based on `IsWalking`.
     - `AnyState` -> `Attack` based on `Attack` trigger.
     - `Attack` -> `Idle` after the animation completes (using Exit Time).

3. **Create EnemyBat Prefab**:
   - Duplicate `Assets/Prefabs/Enemy.prefab` and name it `EnemyBat.prefab`.
   - Remove the `EnemyAStarFollow` component.
   - Add the `EnemyMeleeAI` component.
   - Assign the `EnemyBatController` to the `Animator`.
   - Configure `EnemyMeleeAI`:
     - `attackRange`: ~1.5 (close proximity).
     - `damage`: 1.
     - `targetLayers`: `Player` layer (9).
     - `attackTrigger`: "Attack".
     - `idleSprite`: `Enemy1Bat`.
   - Update `SpriteRenderer` to use `Enemy1Bat` by default.

4. **Verification**:
   - Place an `EnemyBat` in the scene.
   - Ensure it chases the player when in range.
   - Ensure it stops and attacks when close.
   - Verify damage is dealt to the player.
   - Check if the animation plays correctly.

# Verification & Testing
- **Movement Test**: Verify the Bat uses A* to navigate around walls to reach the player.
- **Attack Range Test**: Check that the Bat only attacks when close enough.
- **Damage Test**: Verify the player's health decreases upon being hit.
- **Animation Test**: Ensure the sprite swaps to the swinging animation during the attack and returns to idle/walk.
- **Stun Test**: Ensure the `Stun` method works (if applicable) as per other enemies.
