using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent] // Just making sure no duplicate component for this script is allowed in any object
public class GameManager : SingletonAbstract<GameManager>
{
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    [Tooltip("Populate with the MessageText TextMeshPro component in the FadeScreenUI")]
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    [Tooltip("Populate with the FadeImage CanvasGroup component in the FadeScreenUI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Tooltip("Populate with pause menu gameobject in hierarchy")]
    [SerializeField] private GameObject pauseMenu;


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

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long totalSoul;
    private int soulMultiplier;
    private InstantiatedRoom bossRoom;
    private bool isFading = false;
    private bool isRestarting = false;


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

        // Subscribe to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

        // Subscribe to soul collected event
        StaticEventHandler.OnSoulsCollected += StaticEventHandler_OnSoulsCollected;

        // Subscribe to multiplier event
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;

        // Subscribe to player destroyed event
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        // Unsubscribe from room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        // Unsubscribe from room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;

        // Unsubscribe from soul collected event
        StaticEventHandler.OnSoulsCollected -= StaticEventHandler_OnSoulsCollected;

        // Unsubscribe from multiplier event
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

        // Unubscribe from player destroyed event
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
    }


    /// <summary>
    /// Handle soul multiplier event
    /// </summary>
    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier)
        {
            soulMultiplier++;
        }
        else
        {
            soulMultiplier--;
        }

        // Clamp between 1 and 10
        soulMultiplier = Mathf.Clamp(soulMultiplier, 1, 10);

