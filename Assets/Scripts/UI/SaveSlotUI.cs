using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Project.Scripts.UI
{
    public class SaveSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button deleteButton;

        public void Setup(SaveSlot data, Action onPlay, Action onDelete)
        {
            titleText.text = $"Slot {data.slotId + 1}: {data.levelName}";
            infoText.text = $"{data.date}\nWeapon: {data.weaponPrefabName}";
            
            playButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
            
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => onPlay?.Invoke());
            
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => onDelete?.Invoke());
        }

        public void SetupEmpty(int slotId)
        {
            titleText.text = $"Slot {slotId + 1}: Empty";
            infoText.text = "No data saved.";
            
            playButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }
    }
}
