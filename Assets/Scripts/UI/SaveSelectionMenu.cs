using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Project.Scripts;

namespace Project.Scripts.UI
{
    public class SaveSelectionMenu : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotContainer;
        [SerializeField] private MainMenuManager mainMenuManager;

        private void OnEnable()
        {
            RefreshSlots();
        }

        public void RefreshSlots()
        {

            foreach (Transform child in slotContainer)
            {
                Destroy(child.gameObject);
            }

            List<SaveSlot> slots = GameSaveManager.GetSlots();

            for (int i = 0; i < 4; i++)
            {
                int slotId = i;
                GameObject slotObj = Instantiate(slotPrefab, slotContainer);
                SaveSlotUI ui = slotObj.GetComponent<SaveSlotUI>();

                SaveSlot slotData = slots.Find(s => s.slotId == slotId);

                if (slotData != null)
                {
                    ui.Setup(slotData, () => LoadSlot(slotId), () => DeleteSlot(slotId));
                }
                else
                {
                    ui.SetupEmpty(slotId);
                }
            }
        }

        private void LoadSlot(int slotId)
        {
            GameSaveManager.LoadGame(slotId);
        }

        private void DeleteSlot(int slotId)
        {
            GameSaveManager.DeleteSave(slotId);
            RefreshSlots();
        }

        public void Back()
        {
            gameObject.SetActive(false);
            if (mainMenuManager != null)
            {
                mainMenuManager.OpenMainPanel();
            }
        }
    }
}