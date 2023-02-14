using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicController : MonoBehaviour
{
    [Tooltip("Total time it will take the old audio to fade out into the new audio")]
    [SerializeField] float audioFadeTime = .15f;

    [Tooltip("If the game state either has 0 audio or doesnt exist, use this as a default song")]
    [SerializeField] AudioClip defaultAudioClip;
    [Space(10)]
    [SerializeField] StateMusic[] allStateMusic;
    [Space(10)]
    [SerializeField] AudioSource mainMusic;
    public static MusicController Instance;

    private GameState previousGameState = GameState.gameStarted;
    Coroutine musicFade;

    private void Awake()
    {
        Instance = this;
        musicFade = StartCoroutine(SwitchClips(previousGameState));
    }

    private void FixedUpdate()
    {
        //if there is a change in states
        if (previousGameState != GameManager.Instance.GetCurrentGameState())
        {
            OnGameStateChange();
        }

        previousGameState = GameManager.Instance.GetCurrentGameState();
    }

    /// <summary>
    /// What to do on state change
    /// </summary>
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
    /// <param name="searchState"></param>
    /// <returns></returns>
    AudioClip GetStateSong(GameState searchState)
    {
        foreach (StateMusic currentState in allStateMusic)
        {
            //if the current state has the state we are searching for
            if (currentState.ContainsState(searchState))
            {
                StateMusic hasState = currentState;

                //if there is an actual song to pick
                if (hasState.stateMusics.Count > 0) 
                {
                    return currentState.GetRandomClip();
                }
            }
        }

        //if nothing, play the default music
        return defaultAudioClip;
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
