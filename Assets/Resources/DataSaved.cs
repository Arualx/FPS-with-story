using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataSaved : MonoBehaviour
{
    public static DataSaved instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // SOUND
        audioMixer = FindAnyObjectByType<AudioMixer>();
        soundSlider = GameObject.FindWithTag("SoundSlider").GetComponent<Slider>();
        musicSlider = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();

        // RESOLUTION
        resolutionDropdown = GameObject.FindWithTag("ResolutionDropdown").GetComponent<TMP_Dropdown>();

    }

    [Header("Audio Settings")]
    private float savedSound;
    private Slider soundSlider;

    private float savedMusic;
    private Slider musicSlider;

    private AudioMixer audioMixer;

    [Header("Other Settings")]

    private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutionsAvailable;
    private int savedResolutionIndex;

    private TMP_Dropdown StartDropdown;


    private void Start()
    {

        // Load saved settings or set defaults
        savedSound = PlayerPrefs.GetFloat("Sound", 1f);
        savedMusic = PlayerPrefs.GetFloat("Music", 1f);

        // Apply saved settings to slider
        soundSlider.value = savedSound;
        musicSlider.value = savedMusic;

        // Remove existing listeners to prevent multiple calls
        // Add listeners to update values when sliders are changed
        soundSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        soundSlider.onValueChanged.AddListener((volume) => ApplyVolume("Sound", volume));
        musicSlider.onValueChanged.AddListener((volume) => ApplyVolume("Music", volume));


        // Get available resolutions and populate dropdown
        resolutionsAvailable = Screen.resolutions;
        
        resolutionsAvailable = Screen.resolutions.DistinctBy(r => (r.width, r.height)).ToArray();
        var options = resolutionsAvailable.Select(r => $"{r.width} x {r.height}").Distinct().ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        // Load saved resolution index or set to last available
        savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutionsAvailable.Length - 1);

        // Apply saved resolution
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        ApplyResolution(savedResolutionIndex);



    }

    public void ApplyVolume(string parameter, float volume)
    {
        // Convert to dB
        float dB = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20f; 
        audioMixer.SetFloat(parameter, dB);
    }

    public void ApplyResolution(int index)
    {
        Resolution res = resolutionsAvailable[index];
        Screen.SetResolution(res.width, res.height, true);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }


    public void SaveSettings()
    {
        // Update saved values with current settings before saving
        savedSound = soundSlider.value;
        savedMusic = musicSlider.value;
        savedResolutionIndex = resolutionDropdown.value;

        // Update PlayerPrefs with current settings
        PlayerPrefs.SetFloat("Sound", savedSound);
        PlayerPrefs.SetFloat("Music", savedMusic);
        PlayerPrefs.SetInt("ResolutionIndex", savedResolutionIndex);
        PlayerPrefs.Save();
    }

    public void CancelSettings()
    {
        // Revert to last saved values and cancel changes
        soundSlider.value = savedSound;
        musicSlider.value = savedMusic;

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        ApplyResolution(savedResolutionIndex);
    }


    //when pressed esc to exit the game, save the settings
}