        // Call soul amount changed event
        StaticEventHandler.CallSoulAmountChangedEvent(totalSoul, soulMultiplier);
    }


    /// <summary>
    /// Handle total soul amount changed event
    /// </summary>
    private void StaticEventHandler_OnSoulsCollected(SoulsCollectedArgs soulsCollectedArgs)
    {
        // Increase total soul amount
        totalSoul += soulsCollectedArgs.soulCount * soulMultiplier;

        // Call total soul amount changed event
        StaticEventHandler.CallSoulAmountChangedEvent(totalSoul, soulMultiplier);
    }

    /// <summary>
    /// Handle room changed event
    /// </summary>
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }


    /// <summary>
    /// Handle player destroyed event
    /// </summary>
    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
    }


    /// <summary>
    /// Handle room enemies defeated event
    /// </summary>
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }


    /// <summary>
    /// Room enemies defated - test if all dungeon rooms have been cleared of enemies - if so load next dungeon game level
    /// </summary>
    private void RoomEnemiesDefeated()
    {
        // Initialize dungeon as being cleared - but then test each room
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        // Loop through all dungeon rooms to see if cleared of enemies
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            // Skip boss room for time being
            if (keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            // Check if other rooms have been cleared of enemies
            if (!keyValuePair.Value.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        // Set game state
        // If dungeon level completly cleared (i.e. dungeon cleared apart from boss and there is no boss room OR dungeon cleared apart from boss and boss room is also cleared)
        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            // Are there more dungeon levels then
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else
            {
                gameState = GameState.gameWon;
            }
        }
        // Else if dungeon level cleared apart from boss room
        else if (isDungeonClearOfRegularEnemies)
        {
            gameState = GameState.bossStage;

            StartCoroutine(BossStage());
        }
    }


    /// <summary>
    /// Enter boss stage
    /// </summary>
    private IEnumerator BossStage()
    {
        // Activate boss room (have to make sure it's active before we can unlock the door)
        bossRoom.gameObject.SetActive(true);

        // Unlock boss room
        bossRoom.UnlockDoors(0f);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // Display boss message
        yield return StartCoroutine(DisplayMessageRoutine(
            "VERY IMPRESSIVE " + GameResources.Instance.currentPlayer.playerName + "! \n\nNOW FIND AND DEFEAT THE ZODIAC BOSS - " + dungeonLevelList[currentDungeonLevelListIndex].levelName.ToUpper(),
           Color.white, 4f));

        // Fade out canvas
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));
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

        // Set total soul to zero
        totalSoul = 0;

        // Set the multiplier to 1
        soulMultiplier = 1;

        // Set screen to black
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
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
            // ----------------------------------------------------- GAME STARTED -----------------------------------------------------------

            case GameState.gameStarted:
                // Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;

                // Trigger room enemies defeated, since we start in the entrance where there are no enemies (just in case for some reason we want a level to only have entrance room and boss room)
                RoomEnemiesDefeated();
                break;

            // ----------------------------------------------------- GAME STARTED -----------------------------------------------------------



            // ----------------------------------------------------- PLAYING LEVEL -----------------------------------------------------------

            // While playing the level handle the tab key for the dungeon overview map / esc key for pause menu
            case GameState.playingLevel:

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    DisplayDungeonOverviewMap();
                }
                break;

            // ----------------------------------------------------- PLAYING LEVEL -----------------------------------------------------------



            // ----------------------------------------------------- ENGAGING ENEMIES -----------------------------------------------------------

            // While engaging enemies handle the escape key for the pause menu
            case GameState.engagingEnemies:

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;

            // ----------------------------------------------------- ENGAGING ENEMIES -----------------------------------------------------------



            // ----------------------------------------------------- DUNGEON OVERVIEW MAP -----------------------------------------------------------

            // if in the dungeon overview map handle the release of the tab key to clear the map
            case GameState.dungeonOverviewMap:

                // Key released
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    // Clear dungeonOverviewMap
                    DungeonMap.Instance.ClearDungeonOverViewMap();
                }
                break;

            // ----------------------------------------------------- DUNGEON OVERVIEW MAP -----------------------------------------------------------



            // ----------------------------------------------------- BOSS STAGE -----------------------------------------------------------

            // While playing the level and before the boss is engaged, handle the tab key for the dungeon overview map.
            case GameState.bossStage:

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    DisplayDungeonOverviewMap();
                }
                break;

            // ----------------------------------------------------- BOSS STAGE -----------------------------------------------------------



            // ----------------------------------------------------- ENGAGING BOSS -----------------------------------------------------------

            // While engaging the boss handle the escape key for the pause menu
            case GameState.engagingBoss:

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;

            // ----------------------------------------------------- ENGAGING BOSS -----------------------------------------------------------



            // ----------------------------------------------------- LEVEL COMPLETED -----------------------------------------------------------

            // Handle the level being completed
            case GameState.levelCompleted:

                // Display level completed text
                StartCoroutine(LevelCompleted());
                break;

            // ----------------------------------------------------- LEVEL COMPLETED -----------------------------------------------------------



            // ----------------------------------------------------- GAME WON -----------------------------------------------------------

            case GameState.gameWon:

                // Make sure we only call this once when won (since this function will be constantly calling in Update)
                if (previousGameState != GameState.gameWon)
                {
                    StartCoroutine(GameWon());
                }
                break;

            // ----------------------------------------------------- GAME WON -----------------------------------------------------------



            // ----------------------------------------------------- GAME LOST -----------------------------------------------------------

            // Handle the game being lost (only trigger this once - test the previous game state to do this)
            case GameState.gameLost:

                if (previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines(); // Prevent messages if you clear the level just as you get killed
                    StartCoroutine(GameLost());
                }
                break;

            // ----------------------------------------------------- GAME LOST -----------------------------------------------------------



            // ----------------------------------------------------- RESTART GAME -----------------------------------------------------------

            // restart the game
            case GameState.restartGame:

                RestartGame();
                break;

            // ----------------------------------------------------- RESTART GAME -----------------------------------------------------------



            // ----------------------------------------------------- GAME PAUSE -----------------------------------------------------------

            // if the game is paused and the pause menu showing, then pressing escape again will clear the pause menu
            case GameState.gamePause:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;

                // ----------------------------------------------------- GAME PAUSE -----------------------------------------------------------
        }
    }

    /// <summary>
    /// Dungeon Map Screen Display
    /// </summary>
    private void DisplayDungeonOverviewMap()
    {
        // Return if fading
        if (isFading) return;

        // Display dungeonOverviewMap
        DungeonMap.Instance.DisplayDungeonOverViewMap();
    }


    /// <summary>
    /// Pause game menu - also called from resume game button on pause menu
    /// </summary>
    public void PauseGameMenu()
    {
        if (gameState != GameState.gamePause)
        {
            pauseMenu.SetActive(true);
            GetPlayer().playerControl.EnablePlayer(false);

            // Set game state
            previousGameState = gameState;
            gameState = GameState.gamePause;
        }
        else if (gameState == GameState.gamePause)
        {
            pauseMenu.SetActive(false);
            GetPlayer().playerControl.EnablePlayer(true);

            // Set game state
            gameState = previousGameState;
            previousGameState = GameState.gamePause;
        }
    }

    void RestartGame()
    {
        if (!isRestarting)
        {
            LoadingManager.Instance.LoadSceneWithTransistion("Game");
            isRestarting = true;
        }
    }


    /// <summary>
    /// Fade Canvas Group
    /// </summary>
    public IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {
        isFading = true;

        Image image = canvasGroup.GetComponent<Image>();
        image.color = backgroundColor;

        float time = 0;

        while (time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }

        isFading = false;
    }


    /// <summary>
    /// Show level as being completed - load next level
    /// </summary>
    private IEnumerator LevelCompleted()
    {
        // Play next level
        gameState = GameState.playingLevel;

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // Display level completed
        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! \n\nYOU'VE SURVIVED YOUR FATE ... FOR NOW", Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("TAKE YOUR TIME AND GET READY - PRESS RETURN\n\nTO DESCEND FURTHER INTO YOUR FATE", Color.white, 3f));

        // Fade out canvas
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // When player presses the return key proceed to the next level
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        yield return null; // To avoid enter being detected twice

        // Increase index to next level
        currentDungeonLevelListIndex++;

        PlayDungeonLevel(currentDungeonLevelListIndex);
    }


    /// <summary>
    /// Game Won
    /// </summary>
    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        Debug.Log("---------------------------- Game Won ----------------------------");
        yield return new WaitForSeconds(2f);

        // Disable player
        GetPlayer().playerControl.EnablePlayer(false);

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        // Display game won
        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU JOURNEY HAS SUCCESSFULLY ENDED", Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("YOU COLLECTED " + totalSoul.ToString("#,###,##0") + " SOULS", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

        // Set game state to restart game
        gameState = GameState.restartGame;
        RestartGame();
    }


    /// <summary>
    /// Game Lost
    /// </summary>
    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        Debug.Log("--------------------------------- Game Lost ---------------------------------");
        yield return new WaitForSeconds(2f);

        // Disable player
        GetPlayer().playerControl.EnablePlayer(false);

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        // Disable enemies (FindObjectsOfType is resource hungry - but ok to use in this end of game situation)
        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        // Display game lost
        yield return StartCoroutine(DisplayMessageRoutine("THIS IS IT, " + GameResources.Instance.currentPlayer.playerName + "! YOU HAVE SUCCUMBED TO YOUR FATE.", Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("YOU COLLECTED " + totalSoul.ToString("#,###,##0") + " SOULS", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

        // Set game state to restart game
        gameState = GameState.restartGame;
        RestartGame();
    }


    /// <summary>
    /// Set the current room the player is in
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        // The order matters here
        previousRoom = currentRoom;
        currentRoom = room;

        // Testing
        // Debug.Log("Current Room: " + room.prefab.name.ToString());
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

        // Display Dungeon Level Text
        StartCoroutine(DisplayDungeonLevelText());
    }


    /// <summary>
    /// Display the dungeon level text
    /// </summary>
    private IEnumerator DisplayDungeonLevelText()
    {
        // Set screen to black
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));

        GetPlayer().playerControl.EnablePlayer(false);

        // Message
        //string messageText = "LEVEL " + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList[currentDungeonLevelListIndex].levelName.ToUpper();
        string messageText = "Demo LEVEL" + "\n\n" + dungeonLevelList[currentDungeonLevelListIndex].levelName.ToUpper();

        yield return StartCoroutine(DisplayMessageRoutine(messageText, Color.white, 3f));

        GetPlayer().playerControl.EnablePlayer(true);

        // Fade In
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));
    }


    /// <summary>
    /// Display the message text for displaySeconds - if displaySeconds = 0 then the message is displayed until the return key is pressed
    /// </summary>
    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        // Set text
        messageTextTMP.SetText(text);
        messageTextTMP.color = textColor;

        // Display the message for the given time
        if (displaySeconds > 0f)
        {
            float timer = displaySeconds;

            // We either wait for the timer run out or press return key to skip the message
            while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        // Else display the message until the return button is pressed
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }

        yield return null;

        // Clear text
        messageTextTMP.SetText("");
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


    #region Validation
    // Compiler directive: only runs in the unity editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the inspector
    /// </summary>
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(messageTextTMP), messageTextTMP);
        HelperUtilities.ValidateCheckNullValue(this, nameof(canvasGroup), canvasGroup);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif
    #endregion
}
