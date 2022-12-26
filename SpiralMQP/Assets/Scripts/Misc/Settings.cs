using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region ROOM SETTINGS

    // Max number of child corridors leading from a room 
    // Maximum should be 3 although this is not recommended since it can cause the dungeon building to fail and the rooms are more likely NOT fit together
    public const int maxChildCorridor = 3;

    #endregion
}
