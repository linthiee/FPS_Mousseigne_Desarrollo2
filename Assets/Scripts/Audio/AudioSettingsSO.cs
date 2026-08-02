using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Grupo02_Platformer2D/AudioSettings")]

public class AudioSettingsSO : ScriptableObject
{
    private const string MasterKey = "VolumeMaster";
    private const string MusicKey = "VolumeMusic";
    private const string VFXKey = "VolumeVFX";

    public float masterVolume;
    public float musicVolume;
    public float vfxVolume;

    public void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MasterKey, 0.75f);
        musicVolume = PlayerPrefs.GetFloat(MusicKey, 0.75f);
        vfxVolume = PlayerPrefs.GetFloat(VFXKey, 0.75f);
    }

    public void SaveMaster(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat(MasterKey, value);
    }

    public void SaveMusic(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SaveVFX(float value)
    {
        vfxVolume = value;
        PlayerPrefs.SetFloat(VFXKey, value);
    }
}