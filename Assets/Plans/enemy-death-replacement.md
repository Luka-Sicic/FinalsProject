# Project Overview
- Game Title: Top-down Shooter (Project-Final)
- High-Level Concept: A top-down action game where the player shoots enemies.
- Players: Single player
- Target Platform: Standalone Windows
- Render Pipeline: URP

# Game Mechanics
## Enemy Death
When an enemy is hit by a bullet, it should be destroyed and replaced by a static "corpse" object using the `Enemy1Dead` sprite. The corpse must maintain the orientation (rotation) the enemy had at the moment of death.

# Key Asset & Context
- `Assets/Sprites/Enemy1Dead.aseprite`: The sprite to be used for the dead enemy.
- `Assets/Scripts/Health.cs`: New script to handle enemy health and death logic.
- `Assets/Scripts/Bullet.cs`: Existing script to be modified to trigger damage.
- `Assets/Prefabs/Enemy.prefab`: The enemy prefab to be updated.

# Implementation Steps

## 1. Create Health Script
**Description**: Create a generic `Health` script that handles damage and instantiates a corpse on death.
**Assigned role**: developer
**Dependencies**: None
**Parallelizable**: Yes

```csharp
using UnityEngine;

namespace Project.Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 1;
        [SerializeField] private Sprite deathSprite;
        
        private int _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Create a new object for the corpse
            GameObject corpse = new GameObject(name + "_Corpse");
            corpse.transform.position = transform.position;
            corpse.transform.rotation = transform.rotation;
            corpse.transform.localScale = transform.localScale;

            // Add SpriteRenderer and configure it
            SpriteRenderer sr = corpse.AddComponent<SpriteRenderer>();
            sr.sprite = deathSprite;
            
            // Match sorting layer and order from the original object
            SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
            if (originalSR != null)
            {
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;
                sr.color = originalSR.color;
            }

            // Optional: You might want to add a script to the corpse to fade it out or clean it up later
            // corpse.AddComponent<CorpseCleanup>(); 

            Destroy(gameObject);
        }
    }
}
```

## 2. Modify Bullet Script
**Description**: Update `Bullet.cs` to detect collisions with objects that have a `Health` component and deal damage.
**Assigned role**: developer
**Dependencies**: Step 1
**Parallelizable**: No

```csharp
// In Bullet.cs
void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        return;
    }

    // Try to get Health component from the hit object
    var health = collision.gameObject.GetComponent<Project.Scripts.Health>();
    if (health != null)
    {
        health.TakeDamage(1);
    }

    Destroy(gameObject);
}
```

## 3. Update Enemy Prefab
**Description**: Add the `Health` component to the Enemy prefab and assign the `Enemy1Dead` sprite.
**Assigned role**: developer
**Dependencies**: Step 1
**Parallelizable**: No

- Path: `Assets/Prefabs/Enemy.prefab`
- Component to add: `Health`
- `maxHealth` set to 1.
- `deathSprite` set to the `Enemy1Dead` sprite from `Assets/Sprites/Enemy1Dead.aseprite`.

# Verification & Testing
1. **Manual Test**: Play the game and shoot an enemy.
2. **Success Criteria**:
    - The enemy object is removed from the scene.
    - A new object named "Enemy_Corpse" appears at the same location.
    - The corpse shows the `Enemy1Dead` sprite.
    - The corpse is rotated the same way the enemy was facing.
3. **Edge Case**: Ensure multiple enemies can die and leave separate corpses.
