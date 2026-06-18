using UnityEngine;
using UnityEngine.InputSystem;

public class InputSettingsManager : MonoBehaviour
{
    public static InputSettingsManager Instance { get; private set; }

    [SerializeField] private InputActionAsset actionsAsset;
    private const string RebindsKey = "rebinds";

    public void Initialize(InputActionAsset asset)
    {
        actionsAsset = asset;
        LoadOverrides();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadOverrides();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveOverrides()
    {
        if (actionsAsset == null) return;
        string rebinds = actionsAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, rebinds);
        PlayerPrefs.Save();
    }

    public void LoadOverrides()
    {
        if (actionsAsset == null) return;
        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionsAsset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ResetOverrides()
    {
        if (actionsAsset == null) return;
        actionsAsset.RemoveAllBindingOverrides();
        SaveOverrides();
    }
}
