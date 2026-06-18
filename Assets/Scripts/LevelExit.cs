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

        
        if (player != null && player.weapon != null)
        {
            string weaponName = player.weapon.gameObject.name.Replace("(Clone)", "").Trim();
            Project.Scripts.GameSaveManager.SaveWeapon(weaponName);
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
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
