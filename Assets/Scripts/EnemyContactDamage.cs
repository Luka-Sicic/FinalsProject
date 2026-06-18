using UnityEngine;
using Project.Scripts;

namespace Project.Scripts
{
    public class EnemyContactDamage : MonoBehaviour
    {
        public int damage = 1;
        public float damageInterval = 1f;
        
        private float _lastDamageTime;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (Time.time >= _lastDamageTime + damageInterval)
                {
                    if (collision.gameObject.TryGetComponent<Health>(out var health))
                    {
                        health.TakeDamage(damage);
                        _lastDamageTime = Time.time;
                        Debug.Log($"[EnemyContactDamage] Dealt {damage} damage to Player.");
                    }
                }
            }
        }
    }
}
