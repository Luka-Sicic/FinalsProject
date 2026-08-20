using UnityEngine;
using UnityEngine.InputSystem;
using Project.Scripts;

public class CameraTerminal : MonoBehaviour
{
    [Header("Settings")]
    public float pickupDistance = 2f;
    [SerializeField] private InputActionReference interactAction;

    [Header("UI")]
    public GameObject canvasPrompt;

    private PlayerController player;
    private static CameraTerminal currentInteractable;
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
        // Turn off all security cameras
        SecurityCamera[] cameras = Object.FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None);
        foreach (var cam in cameras)
        {
            cam.TurnOff();
        }

        Debug.Log($"Security Terminal: {cameras.Length} cameras disabled.");

        if (canvasPrompt != null) canvasPrompt.SetActive(false);
        
        // Disable this script to prevent multiple interactions
        this.enabled = false;
        if (currentInteractable == this)
        {
            currentInteractable = null;
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
