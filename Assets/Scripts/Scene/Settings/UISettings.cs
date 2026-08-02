using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderVFX;

    [SerializeField] private AudioSettingsSO settingsSO;
    [SerializeField] private AudioMixer mixer;
    private void Awake()
    {
        settingsSO.LoadSettings();

        sliderMaster.onValueChanged.RemoveAllListeners();
        sliderMusic.onValueChanged.RemoveAllListeners();
        sliderVFX.onValueChanged.RemoveAllListeners();

        sliderMaster.value = settingsSO.masterVolume;
        sliderMusic.value = settingsSO.musicVolume;
        sliderVFX.value = settingsSO.vfxVolume;

        sliderMaster.onValueChanged.AddListener(ChangeMasterVolume);
        sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);
        sliderVFX.onValueChanged.AddListener(ChangeVFXVolume);
    }

    private void Start()
    {
        mixer.SetFloat("MasterVolume", ConvertToDecibels(sliderMaster.value));
        mixer.SetFloat("MusicVolume", ConvertToDecibels(sliderMusic.value));
        mixer.SetFloat("VFXVolume", ConvertToDecibels(sliderVFX.value));
    }

    private void ChangeMasterVolume(float currentValue)
    {
        settingsSO.SaveMaster(currentValue);
        mixer.SetFloat("MasterVolume", ConvertToDecibels(currentValue));
    }

    private void ChangeMusicVolume(float currentValue)
    {
        settingsSO.SaveMusic(currentValue);
        mixer.SetFloat("MusicVolume", ConvertToDecibels(currentValue));
    }

    private void ChangeVFXVolume(float currentValue)
    {
        settingsSO.SaveVFX(currentValue);
        mixer.SetFloat("VFXVolume", ConvertToDecibels(currentValue));
    }

    private float ConvertToDecibels(float linearValue)
    {
        if (linearValue <= 0.0001f)
        {
            return -80f;
        }
        else
        {
            return Mathf.Log10(linearValue) * 20;
        }
    }
}