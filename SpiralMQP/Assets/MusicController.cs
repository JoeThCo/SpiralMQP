using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicController : MonoBehaviour
{
    [SerializeField] float audioFadeTime = .15f;
    [SerializeField] AudioClip noStateAudio;
    [Space(10)]
    [SerializeField] StateMusic[] allStateMusic;
    [Space(10)]
    [SerializeField] AudioSource mainMusic;
    public static MusicController Instance;

    private GameState previousGameState = GameState.gameStarted;
    Coroutine musicFade;

    //check what state the game is in
    //if the state is the same as last frame or last check in, keep playing the music
    //else pick a new song, set volume to 0
    //fade in the new music and fade out the old music
    //when it is done, stop the old music

    private void Awake()
    {
        Instance = this;
        musicFade = StartCoroutine(SwitchClips(previousGameState));
    }

    private void FixedUpdate()
    {
        if (previousGameState != GameManager.Instance.GetCurrentGameState())
        {
            OnGameStateChange();
        }

        previousGameState = GameManager.Instance.GetCurrentGameState();
    }

    public void OnGameStateChange()
    {
        Debug.Log("New State!");
        StopCoroutine(musicFade);
        musicFade = StartCoroutine(SwitchClips(GameManager.Instance.GetCurrentGameState()));
    }


    /// <summary>
    /// Fades out the old clip and plays the new wanted clip
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    IEnumerator SwitchClips(GameState state)
    {
        mainMusic.DOFade(0, audioFadeTime);
        yield return new WaitForSeconds(audioFadeTime * .5f);

        mainMusic.clip = GetStateSong(state);
        mainMusic.Play();

        mainMusic.DOFade(1, audioFadeTime * .5f);
        yield return new WaitForSeconds(audioFadeTime * .5f);
    }

    /// <summary>
    /// Gets the audio clip based on the state.
    /// If no state, plays a default audio clip
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    AudioClip GetStateSong(GameState state)
    {
        foreach (StateMusic currentState in allStateMusic)
        {
            if (currentState.ContainsState(state))
            {
                return currentState.GetRandomClip();
            }
        }

        return noStateAudio;
    }

    [System.Serializable]
    class StateMusic
    {
        public List<GameState> musicStates = new List<GameState>();
        public List<AudioClip> stateMusics = new List<AudioClip>();

        /// <summary>
        /// If contains the searchState
        /// </summary>
        /// <param name="searchState"></param>
        /// <returns></returns>
        public bool ContainsState(GameState searchState)
        {
            return musicStates.Contains(searchState);
        }

        /// <summary>
        /// Gets a random music clip to play
        /// </summary>
        /// <returns></returns>
        public AudioClip GetRandomClip()
        {
            return stateMusics[Random.Range(0, stateMusics.Count)];
        }
    }
}
