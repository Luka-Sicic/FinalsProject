using UnityEngine;
using Project.Scripts;

public class Bat : Weapon
{
    public float damage = 10f;
    public float attackRange = 1.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public string attackTrigger = "batAttack";

    [Header("Bat Audio")]
    public AudioClip swingSound;
    public AudioClip hitSound;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public override void Fire()
    {

        if (audioSource != null && swingSound != null)
        {
            audioSource.PlayOneShot(swingSound);
        }

        if (animator != null)
        {
            animator.SetTrigger(attackTrigger);
        }

        Vector3 point = attackPoint != null ? attackPoint.position : transform.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point, attackRange, enemyLayers);

        Debug.Log($"Bat swung at {point}. Found {hitEnemies.Length} colliders on layers {enemyLayers.value}");

        bool hitSomething = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log($"Hit enemy: {enemy.name}");
                health.TakeDamage(1);
                hitSomething = true;
            }
            else
            {
                Debug.Log($"Hit object {enemy.name} but it has no Health component.");
            }
        }

        if (hitSomething && audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 point = attackPoint != null ? attackPoint.position : transform.position;
        Gizmos.DrawWireSphere(point, attackRange);
    }
}