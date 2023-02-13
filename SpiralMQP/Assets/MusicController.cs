using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] StateMusic[] allStateMusic;
    [Space(10)]
    [SerializeField] AudioSource mainMusic;
    public static MusicController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnGameStateChange()
    {

    }

    [System.Serializable]
    class StateMusic
    {
        public List<GameState> musicStates = new List<GameState>();
        public List<AudioClip> stateMusics = new List<AudioClip>();

        public bool ContainsState(GameState searchState)
        {
            return musicStates.Contains(searchState);
        }

        public AudioClip GetRandomClip()
        {
            return stateMusics[Random.Range(0, stateMusics.Count)];
        }
    }
}
