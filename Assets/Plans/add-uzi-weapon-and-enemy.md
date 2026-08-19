# Project Overview
- Game Title: Top-Down Shooter (Inferred)
- High-Level Concept: Action-oriented top-down shooter where players pick up and use various weapons against enemies.
- Players: Single player
- Inspiration / Reference Games: Hotline Miami style
- Tone / Art Direction: Pixel art (Aseprite sprites)
- Target Platform: Standalone Windows
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
The player explores levels, eliminates enemies using weapons, and collects new weapons dropped by enemies or found in the environment.
## Controls and Input Methods
- Movement: WASD / Vector2 Input Action
- Attack: Mouse Left Click / "Attack" Input Action (WasPressedThisFrame for semi-auto, IsPressed for auto)
- Reload: R / "Reload" Input Action
- Interact: E / "Interact" Input Action (to pick up weapons)

# UI
- Weapon Icon: The Uzi sprite will be displayed in the HUD when equipped.
- Ammo Display: Current ammo / Max ammo (Spare reloads) will be shown.

# Key Asset & Context
- `Uzi.cs`: New script for the Uzi weapon, supporting continuous fire.
- `EnemyUziAI.cs`: New script for the Uzi-wielding enemy, supporting 3-round bursts.
- `PlayerController.cs`: Modified to support automatic weapons and Uzi animations.
- `Weapon.cs`: Modified to add `IsAutomatic` and `FireRate` properties.
- `Assets/Sprites/Uzi.aseprite`: Sprite for the pickup and UI icon.
- `Assets/Sprites/PlayerUzi.aseprite`: Visuals for the player holding the Uzi.
- `Assets/Sprites/Enemy1Uzi.aseprite`: Visuals for the Uzi enemy.
- `UziPickup` Prefab: Interaction object in the world.
- `UziPrefab`: Weapon instance attached to the player.
- `Enemy1Uzi` Prefab: The new enemy unit.

# Implementation Steps
## 1. Weapon System Upgrades
- **Description**: Add `IsAutomatic` and `FireRate` to the base `Weapon` class and `Gun` class to support full-auto weapons.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## 2. Implement Uzi Script
- **Description**: Create `Uzi.cs` inheriting from `Gun`. Set `IsAutomatic` to true, `maxAmmo` to a larger value (e.g., 30), and define a `fireRate` (e.g., 0.1s).
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

## 3. Update Player Controller
- **Description**: Modify `PlayerController.cs` to check `Weapon.IsAutomatic`. Use `attackAction.action.IsPressed()` for auto weapons with a timer based on `Weapon.FireRate`. Add support for `HasUzi` animator bool and `playeruzi` trigger.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

## 4. Implement Enemy Uzi AI
- **Description**: Create `EnemyUziAI.cs` based on `EnemyPistolAI.cs`. Modify the `Shoot()` method to trigger a Coroutine that fires 3 bullets with a small delay between each.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## 5. Create Uzi Prefabs
- **Description**: 
    - Create `UziPrefab`: Copy `PistolPrefab`, replace `Pistol` script with `Uzi` script, set `weaponIcon` to the Uzi sprite.
    - Create `UziPickup`: Copy `PistolPickup`, set `weaponPrefab` to `UziPrefab`, set sprite to Uzi, set `animBool` to "HasUzi" and `animTrigger` to "playeruzi".
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

## 6. Create Enemy Uzi Prefab
- **Description**: Create `Enemy1Uzi` prefab by copying `EnemyPistol`. Replace `EnemyPistolAI` with `EnemyUziAI`. Set `staticSprite` to the `Enemy1Uzi` sprite. In the `Health` component, set `dropPrefab` to `UziPickup`.
- **Assigned role**: developer
- **Dependencies**: Step 4, Step 5
- **Parallelizable**: No

## 7. Register Weapon
- **Description**: Add the new `UziPrefab` to the `allWeaponPrefabs` list in the `Player` prefab's `PlayerController` component.
- **Assigned role**: developer
- **Dependencies**: Step 5
- **Parallelizable**: No

# Verification & Testing
- **Manual Test**: Pick up the Uzi and verify it fires continuously while holding the attack button.
- **Manual Test**: Verify the Uzi UI icon appears correctly.
- **Manual Test**: Kill an `Enemy1Uzi` and verify they fire in bursts and drop the Uzi pickup.
- **Animator Check**: Ensure the animator parameters `HasUzi` (Bool) and `playeruzi` (Trigger) are added to the `PlayerController` animator if not already present.
