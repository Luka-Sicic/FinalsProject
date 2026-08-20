using UnityEngine;
using UnityEngine.InputSystem;
using Project.Scripts;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Weapon weapon;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Settings")]
    public bool loadWeaponOnStart = true;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject[] allWeaponPrefabs;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference kickAction;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private InputActionReference interactAction;

    [Header("Stealth Settings")]
    public Sprite stealthSprite;
    public float stealthAttackDamage = 1f;
    public float stealthAttackRange = 1.5f;
    public LayerMask stealthAttackLayers;
    public AudioClip stealthSwingSound;
    public AudioClip stealthHitSound;
    private bool isStealth = false;

    public bool IsStealth => isStealth;

    [Header("Kick Settings")]
    public float kickRange = 0.8f;
    public int kickDamage = 0;
    public float kickForce = 5f;
    public LayerMask kickLayers;
    public float kickCooldown = 1f;
    private float nextKickTime;

    Vector2 moveDirection;
    Vector2 mousePosition;
    private float nextFireTime;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int PlayerKickHash = Animator.StringToHash("playerkick");

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (attackAction != null) attackAction.action.Enable();
        if (kickAction != null) kickAction.action.Enable();
        if (reloadAction != null) reloadAction.action.Enable();
        if (interactAction != null) interactAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (attackAction != null) attackAction.action.Disable();
        if (kickAction != null) kickAction.action.Disable();
        if (reloadAction != null) reloadAction.action.Disable();
        if (interactAction != null) interactAction.action.Disable();
    }

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (loadWeaponOnStart)
        {
            LoadSavedWeapon();
        }
    }

    private void LoadSavedWeapon()
    {
        string savedWeaponName = Project.Scripts.GameSaveManager.GetSavedWeapon();
        if (string.IsNullOrEmpty(savedWeaponName)) return;

        foreach (GameObject prefab in allWeaponPrefabs)
        {
            if (prefab.name == savedWeaponName)
            {
                GameObject weaponInstance = Instantiate(prefab);
                Weapon newWeapon = weaponInstance.GetComponent<Weapon>();
                if (newWeapon != null)
                {
                    EquipWeapon(newWeapon);

                    UpdateAnimatorBools(savedWeaponName);
                }
                break;
            }
        }
    }

    private void UpdateAnimatorBools(string weaponName)
    {
        if (animator == null) return;

        bool isShotgun = weaponName.Contains("Shotgun");
        bool isPistol = weaponName.Contains("Pistol");
        bool isBat = weaponName.Contains("Bat");
        bool isUzi = weaponName.Contains("Uzi");

        animator.SetBool("HasShotgun", isShotgun);
        animator.SetBool("HasPistol", isPistol);
        animator.SetBool("HasBat", isBat);
        animator.SetBool("HasUzi", isUzi);

        if (isShotgun) animator.SetTrigger("playershotgun");
        else if (isPistol) animator.SetTrigger("playerpistol");
        else if (isBat) animator.SetTrigger("playerbat");
        else if (isUzi) animator.SetTrigger("playeruzi");
    }

    void Update()
    {
        if (moveAction != null)
        {
            moveDirection = moveAction.action.ReadValue<Vector2>().normalized;
        }

        if (isStealth)
        {
            if (attackAction != null && attackAction.action.WasPressedThisFrame())
            {
                StealthAttack();
            }
        }
        else if (attackAction != null && weapon != null && !weapon.IsReloading)
        {
            bool shouldFire = false;
            if (weapon.isAutomatic)
            {
                if (attackAction.action.IsPressed() && Time.time >= nextFireTime)
                {
                    shouldFire = true;
                    nextFireTime = Time.time + weapon.fireRate;
                }
            }
            else
            {
                if (attackAction.action.WasPressedThisFrame())
                {
                    shouldFire = true;
                }
            }

            if (shouldFire)
            {
                weapon.Fire();

                if (weapon.name.Contains("Shotgun")) animator.SetTrigger("playershotgun");
                else if (weapon.name.Contains("Pistol")) animator.SetTrigger("playerpistol");
                else if (weapon.name.Contains("Bat")) animator.SetTrigger("playerbat");
                else if (weapon.name.Contains("Uzi")) animator.SetTrigger("playeruzi");
            }
        }

        if (!isStealth && reloadAction != null && reloadAction.action.WasPressedThisFrame() && weapon != null)
            weapon.Reload();

        if (!isStealth && kickAction != null && kickAction.action.WasPressedThisFrame() && Time.time >= nextKickTime)
        {
            Kick();
            nextKickTime = Time.time + kickCooldown;
        }

        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            ToggleStealth();
        }

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (animator != null)
        {
            animator.SetBool(IsWalkingHash, moveDirection.magnitude > 0);
        }
    }

    public void Kick()
    {
        Debug.Log("Player Kicked!");
if (animator != null)
        {
            animator.SetTrigger(PlayerKickHash);
        }

        Vector2 kickOrigin = (Vector2)transform.position + (Vector2)transform.right * 0.5f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(kickOrigin, kickRange, kickLayers);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Door>(out Door door))
            {
                door.Kick(transform.position);
            }

            if (hitCollider.CompareTag("Enemy"))
            {

                if (hitCollider.TryGetComponent<EnemyAStarFollow>(out var aStarEnemy))
                {
                    aStarEnemy.Stun(1f);
                }
                else if (hitCollider.TryGetComponent<EnemyShotgunAI>(out var shotgunEnemy))
                {
                    shotgunEnemy.Stun(1f);
                }

                Rigidbody2D enemyRb = hitCollider.GetComponent<Rigidbody2D>();
                if (enemyRb == null) enemyRb = hitCollider.GetComponentInParent<Rigidbody2D>();

                if (enemyRb != null)
                {
                    enemyRb.AddForce(transform.right * kickForce, ForceMode2D.Impulse);
                }
            }

            if (kickDamage > 0 && hitCollider.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(kickDamage);
            }
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        // Hide weapon sprite if the player animation already handles it
        SpriteRenderer weaponSR = weapon.GetComponent<SpriteRenderer>();
        if (weaponSR != null) weaponSR.enabled = false;

        UpdateAnimatorBools(newWeapon.name);
    }

    public void StealthAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("playerstab");
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && stealthSwingSound != null)
        {
            audioSource.PlayOneShot(stealthSwingSound);
        }

        Vector2 attackOrigin = (Vector2)transform.position + (Vector2)transform.right * 0.5f;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin, stealthAttackRange, stealthAttackLayers);

        bool hitSomething = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage((int)stealthAttackDamage);
                hitSomething = true;
            }
        }

        if (hitSomething && audioSource != null && stealthHitSound != null)
        {
            audioSource.PlayOneShot(stealthHitSound);
        }
    }

    private void ToggleStealth()
    {
        isStealth = !isStealth;
        
        if (animator != null)
        {
            animator.SetBool("IsStealth", isStealth);
            
            if (isStealth)
            {
                animator.SetBool("HasShotgun", false);
                animator.SetBool("HasPistol", false);
                animator.SetBool("HasBat", false);
                animator.SetBool("HasUzi", false);
            }
        }

        if (isStealth)
        {
            if (weapon != null) weapon.gameObject.SetActive(false);
        }
        else
        {
            if (weapon != null)
            {
                weapon.gameObject.SetActive(true);
                UpdateAnimatorBools(weapon.name);
            }
        }
    }

    void FixedUpdate()
    {

        float currentSpeed = isStealth ? moveSpeed * 0.6f : moveSpeed;
        rb.linearVelocity = moveDirection * currentSpeed;

        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }
}