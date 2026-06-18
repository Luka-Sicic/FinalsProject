using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mainMixer;
    
    private const string MasterVolParam = "MasterVolume";
    private const string MusicVolParam = "MusicVolume";
    private const string SFXVolParam = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        LoadSettings();
    }

    private void Start()
    {
        LoadSettings();
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(MasterVolParam, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(MusicVolParam, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(SFXVolParam, volume);
    }

    private void SetVolume(string parameter, float volume)
    {
        if (_mainMixer != null)
        {
            
            float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            _mainMixer.SetFloat(parameter, db);
        }
        PlayerPrefs.SetFloat(parameter, volume);
    }

    public float GetVolume(string parameter)
    {
        return PlayerPrefs.GetFloat(parameter, 0.75f);
    }

    private void LoadSettings()
    {
        SetMasterVolume(GetVolume(MasterVolParam));
        SetMusicVolume(GetVolume(MusicVolParam));
        SetSFXVolume(GetVolume(SFXVolParam));
    }
}
