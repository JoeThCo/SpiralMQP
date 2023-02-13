public enum Orientation
{
    north,
    east,
    south,
    west,
    none
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}

// Used as statemachine for the game
public enum GameState
{
    gameStarted, // Set when the game first started
    playingLevel, // While there are more dungeon rooms in a level that need to be cleared of enemies
    engagingEnemies, // When player enters a room with enemies (not the boss room), when the room has been cleared, it will be reset to playingLevel state
    bossStage, // If all rooms are cleard and player is ready for boss room, then the game will be set to this state, the boss room will have its door unlocked
    engagingBoss, // When the player enters the boss room to battle the boss
    levelCompleted, // When the player has cleared the dungeon and beaten the boss (if there is one in this level) and there are more levels, set to this state
    gameWon, // When the player has cleared the dungeon and beaten the boss (if there is one) and there are no more levels, then set to this state
    gameLost, // If the player is destroyed, the game state is set to this state
    gamePause, // If the game is paused
    dungeonOverviewMap, // If the dungeon overview map is activated, set to this state
    restartGame // After the game "won" or "lost" state has been processed, the game state will be set to this state
}



