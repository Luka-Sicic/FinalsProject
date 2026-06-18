using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [Header("Settings")]
    public GameObject weaponPrefab;
    public float pickupDistance = 2f;
    [SerializeField] private InputActionReference interactAction;
    
    [Header("Animation")]
public string animTrigger;
    public string animBool;

    [Header("UI")]
    public GameObject canvasPrompt;

    private PlayerController player;
    private static WeaponPickup currentInteractable;
    private static float nextPickupTime = 0f;
    private const float GlobalPickupCooldown = 0.2f;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    void Start()
{
        player = Object.FindAnyObjectByType<PlayerController>();
        if (canvasPrompt != null) canvasPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool inRange = distance < pickupDistance;

        if (inRange)
        {
            if (currentInteractable == null || currentInteractable == this)
            {
                currentInteractable = this;
            }
            else
            {
                float currentDist = Vector2.Distance(currentInteractable.transform.position, player.transform.position);
                if (distance < currentDist)
                {
                    
                    if (currentInteractable.canvasPrompt != null) 
                        currentInteractable.canvasPrompt.SetActive(false);
                    currentInteractable = this;
                }
            }
        }
        else if (currentInteractable == this)
        {
            currentInteractable = null;
            if (canvasPrompt != null) canvasPrompt.SetActive(false);
        }

        if (currentInteractable == this)
        {
            if (canvasPrompt != null && !canvasPrompt.activeSelf) 
                canvasPrompt.SetActive(true);
            
            bool interactPressed = interactAction != null && interactAction.action.WasPressedThisFrame();
            if (interactPressed && Time.time >= nextPickupTime)
            {
                nextPickupTime = Time.time + GlobalPickupCooldown;
                PickUp();
            }
}
    }

    void PickUp()
    {
        if (currentInteractable == this)
        {
            currentInteractable = null;
        }

        if (canvasPrompt != null) canvasPrompt.SetActive(false);

        if (weaponPrefab == null)
        {
            Debug.LogWarning("WeaponPickup: No weapon prefab assigned!");
            return;
        }

        
        Weapon prefabWeapon = weaponPrefab.GetComponent<Weapon>();
        if (player.weapon != null && player.weapon.GetType() == prefabWeapon.GetType())
        {
            player.weapon.spareReloads++;
            Destroy(gameObject);
            return;
        }

        GameObject weaponInstance = Instantiate(weaponPrefab);
Weapon weaponScript = weaponInstance.GetComponent<Weapon>();
        
        if (weaponScript != null)
        {
            player.EquipWeapon(weaponScript);
        }

        
        if (player.animator != null)
        {
            
            player.animator.SetBool("HasShotgun", false);
            player.animator.SetBool("HasPistol", false);
            player.animator.SetBool("HasBat", false);

            if (!string.IsNullOrEmpty(animTrigger))
                player.animator.SetTrigger(animTrigger);
            
            if (!string.IsNullOrEmpty(animBool))
            {
                player.animator.SetBool(animBool, true);
            }
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (currentInteractable == this)
        {
            currentInteractable = null;
        }
        if (canvasPrompt != null) canvasPrompt.SetActive(false);
    }
}
