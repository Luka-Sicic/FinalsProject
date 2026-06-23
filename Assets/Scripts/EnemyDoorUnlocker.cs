using UnityEngine;
using Pathfinding;

namespace Project.Scripts
{

    public class EnemyDoorUnlocker : MonoBehaviour
    {
        private AIPath _aiPath;

        private void Awake()
        {
            _aiPath = GetComponent<AIPath>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {

            if (_aiPath != null && !_aiPath.canMove) return;

            Door door = collision.gameObject.GetComponent<Door>();

            if (door != null && door.isLocked)
            {

                door.UnlockDoor();
                Debug.Log($"[EnemyDoorUnlocker] {name} unlocked door: {door.name}");
            }
        }
    }
}