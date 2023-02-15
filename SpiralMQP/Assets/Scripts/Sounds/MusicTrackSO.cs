using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack", menuName = "Scriptable Objects/Sounds/MusicTrack")]
public class MusicTrackSO : ScriptableObject
{
    [Space(10)]
    [Header("MUSIC TRACK DETAILS")]
    [Tooltip("The name for the music track")]
    public string musicName;

    [Tooltip("The audio clip for the music track")]
    public AudioClip musicClip;
    
    [Tooltip("The volume for the music track")]
    [Range(0, 1)]
    public float musicVolume = 1f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(musicName), musicName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicClip), musicClip);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(musicVolume), musicVolume, true);
    }
#endif
    #endregion
}
