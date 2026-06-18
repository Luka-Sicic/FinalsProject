using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Project.Scripts.UI
{
    public class DeathMenuManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject deathMenuUI;
        public TextMeshProUGUI statusText;
        public Button respawnButton;
        public Button quitButton;

        private void OnEnable()
        {
            Health.OnPlayerDeath += ShowDeathMenu;
        }

        private void OnDisable()
        {
            Health.OnPlayerDeath -= ShowDeathMenu;
        }

        private void Start()
        {
            if (deathMenuUI != null)
                deathMenuUI.SetActive(false);

            if (respawnButton != null)
                respawnButton.onClick.AddListener(RestartLevel);

            if (quitButton != null)
                quitButton.onClick.AddListener(LoadMainMenu);
        }

        private void ShowDeathMenu()
        {
            Debug.Log("[DeathMenuManager] ShowDeathMenu called");
            if (deathMenuUI != null)
            {
                deathMenuUI.SetActive(true);
                Debug.Log("[DeathMenuManager] deathMenuUI set to active");
            }
            else
            {
                Debug.LogError("[DeathMenuManager] deathMenuUI is NULL");
            }

            if (statusText != null)
                statusText.text = "You died";

            Time.timeScale = 0f;
            
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("[DeathMenuManager] Time.timeScale set to 0 and cursor unlocked");
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0); 
        }
    }
}
