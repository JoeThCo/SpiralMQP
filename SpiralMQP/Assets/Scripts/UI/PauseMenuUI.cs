using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Tooltip("Populate with the music volume level")]
    [SerializeField] private TextMeshProUGUI musicLevelText;

    [Tooltip("Populate with the sounds volume level")]
    [SerializeField] private TextMeshProUGUI soundsLevelText;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private int previousMusicLevel;
    private int previousSoundLevel;

    private void Start()
    {
        // Initially hide the pause menu
        StartCoroutine(InitializeUI(false));
    }

    /// <summary>
    /// Initialize the UI text
    /// </summary>
    private IEnumerator InitializeUI(bool gameObjectStateAfter)
    {
        // Wait a frame to ensure the previous music and sound levels have been set
        yield return null;

        // Initialise UI text
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        previousSoundLevel = SoundEffectManager.Instance.soundsVolume;
        soundSlider.value = previousSoundLevel;

        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
        previousMusicLevel = MusicManager.Instance.musicVolume;
        musicSlider.value = previousMusicLevel;

        gameObject.SetActive(gameObjectStateAfter);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        // Initialise UI text
        StartCoroutine(InitializeUI(true));
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    // Quit and load main menu - linked to from pause menu UI button
    public void LoadMainMenu()
    {
        LoadingManager.Instance.LoadSceneWithTransistion("MainMenu");
    }

    /// <summary>
    /// Increase music volume - linked to from music volume increase button in UI
    /// </summary>
    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    /// <summary>
    /// Decrease music volume - linked to from music volume decrease button in UI
    /// </summary>
    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    /// <summary>
    /// Handle the music slider on change event
    /// </summary>
    public void OnMusicSliderChange()
    {
        // Get the current music level volume
        int currentMusicLevel = (int)musicSlider.value;

        // Adjust the volume accordingly
        if (currentMusicLevel > previousMusicLevel)
        {
            for (int i = 0; i < currentMusicLevel - previousMusicLevel; i++)
            {
                IncreaseMusicVolume();
            }
        }
        else
        {
            for (int i = 0; i < previousMusicLevel - currentMusicLevel; i++)
            {
                DecreaseMusicVolume();
            }
        }

        // Update the previous music level value holder
        previousMusicLevel = currentMusicLevel;
    }

    /// <summary>
    /// Increase sounds volume - linked to from sounds volume increase button in UI
    /// </summary>
    public void IncreaseSoundsVolume()
    {
        SoundEffectManager.Instance.IncreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    /// <summary>
    /// Decrease sounds volume - linked to from sounds volume decrease button in UI
    /// </summary>
    public void DecreaseSoundsVolume()
    {
        SoundEffectManager.Instance.DecreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    /// <summary>
    /// Handle the sound slider on change event
    /// </summary>
    public void OnSoundSliderChange()
    {
        // Get the current sound level volume
        int currentSoundLevel = (int)soundSlider.value;

        // Adjust the volume accordingly
        if (currentSoundLevel > previousSoundLevel)
        {
            for (int i = 0; i < currentSoundLevel - previousSoundLevel; i++)
            {
                IncreaseSoundsVolume();
            }
        }
        else
        {
            for (int i = 0; i < previousSoundLevel - currentSoundLevel; i++)
            {
                DecreaseSoundsVolume();
            }
        }

        // Update the previous sound level value holder
        previousSoundLevel = currentSoundLevel;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicLevelText), musicLevelText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundsLevelText), soundsLevelText);
    }
#endif
    #endregion Validation
}