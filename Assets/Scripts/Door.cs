using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public bool isLocked = false;
    public float kickForce = 500f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _kickSound;

    private Rigidbody2D rb;
    private HingeJoint2D hinge;
    private JointAngleLimits2D closedLimits;
    private JointAngleLimits2D openLimits;
    private float lethalTimer = 0f;
    private bool _hasBeenOpened = false;

    void Start()
{
        rb = GetComponent<Rigidbody2D>();
        hinge = GetComponent<HingeJoint2D>();

        if (hinge != null)
        {
            closedLimits = hinge.limits;
        }

        if (isLocked)
        {
            LockDoor();
        }
    }

    void Update()
    {
        if (lethalTimer > 0)
        {
            lethalTimer -= Time.deltaTime;
        }
    }

    public void Interact()
    {
        if (_hasBeenOpened) return;

        if (isLocked)
        {
            UnlockDoor();
            Debug.Log("Door unlocked!");
        }
    }

    public string GetInteractPrompt()
    {
        if (_hasBeenOpened) return "";
        return isLocked ? "Unlock Door" : "";
    }

    public void LockDoor()
    {
        isLocked = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        _hasBeenOpened = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void Kick(Vector2 kickerPosition)
    {
        if (_hasBeenOpened) return;

        if (_audioSource != null && _kickSound != null)
        {
            _audioSource.PlayOneShot(_kickSound);
        }

        lethalTimer = 1.0f;
        if (isLocked) UnlockDoor();
        _hasBeenOpened = true;

        if (rb != null)
        {

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.WakeUp();

            Vector2 doorCenter = rb.worldCenterOfMass;
            Vector2 direction = (doorCenter - kickerPosition).normalized;

            rb.AddForceAtPosition(direction * kickForce, doorCenter, ForceMode2D.Impulse);

            Debug.Log("[Door] Kicked door: " + name + " with force " + kickForce);

            CheckImmediateCollisions();
        }
    }

    private void CheckImmediateCollisions()
    {
        Collider2D[] myColliders = GetComponents<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        filter.useTriggers = false;

        List<Collider2D> results = new List<Collider2D>();
        foreach (var col in myColliders)
        {
            int count = col.Overlap(filter, results);
            for (int i = 0; i < count; i++)
            {
                if (results[i].gameObject != gameObject)
                {
                    HandleLethalCollision(results[i].gameObject);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (lethalTimer > 0)
        {
            HandleLethalCollision(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lethalTimer > 0)
        {
            HandleLethalCollision(collision.gameObject);
        }
    }

    private void HandleLethalCollision(GameObject other)
    {
        Debug.Log("[Door] Collision while lethal with: " + other.name + " (Layer: " + LayerMask.LayerToName(other.layer) + ")");

        if (other.GetComponentInParent<Door>() != null)
        {
            Debug.Log("[Door] Ignored (Door)");
            return;
        }

        if (other.CompareTag("Player") || other.GetComponentInParent<PlayerController>() != null)
        {
            Debug.Log("[Door] Ignored (Player)");
            return;
        }

        Project.Scripts.Health health = other.GetComponentInParent<Project.Scripts.Health>();
        if (health != null)
        {
            health.TakeDamage(999);
            Debug.Log("[Door] Enemy smashed by door: " + other.name);
        }
        else
        {
            Debug.Log("[Door] No Health component found on: " + other.name);
        }
    }
}