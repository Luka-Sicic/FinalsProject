using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSliderUI : MonoBehaviour
{
    public enum VolumeType { Master, Music, SFX }

    [SerializeField] private VolumeType _type;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _valueText;

    public void Initialize(VolumeType type, Slider slider, TMP_Text valueText)
    {
        _type = type;
        _slider = slider;
        _valueText = valueText;
        Setup();
    }

    private void Start()
    {
        if (_slider != null) Setup();
    }

    private void Setup()
    {
        float currentVol = 0.75f;
        if (AudioManager.Instance != null)
        {
            switch (_type)
            {
                case VolumeType.Master: currentVol = AudioManager.Instance.GetVolume("MasterVolume"); break;
                case VolumeType.Music: currentVol = AudioManager.Instance.GetVolume("MusicVolume"); break;
                case VolumeType.SFX: currentVol = AudioManager.Instance.GetVolume("SFXVolume"); break;
            }
        }

        _slider.value = currentVol;
        UpdateText(currentVol);

        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            switch (_type)
            {
                case VolumeType.Master: AudioManager.Instance.SetMasterVolume(value); break;
                case VolumeType.Music: AudioManager.Instance.SetMusicVolume(value); break;
                case VolumeType.SFX: AudioManager.Instance.SetSFXVolume(value); break;
            }
        }
        UpdateText(value);
    }

    private void UpdateText(float value)
    {
        if (_valueText != null)
        {
            _valueText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }
}