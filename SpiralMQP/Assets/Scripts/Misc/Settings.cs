using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region UNITES
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion


    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempt = 10; // To prevent the dungeon generation algorithm getting into an infinite loop
    #endregion


    #region ROOM SETTINGS
    // Max number of child corridors leading from a room 
    // Maximum should be 3 although this is not recommended since it can cause the dungeon building to fail and the rooms are more likely NOT fit together
    public const int maxChildCorridor = 3;
    public const float fadeInTime = 0.3f; // Time to fade in the room and door
    public const float doorUnlockDelay = 1f;
    #endregion


    #region ANIMATOR PARAMETERS
    // Instead of parameter names, we hash them into numbers (easier to access and less likely to mess up) 
    // Animator parameters - Player
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp"); // For Dreamy this is a up dash
    public static int rollDown = Animator.StringToHash("rollDown"); // For Dreamy this is a down dash
    public static int rollLeft = Animator.StringToHash("rollLeft"); // For Dreamy this is a left dash
    public static int rollRight = Animator.StringToHash("rollRight"); // For Dreamy this is a right dash
    public static int flipUp = Animator.StringToHash("flipUp");
    public static int flipRight = Animator.StringToHash("flipRight");
    public static int flipLeft = Animator.StringToHash("flipLeft");
    public static int flipDown = Animator.StringToHash("flipDown");
    public static int use = Animator.StringToHash("use");
    public static float baseSpeedForPlayerAnimations = 8f;

    // Animator parameters - Enemy
    public static float baseSpeedForEnemyAnimations = 3f;

    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");

    // Animator parameters - Damageable Decoration
    public static int destroy = Animator.StringToHash("destroy");
    public static string stateDestroyed = "Destroyed";

    // Animator parameters - UI
    public static string healthPercentUI = "HealthPercent";
    public static string isHit = "Hit";
    #endregion ANIMATOR PARAMETERS


    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion


    #region AUDIO
    public const float musicFadeOutTime = 0.5f; // Default music fade out transition
    public const float musicFadeInTime = 0.5f; // Default music fade in transition
    #endregion


    #region FIRING CONTROL
    // If the target distance is less than this value then aim angle will be used(calculated from player pivot point)
    // Else the weapon aim angle will be used (calculated from weapon pivot point)
    public const float useAimAngleDistance = 3.5f;
    #endregion


    #region ASTAR PATHFINDING PARAMETERS
    public const int defaultAstarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const float playerMoveDistanceToRebuildPath = 3f; // If the player moves more than 3 Unity units, start rebuilding the path
    public const float enemyPathRebuildCooldown = 2f; // We don't want the enemy to keep rebuilding the path

    // This is used for optimizing the AStar pathfinding, we want to split the pathfinding calculation into different frames for each enemy to achieve better performance
    public const int targetFrameRateToSpreadPathfindingOver = 60; 
    #endregion ASTAR PATHFINDING PARAMETERS


    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion


    #region UI PARAMETERS
    public const float uiHeartSpacing = 16f; // We can use something else in the future, heart for health is too generic
    public const float uiAmmoIconSpacing = 4f;
    #endregion    


    #region CONTACT DAMAGE PARAMETERS
    public const float contactDamageCollisionResetDelay = 0.5f;
    #endregion

}
