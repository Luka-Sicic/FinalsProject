using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public static class GameSaveManager
    {
        private const string LevelKey = "LastLevelIndex";
        private const string WeaponKey = "EquippedWeapon";
        private const string HasSaveKey = "HasGameSave";

        public static void SaveGame(string weaponPrefabName)
        {
            PlayerPrefs.SetInt(LevelKey, SceneManager.GetActiveScene().buildIndex);
            SaveWeapon(weaponPrefabName);
            PlayerPrefs.SetInt(HasSaveKey, 1);
            PlayerPrefs.Save();
            Debug.Log($"Game Saved: Level {SceneManager.GetActiveScene().name}, Weapon {weaponPrefabName}");
        }

        public static void SaveWeapon(string weaponPrefabName)
        {
            PlayerPrefs.SetString(WeaponKey, weaponPrefabName);
            PlayerPrefs.Save();
        }

        public static bool HasSave()
        {
            return PlayerPrefs.GetInt(HasSaveKey, 0) == 1;
        }

        public static void LoadGame()
        {
            if (HasSave())
            {
                int levelIndex = PlayerPrefs.GetInt(LevelKey);
                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                Debug.LogWarning("No save game found!");
            }
        }

        public static string GetSavedWeapon()
        {
            return PlayerPrefs.GetString(WeaponKey, "");
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteKey(HasSaveKey);
            PlayerPrefs.DeleteKey(LevelKey);
            PlayerPrefs.DeleteKey(WeaponKey);
            PlayerPrefs.Save();
        }
    }
}
