using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Project.Scripts;

namespace Project.Scripts.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Input")]
        [SerializeField] private InputActionReference pauseAction;

        [Header("Cursors")]
        [SerializeField] private Texture2D mouseRegular;
        [SerializeField] private Texture2D mouseAim;
        [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

        private bool isPaused = false;
        private Health playerHealth;

        private void Start()
        {
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }

            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);

            
            if (mouseAim != null)
            {
                Cursor.SetCursor(mouseAim, cursorHotspot, CursorMode.Auto);
            }
        }

        private void OnEnable()
        {
            if (pauseAction != null) pauseAction.action.Enable();
        }

        private void Update()
        {
            if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    
                    if (GameObject.FindGameObjectWithTag("Player") != null)
                    {
                        Pause();
                    }
                }
            }
        }

        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (mouseRegular != null)
            {
                Cursor.SetCursor(mouseRegular, cursorHotspot, CursorMode.Auto);
            }
        }

        public void Resume()
        {
            isPaused = false;
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 

            if (mouseAim != null)
            {
                Cursor.SetCursor(mouseAim, cursorHotspot, CursorMode.Auto);
            }
        }

        public void OpenSettings()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            if (pausePanel != null) pausePanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);
        }

        public void SaveAndQuit()
        {
            
            Time.timeScale = 1f;

            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null && pc.weapon != null)
                {
                    
                    string weaponName = pc.weapon.gameObject.name.Replace("(Clone)", "").Trim();
                    GameSaveManager.SaveGame(weaponName);
                }
                else
                {
                    GameSaveManager.SaveGame("");
                }
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}
