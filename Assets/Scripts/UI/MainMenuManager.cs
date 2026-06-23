using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject loadGamePanel;
        [SerializeField] private GameObject confirmationPanel;

        private int pendingSlotId = -1;

        public void NewGame()
        {
            int slotId = GameSaveManager.NewGameAllocatedSlot();
            
            // Check if this slot already has data (overwrite)
            var slots = GameSaveManager.GetSlots();
            bool isOverwrite = slots.Exists(s => s.slotId == slotId);

            if (isOverwrite && confirmationPanel != null)
            {
                pendingSlotId = slotId;
                mainPanel.SetActive(false);
                confirmationPanel.SetActive(true);
            }
            else
            {
                StartNewGame(slotId);
            }
        }

        public void ConfirmNewGame()
        {
            if (pendingSlotId != -1)
            {
                StartNewGame(pendingSlotId);
            }
        }

        public void CloseConfirmation()
        {
            pendingSlotId = -1;
            if (confirmationPanel != null) confirmationPanel.SetActive(false);
            if (mainPanel != null) mainPanel.SetActive(true);
        }

        private void StartNewGame(int slotId)
        {
            GameSaveManager.PrepareNewGame(slotId);
            SceneManager.LoadScene("Level1");
        }

        public void LoadGame()
{
            if (loadGamePanel != null)
            {
                mainPanel.SetActive(false);
                loadGamePanel.SetActive(true);
            }
            else
            {
                // Fallback to old behavior if panel is not assigned
                if (GameSaveManager.HasSave())
                {
                    GameSaveManager.LoadGame();
                }
                else
                {
                    Debug.Log("No save game found");
                }
            }
        }

        public void OpenMainPanel()
        {
            if (mainPanel != null) mainPanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (loadGamePanel != null) loadGamePanel.SetActive(false);
        }

        public void OpenSettings()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            OpenMainPanel();
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
