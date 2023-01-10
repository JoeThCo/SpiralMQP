using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // Just making sure no duplicate component for this script is allowed in any object
public class GameManager : SingletonAbstract<GameManager>
{
    [Space(10)]
    [Header("DUNGEON LEVELS")]
    [Tooltip("Create with dungeon level scriptable objects")]
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    [Tooltip("Create with the starting dungeon level for testing, first level = 0")]
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [HideInInspector] public GameState gameState;

    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameState.gameStarted;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleGameState();

        // For testing only
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameState = GameState.gameStarted;
        }
    }

    /// <summary>
    /// Handle game states
    /// </summary>
    private void HandleGameState()
    {
        // Handle game states
        switch (gameState)
        {
            case GameState.gameStarted:
                // Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                break;

        }
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }
    }



    #region Validation
    // Compiler directive: only runs in the unity editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the inspector
    /// </summary>
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif

    #endregion
}
