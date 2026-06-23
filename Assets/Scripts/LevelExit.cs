using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    public float pickupDistance = 2f;
    [SerializeField] private InputActionReference interactAction;

    [Header("UI")]
    public GameObject canvasPrompt;

    private PlayerController player;
    private static LevelExit currentInteractable;
    private static float nextInteractTime = 0f;
    private const float GlobalInteractCooldown = 0.2f;

    private void Start()
    {
        player = Object.FindAnyObjectByType<PlayerController>();
        if (canvasPrompt != null) canvasPrompt.SetActive(false);
    }

    private void Update()
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
            if (interactPressed && Time.time >= nextInteractTime)
            {
                nextInteractTime = Time.time + GlobalInteractCooldown;
                Interact();
            }
        }
    }

    private void Interact()
    {
        if (currentInteractable == this)
        {
            currentInteractable = null;
        }

        if (canvasPrompt != null) canvasPrompt.SetActive(false);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if next scene exists
        bool hasNextLevel = nextSceneIndex < SceneManager.sceneCountInBuildSettings;

        if (player != null && player.weapon != null)
        {
            string weaponName = player.weapon.gameObject.name.Replace("(Clone)", "").Trim();
            // Save progress to the NEXT level if it exists, otherwise don't update level index in save
            if (hasNextLevel)
            {
                Project.Scripts.GameSaveManager.SaveWeapon(weaponName, nextSceneIndex);
            }
            else
            {
                Project.Scripts.GameSaveManager.SaveWeapon(weaponName); // Save current as fallback
            }
        }

        if (hasNextLevel)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
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
