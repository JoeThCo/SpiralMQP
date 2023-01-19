using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempt = 10; // To prevent the dungeon generation algorithm getting into an infinite loop
    #endregion


    #region ROOM SETTINGS
    // Max number of child corridors leading from a room 
    // Maximum should be 3 although this is not recommended since it can cause the dungeon building to fail and the rooms are more likely NOT fit together
    public const int maxChildCorridor = 3;
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
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");

    #endregion
}
