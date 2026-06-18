using UnityEngine;
using Project.Scripts;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
