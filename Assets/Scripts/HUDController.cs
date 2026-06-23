using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Scripts;

public class HUDController : MonoBehaviour
{
    [Header("Health UI")]
    public TextMeshProUGUI healthText;
    
    [Header("Weapon UI")]
    public Image weaponIcon;
    public TextMeshProUGUI ammoText;

    [Header("Objective UI")]
    public TextMeshProUGUI objectiveText;

    private Health playerHealth;
    private PlayerController playerController;

    private void Start()
    {
        if (weaponIcon != null)
        {
            weaponIcon.preserveAspect = true;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerController = player.GetComponent<PlayerController>();
        }

        // Set default objective
        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Find a way to the next level";
        }
    }

    private void Update()
    {
        UpdateHealthUI();
        UpdateWeaponUI();
    }

    private void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"Health: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }
    }

    private void UpdateWeaponUI()
    {
        if (playerController != null)
        {
            Weapon currentWeapon = playerController.weapon;
            if (currentWeapon != null)
            {
                if (weaponIcon != null)
                {
                    weaponIcon.sprite = currentWeapon.weaponIcon;
                    weaponIcon.enabled = currentWeapon.weaponIcon != null;
                }

                if (ammoText != null)
                {
                    ammoText.text = $"Ammo: {currentWeapon.ammo} / {currentWeapon.maxAmmo} ({currentWeapon.spareReloads})";
                }
            }
            else
            {
                if (weaponIcon != null) weaponIcon.enabled = false;
                if (ammoText != null)
                {
                    ammoText.text = "No Weapon";
                }
            }
        }
    }
}
