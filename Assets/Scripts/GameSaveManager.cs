using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts
{
    [Serializable]
    public class SaveSlot
    {
        public int slotId;
        public int levelIndex;
        public string levelName;
        public string weaponPrefabName;
        public string date;
        public long timestamp;
    }

    [Serializable]
    public class SaveSystemData
    {
        public List<SaveSlot> slots = new List<SaveSlot>();
    }

    public static class GameSaveManager
    {
        private const string SaveDataKey = "GameSaveSystemData";
        private const string CurrentSlotKey = "CurrentSlotId";

        private const string LevelKey = "LastLevelIndex";
        private const string WeaponKey = "EquippedWeapon";
        private const string HasSaveKey = "HasGameSave";

        public static int currentSlotId { get; private set; } = -1;

        static GameSaveManager()
        {
            currentSlotId = PlayerPrefs.GetInt(CurrentSlotKey, -1);
        }

        private static SaveSystemData LoadData()
        {
            if (PlayerPrefs.HasKey(SaveDataKey))
            {
                string json = PlayerPrefs.GetString(SaveDataKey);
                return JsonUtility.FromJson<SaveSystemData>(json);
            }

            if (PlayerPrefs.GetInt(HasSaveKey, 0) == 1)
            {
                SaveSystemData data = new SaveSystemData();
                SaveSlot slot = new SaveSlot
                {
                    slotId = 0,
                    levelIndex = PlayerPrefs.GetInt(LevelKey),
                    levelName = "Legacy Save",
                    weaponPrefabName = PlayerPrefs.GetString(WeaponKey, ""),
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                data.slots.Add(slot);
                SaveData(data);

                PlayerPrefs.DeleteKey(HasSaveKey);
                PlayerPrefs.DeleteKey(LevelKey);
                PlayerPrefs.DeleteKey(WeaponKey);
                PlayerPrefs.Save();

                return data;
            }

            return new SaveSystemData();
        }

        private static void SaveData(SaveSystemData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveDataKey, json);
            PlayerPrefs.Save();
        }

        public static void SaveGame(string weaponPrefabName, int levelIndex = -1)
        {
            if (currentSlotId == -1)
            {
                currentSlotId = NewGameAllocatedSlot();
            }

            SaveSystemData data = LoadData();
            SaveSlot slot = data.slots.FirstOrDefault(s => s.slotId == currentSlotId);

            if (slot == null)
            {
                slot = new SaveSlot { slotId = currentSlotId };
                data.slots.Add(slot);
            }

            int targetLevelIndex = levelIndex == -1 ? SceneManager.GetActiveScene().buildIndex : levelIndex;
            slot.levelIndex = targetLevelIndex;

            string scenePath = SceneUtility.GetScenePathByBuildIndex(targetLevelIndex);
            if (!string.IsNullOrEmpty(scenePath))
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                slot.levelName = sceneName;
            }
            else
            {
                slot.levelName = "Level " + targetLevelIndex;
            }

            slot.weaponPrefabName = weaponPrefabName;
            slot.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            slot.timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            SaveData(data);
            PlayerPrefs.SetInt(CurrentSlotKey, currentSlotId);
            PlayerPrefs.Save();

            Debug.Log($"Game Saved in Slot {currentSlotId}: Level {slot.levelName}, Weapon {weaponPrefabName}");
        }

        public static void SaveWeapon(string weaponPrefabName, int levelIndex = -1)
        {
            SaveGame(weaponPrefabName, levelIndex);
        }

        public static List<SaveSlot> GetSlots()
        {
            return LoadData().slots;
        }

        public static int NewGameAllocatedSlot()
        {
            SaveSystemData data = LoadData();

            for (int i = 0; i < 4; i++)
            {
                if (data.slots.All(s => s.slotId != i))
                {
                    return i;
                }
            }

            SaveSlot oldest = data.slots.OrderBy(s => s.timestamp).First();
            return oldest.slotId;
        }

        public static void LoadGame(int slotId)
        {
            SaveSystemData data = LoadData();
            SaveSlot slot = data.slots.FirstOrDefault(s => s.slotId == slotId);

            if (slot != null)
            {
                currentSlotId = slotId;
                PlayerPrefs.SetInt(CurrentSlotKey, currentSlotId);
                PlayerPrefs.Save();
                SceneManager.LoadScene(slot.levelIndex);
            }
            else
            {
                Debug.LogWarning($"No save game found in slot {slotId}!");
            }
        }

        public static void DeleteSave(int slotId)
        {
            SaveSystemData data = LoadData();
            data.slots.RemoveAll(s => s.slotId == slotId);
            SaveData(data);

            if (currentSlotId == slotId)
            {
                currentSlotId = -1;
                PlayerPrefs.DeleteKey(CurrentSlotKey);
                PlayerPrefs.Save();
            }
        }

        public static void PrepareNewGame(int slotId)
        {
            DeleteSave(slotId);
            currentSlotId = slotId;
            PlayerPrefs.SetInt(CurrentSlotKey, currentSlotId);
            PlayerPrefs.Save();

            SaveGame("", 1);
        }

        public static bool HasSave()
        {
            return LoadData().slots.Count > 0;
        }

        public static string GetSavedWeapon()
        {
            if (currentSlotId == -1) return "";

            SaveSystemData data = LoadData();
            SaveSlot slot = data.slots.FirstOrDefault(s => s.slotId == currentSlotId);
            return slot?.weaponPrefabName ?? "";
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteKey(SaveDataKey);
            PlayerPrefs.DeleteKey(CurrentSlotKey);
            PlayerPrefs.Save();
            currentSlotId = -1;
        }

        public static void LoadGame()
        {
            if (currentSlotId != -1)
            {
                LoadGame(currentSlotId);
            }
            else
            {
                SaveSystemData data = LoadData();
                if (data.slots.Count > 0)
                {

                    SaveSlot latest = data.slots.OrderByDescending(s => s.timestamp).First();
                    LoadGame(latest.slotId);
                }
                else
                {
                    Debug.LogWarning("No save game found!");
                }
            }
        }
    }
}