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
}
