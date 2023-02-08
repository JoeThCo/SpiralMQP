using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent] // Just making sure no duplicate component for this script is allowed in any object
public class GameManager : SingletonAbstract<GameManager>
{
    [Space(10)]
    [Header("DUNGEON LEVELS")]
    [Tooltip("Create with dungeon level scriptable objects")]
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    [Tooltip("Create with the starting dungeon level for testing, first level = 0")]
    [SerializeField] private int currentDungeonLevelListIndex = 0;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    private GameState gameState;
    private GameState previousGameState;

    public GameState GetCurrentGameState() { return gameState; }

    protected override void Awake()
    {
        // Call base class
        base.Awake();

        // Set player details - saved in current player sciptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        // Instantiate the player at awake
        InstantiatePlayer();
    }

    private void OnEnable()
    {
        // Subscribe to room changed event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    /// <summary>
    /// Handle room changed event
    /// </summary>
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }


    /// <summary>
    /// Create player in scene at position
    /// </summary>
    private void InstantiatePlayer()
    {
        // Instantiate player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab); // We will set the position in "PlayDungeonLevel" function 

        // Initialize player
        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }


    // Start is called before the first frame update
    private void Start()
    {
        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleGameState();

        // For testing only
        if (Input.GetKeyDown(KeyCode.G))
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

    /// <summary>
    /// Updates the current game state. 
    /// Old state gets updated as the state before the update
    /// </summary>
    /// <param name="newState">New State the game will be in</param>
    public void ChangeGameState(GameState newState)
    {
        previousGameState = gameState;
        gameState = newState;
    }

    /// <summary>
    /// Set the current room the player is in
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        // The order matters here
        previousRoom = currentRoom;
        currentRoom = room;

        // Debug
        Debug.Log("Current Room: " + room.prefab.name.ToString());
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }

        // Call static event that room has changed
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        // First, Set player position in about mid-room area
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // Second, Get nearest spawn point in room nearest to player
        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

    }

    /// <summary>
    /// Get the current room the player is in
    /// </summary>
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    /// <summary>
    /// Get the player
    /// </summary>
    public Player GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// Get the player minimap icon
    /// </summary>
    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.playerMinimapIcon;
    }

    /// <summary>
    /// Get the current dungeon level
    /// </summary>
    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }


    /// <summary>
    /// Pauses/Resumes the game
    /// </summary>
    /// <param name="isPausingGame"></param>
    public void PauseGame(bool isPausingGame)
    {
        if (isPausingGame)
        {
            ChangeGameState(GameState.gamePause);
            MenuController.Instance.ShowMenu("Pause");
            Time.timeScale = 0;
        }
        else
        {
            ChangeGameState(GameState.playingLevel);
            MenuController.Instance.ShowMenu("Game");
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Load a scene by name
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(name);
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
