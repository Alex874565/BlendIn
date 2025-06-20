using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer audioMixer;

    public void Start()
    {
        InitializeVolume(); // Initialize volume settings when the script starts
    }

    public void InitializeVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume(); // Load saved volume settings if they exist
        }
        else
        {
            // Set default values if no saved settings are found
            musicSlider.value = 0.5f;
            sfxSlider.value = 0.5f;
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Save the volume setting
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", volume); // Save the volume setting
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume)*20);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f); // Default to 0.5 if not set
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f); // Default to 0.5 if not set
        SetMusicVolume(); // Apply the loaded volume
        SetSFXVolume(); // Apply the loaded volume
    }
}
